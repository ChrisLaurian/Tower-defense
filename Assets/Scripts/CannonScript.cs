using System.Collections.Generic;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

// Este script maneja la lógica de disparo y rotación de una torreta en el juego.
public class CannonScript : MonoBehaviour
{
    // Prototipo de la bala que será disparada.
    public GameObject BulletPrototype;

    // Período de disparo de la torreta.
    public float ShootingPeriod;

    // Rango de la torreta.
    public float Range;

    // Velocidad de la bala.
    public float BulletSpeed;

    // Daño de la bala.
    public float Damage;

    // Velocidad de rotación de la torreta.
    public float RotationSpeed;

    // Lista de enemigos a los que la torreta puede atacar.
    public List<GameObject> Enemies;

    // Referencia al objeto de marcador de bala.
    private GameObject bulletPlaceholder;

    // Tiempo restante para el próximo disparo.
    private float timeToShoot = 0.0f;

    // Lista de etiquetas de los enemigos.
    private List<string> enemyTags;

    // Método Start se llama antes del primer frame.
    void Start()
    {
        // Obtiene las etiquetas de los enemigos de la lista de enemigos.
        enemyTags = Enemies.Select(e => e.tag).ToList();

        // Encuentra un enemigo en rango infinito y rota la torreta hacia ese enemigo.
        var enemy = EnemyManagerScript.Instance.GetEnemyInRange(transform.position, float.PositiveInfinity, enemyTags);
        var angle = MathHelpers.Angle(enemy.transform.position - transform.position, transform.up);
        transform.eulerAngles = new Vector3(0, 0, angle);

        // Encuentra y asigna el marcador de bala.
        bulletPlaceholder = transform.Find("Rocket").gameObject;
    }

    // Método FixedUpdate se llama a intervalos fijos y es utilizado para actualizar física.
    void FixedUpdate()
    {
        // Obtiene un enemigo en el rango de la torreta.
        var enemy = EnemyManagerScript.Instance.GetEnemyInRange(transform.position, Range, enemyTags);

        if (enemy != null)
        {
            // Rota la torreta hacia el enemigo.
            TurnToEnemy(enemy.transform.position + enemy.transform.right * 32);

            // Si es tiempo de disparar, crea y dispara una bala.
            if (timeToShoot < 0)
            {
                var bullet = Pool.Instance.ActivateObject(BulletPrototype.tag);
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;

                var bulletScript = bullet.GetComponent<FlyingShotScript>();
                bulletScript.Speed = BulletSpeed;
                bulletScript.Range = Range;
                bulletScript.Direction = transform.transform.up;
                bulletScript.Damage = Damage;
                bulletScript.Target = enemy;
                bulletScript.EnemyTags = enemyTags;
                bulletScript.Turret = transform;

                bullet.SetActive(true);

                timeToShoot = ShootingPeriod;

                if (bulletPlaceholder != null) bulletPlaceholder.SetActive(false);
                return;
            }
        }
        else
        {
            // Si no hay enemigos en rango, rota hacia el enemigo más cercano dentro del doble de rango.
            var closestEnemy = EnemyManagerScript.Instance.GetClosestEnemyInRange(transform.position, Range * 2, enemyTags);
            if (closestEnemy != null) TurnToEnemy(closestEnemy.transform.position + closestEnemy.transform.right * 32);
        }

        // Activa el marcador de bala si es necesario.
        if (bulletPlaceholder != null && timeToShoot < ShootingPeriod / 8.0f && !bulletPlaceholder.activeSelf)
        {
            bulletPlaceholder.SetActive(true);
        }

        // Reduce el tiempo restante para el próximo disparo.
        timeToShoot -= Time.deltaTime;
    }

    // Método privado para rotar la torreta hacia el enemigo.
    private void TurnToEnemy(Vector2 position)
    {
        var direction = position - (Vector2)transform.position;
        var angle = MathHelpers.Angle(direction, transform.up);
        transform.Rotate(0, 0, Mathf.Clamp(angle, -10, 10) * RotationSpeed * Time.deltaTime);
    }
}
