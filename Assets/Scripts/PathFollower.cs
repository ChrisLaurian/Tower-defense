using Assets.Scripts;
using UnityEngine;

// Enum para definir el tipo de camino (tierra o aire)
public class PathFollower : MonoBehaviour
{
    public enum PathType { Ground, Air }

    public PathType Type;            // Tipo de camino (tierra o aire)
    public float Speed;              // Velocidad de movimiento a lo largo de la curva
    public float OffsetAmount;       // Rango de desplazamiento aleatorio desde el camino

    private Vector2 PositionOffset;  // Desplazamiento aleatorio aplicado al objeto
    private Path path;               // Referencia al camino definido
    private int segmentIndex;        // Índice del segmento actual de la curva

    // Puntos de control de la curva de Bezier
    private Vector2 A, B, C, D;
    private Vector2 v1, v2, v3;
    private float t;                 // Parámetro de tiempo para controlar la posición a lo largo de la curva

    void OnEnable()
    {
        // Determina qué camino seguir basado en el tipo especificado
        switch (Type)
        {
            case PathType.Ground:
                path = GameObject.Find("GroundPath").GetComponent<PathCreator>().path;
                break;
            case PathType.Air:
                path = GameObject.Find("AirPath").GetComponent<PathCreator>().path;
                break;
        }

        segmentIndex = 0;
        PositionOffset = Random.insideUnitCircle * Random.Range(-OffsetAmount, OffsetAmount);

        // Calcula los puntos de control del primer segmento y establece la posición inicial del objeto
        RecomputeSegment();
        transform.position = A;
    }

    void FixedUpdate()
    {
        // Si hemos alcanzado el final del camino, salimos del método
        if (segmentIndex >= path.NumSegments) return;

        // Avanzamos el parámetro t a lo largo de la curva de Bezier
        if (t >= 1.0f)
        {
            segmentIndex++;
            if (segmentIndex >= path.NumSegments) return;

            RecomputeSegment(); // Calculamos los puntos de control del siguiente segmento
        }

        // Calculamos la tangente a la curva en el punto actual para orientar el objeto
        var tangent = t * t * v1 + t * v2 + v3;
        t = t + Time.deltaTime * Speed / tangent.magnitude; // Avanzamos t proporcionalmente a la velocidad

        // Actualizamos la posición del objeto utilizando la evaluación de la curva de Bezier
        transform.position = Bezier.EvaluateCubic(A, B, C, D, t);

        // Orientamos el objeto en la dirección de la tangente calculada
        transform.eulerAngles = new Vector3(0.0f, 0.0f, MathHelpers.Angle(tangent, Vector2.right));

        // Si el objeto pertenece a la capa "enemy", actualizamos su estado en el administrador de enemigos
        if (gameObject.layer == LayerMask.NameToLayer("enemy"))
            EnemyManagerScript.Instance.UpdateEnemy(gameObject, path.NumSegments - segmentIndex - t);
    }

    // Método para recalcular los puntos de control del segmento actual de la curva de Bezier
    private void RecomputeSegment()
    {
        // Obtenemos los puntos de control del segmento actual de la curva
        var segment = path.GetPointsInSegment(segmentIndex);

        // Aplicamos el desplazamiento aleatorio a cada punto de control
        A = segment[0] + PositionOffset;
        B = segment[1] + PositionOffset;
        C = segment[2] + PositionOffset;
        D = segment[3] + PositionOffset;

        // Calculamos las derivadas de Bezier para suavizar el movimiento a lo largo de la curva
        v1 = -3 * A + 9 * B - 9 * C + 3 * D;
        v2 = 6 * A - 12 * B + 6 * C;
        v3 = -3 * A + 3 * B;

        t = 0; // Reiniciamos el parámetro t para iniciar desde el comienzo del segmento
    }
}
