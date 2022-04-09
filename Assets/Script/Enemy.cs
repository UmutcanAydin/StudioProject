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

    Rigidbody rgbd;
    PlayerStats playerStats;
    RangeSensor sensor;

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
        if(nearest != null) transform.LookAt(nearest.transform);
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
}
