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

    Rigidbody rgbd;
    PlayerStats playerStats;
    RangeSensor sensor;
    bool firing = false;
    GameObject bullet;
    Rigidbody projectileRGBD;
    public float force = 20f;
    public float shootCoolDown = 1f;
    Coroutine fireRoutine;

    private void Awake()
    {
        rgbd = GetComponent<Rigidbody>();
        playerStats = FindObjectOfType<PlayerStats>();
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
            //rgbd.velocity = transform.forward * movementSpeed;
            rgbd.velocity = new Vector3(transform.forward.x * movementSpeed, rgbd.velocity.y, transform.forward.z * movementSpeed);

            if (Vector3.Distance(transform.position, nearest.transform.position) < stopRange)
            {
                rgbd.velocity = Vector3.zero;
                if (!firing)
                {
                    firing = true;
                    fireRoutine = StartCoroutine(Fire());
                }
            }
            else
            {
                firing = false;
                if (fireRoutine != null) StopCoroutine(fireRoutine);
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
            Destroy(gameObject);
        }
    }

    IEnumerator Fire()
    {
        while (firing)
        {
            bullet = Instantiate(projectile, projectilePosition.position, projectilePosition.rotation);
            projectileRGBD = bullet.GetComponent<Rigidbody>();

            projectileRGBD.AddForce(bullet.transform.forward * force, ForceMode.Impulse);
            yield return new WaitForSeconds(shootCoolDown);
        }
    }
}
