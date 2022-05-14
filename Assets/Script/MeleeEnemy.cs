using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SensorToolkit;

public class MeleeEnemy : MonoBehaviour
{
    public float maxHealth = 100;
    public float currentHealth = 100;
    public float movementSpeed = 2f;
    public float stopRange = 10f;
    public Slider healthSlider;
    public BoxCollider attackBox;

    Rigidbody rgbd;
    PlayerStats playerStats;
    RangeSensor sensor;
    bool firing = false;
    public float shootCoolDown = 1f;
    Coroutine fireRoutine;
    [HideInInspector] public bool hit = false;

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
            if (hit) return;
            //rgbd.velocity = transform.forward * movementSpeed;
            rgbd.velocity = new Vector3(transform.forward.x * movementSpeed, rgbd.velocity.y, transform.forward.z * movementSpeed);

            if (Vector3.Distance(transform.position, nearest.transform.position) < stopRange)
            {
                rgbd.velocity = new Vector3(0, rgbd.velocity.y, 0);
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
            StartCoroutine(AttackDelay());
            yield return new WaitForSeconds(shootCoolDown);
        }
    }

    IEnumerator AttackDelay()
    {
        attackBox.enabled = true;
        yield return new WaitForSeconds(0.2f);
        attackBox.enabled = false;
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
