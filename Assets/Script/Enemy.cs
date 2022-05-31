using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SensorToolkit;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float movementSpeed = 2f;
    public float stopRange = 10f;
    public Slider healthSlider;
    public Transform projectilePosition;
    public GameObject projectile;
    public bool flyingType = false;

    [Header("Levels")]
    public bool level1Enemy = true;
    public bool level2Enemy = false;
    public bool level3Enemy = false;
    public bool level4Enemy = false;
    public bool level5Enemy = false;
    public bool level6Enemy = false;

    Rigidbody rgbd;
    PlayerStats playerStats;
    RangeSensor sensor;
    bool firing = false;
    GameObject bullet;
    Rigidbody projectileRGBD;
    public float force = 20f;
    public float shootCoolDown = 1f;
    Coroutine fireRoutine;
    [HideInInspector] public bool hit = false;
    LevelManager lvlManager;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
        playerStats = FindObjectOfType<PlayerStats>();
        lvlManager = FindObjectOfType<LevelManager>();
        sensor = GetComponent<RangeSensor>();
    }

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    private void Update()
    {
        healthSlider.transform.LookAt(playerStats.transform);

        GameObject nearest = sensor.GetNearest();
        if (nearest != null)
        {
            transform.LookAt(nearest.transform);
            if (hit) return;
            
            if (flyingType) rgbd.velocity = transform.forward * movementSpeed;
            else rgbd.velocity = new Vector3(transform.forward.x * movementSpeed, rgbd.velocity.y, transform.forward.z * movementSpeed);

            if (Vector3.Distance(transform.position, nearest.transform.position) < stopRange)
            {
                rgbd.velocity = new Vector3(0, rgbd.velocity.y, 0);
                if (!firing)
                {
                    firing = true;
                    if(fireRoutine == null) fireRoutine = StartCoroutine(Fire());
                }
            }
            else
            {
                firing = false;
                if (fireRoutine != null)
                {
                    StopCoroutine(fireRoutine);
                    fireRoutine = null;
                }
            }
        }
        else
        {
            rgbd.velocity = new Vector3(0, rgbd.velocity.y, 0);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (level1Enemy) lvlManager.level1DeadEnemyCount++;
            else if (level3Enemy) lvlManager.level3DeadEnemyCount++;
            else if (level4Enemy) lvlManager.level4DeadEnemyCount++;
            else if (level5Enemy) lvlManager.level5DeadEnemyCount++;
            Destroy(gameObject);
        }
    }

    IEnumerator Fire()
    {
        while (firing)
        {
            bullet = Instantiate(projectile, projectilePosition.position, projectilePosition.rotation);
            projectileRGBD = bullet.GetComponent<Rigidbody>();
            bullet.transform.LookAt(playerStats.transform);
            projectileRGBD.AddForce(bullet.transform.forward * force, ForceMode.Impulse);
            yield return new WaitForSeconds(shootCoolDown);
        }
    }

    public void RestartHitState()
    {
        StartCoroutine(RHitState());
    }

    IEnumerator RHitState()
    {
        yield return new WaitForSeconds(1f);
        hit = false;
    }
}
