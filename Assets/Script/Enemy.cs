using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SensorToolkit;

public class Enemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
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
