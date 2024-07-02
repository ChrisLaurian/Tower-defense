using Assets.Scripts;  // Importa el espacio de nombres para acceder a clases y funciones definidas en Scripts

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : FlyingShotScript  // Clase que extiende de FlyingShotScript, representa el comportamiento de un cohete
{
    public float Inertia;  // Inercia del cohete
    public float InitialAcceleration;  // Aceleración inicial del cohete
    public Vector2 ShadowOffset;  // Desplazamiento de la sombra

    private Vector2 velocity;  // Velocidad del cohete
    private float acceleration;  // Aceleración actual del cohete
    private Transform shadow;  // Transform del objeto sombra

    private float startTime;  // Tiempo de inicio para el cohete

    // Se llama cuando el cohete se activa por primera vez
    void OnEnable()
    {
        Pool.Instance.ActivateObject("missileSoundEffect").SetActive(true);  // Activa el efecto de sonido del misil desde el Pool

        shadow = transform.Find("Shadow");  // Encuentra el objeto sombra
        shadow.position = transform.position;  // Establece la posición inicial de la sombra igual a la del cohete
        acceleration = InitialAcceleration;  // Establece la aceleración inicial
        startTime = 1.0f;  // Inicializa el tiempo de inicio
    }

    // Se llama una vez por fotograma
    void FixedUpdate()
    {
        if (startTime >= 0)
        {
            startTime -= Time.deltaTime;  // Reduce el tiempo de inicio
            transform.rotation = Turret.rotation;  // Rota hacia la dirección del Turret
            Direction = Turret.up;  // Establece la dirección hacia arriba del Turret como la dirección del cohete
            velocity = Direction.normalized;  // Normaliza y asigna la dirección como la velocidad del cohete
            return;  // Sale de la función
        }

        if (Target == null || !Target.activeSelf)
        {
            Target = EnemyManagerScript.Instance.GetClosestEnemyInRange(transform.position, float.PositiveInfinity, EnemyTags);  // Obtiene el enemigo más cercano dentro del rango
            if (Target == null) BlowUp();  // Si no hay objetivo, explota
        }

        var direction = Target.transform.position - transform.position;  // Calcula la dirección hacia el objetivo
        var angle = MathHelpers.Angle(direction, transform.up) * Time.deltaTime * Inertia * Mathf.Pow(velocity.sqrMagnitude, 0.5f);  // Calcula el ángulo basado en la inercia y la velocidad del cohete

        velocity = velocity.Rotate(angle);  // Rota la velocidad del cohete según el ángulo calculado
        acceleration *= 0.95f;  // Reduce gradualmente la aceleración
        velocity += velocity * acceleration * Time.deltaTime;  // Ajusta la velocidad con la aceleración

        transform.position += (Vector3)velocity;  // Actualiza la posición del cohete según la velocidad
        transform.up = velocity;  // Orienta el cohete hacia arriba según la velocidad

        shadow.position = transform.position + (Vector3)ShadowOffset * (InitialAcceleration - acceleration) / InitialAcceleration;  // Ajusta la posición de la sombra basada en la aceleración
        shadow.rotation = transform.rotation;  // Asigna la rotación de la sombra igual a la del cohete
    }

    // Override del método BlowUp de FlyingShotScript, se llama cuando el cohete explota
    public override void BlowUp()
    {
        Pool.Instance.ActivateObject("shortExplosionSoundEffect").SetActive(true);  // Activa el efecto de sonido de explosión corta desde el Pool
        base.BlowUp();  // Llama al método base de FlyingShotScript para realizar la explosión
    }
}
