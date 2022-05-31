using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MutantBoss : MonoBehaviour
{
    public GameObject enemyToSpawn;
    public Transform enemySpawnPos;
    LevelManager lvlManager;
    public float maxHealth = 100;
    public float currentHealth = 100;
    GameObject bullet;
    Rigidbody projectileRGBD;
    public float force = 20f;
    public float meleeDamage = 50f;
    public Slider healthSlider;
    public Transform projectilePosition;
    public GameObject projectile;
    public float rangedDelay = 3f;
    public float meleeDelay = 2f;
    PlayerStats playerStats;
    Animator animator;
    bool enemySpawned = false;
    bool firing = false;
    bool melee = false;

    [Header("Levels")]
    public bool level1Enemy = true;
    public bool level2Enemy = false;
    public bool level3Enemy = false;
    public bool level4Enemy = false;
    public bool level5Enemy = false;
    public bool level6Enemy = false;


    private void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        animator = GetComponent<Animator>();
        lvlManager = FindObjectOfType<LevelManager>();
    }

    private void Start()
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = maxHealth;
    }

    private void Update()
    {
        if (!lvlManager.level5Handled) return;
        healthSlider.transform.LookAt(playerStats.transform);

        if (Vector3.Distance(transform.position, playerStats.transform.position) > 40f && (!enemySpawned && !firing))
        {
            int random = Random.Range(0, 2);
            switch (random)
            {
                case 0:
                    enemySpawned = true;
                    animator.SetTrigger("Spawn");
                    StartCoroutine(SpawnAttack());
                    break;
                case 1:
                    firing = true;
                    animator.SetTrigger("Ranged");
                    StartCoroutine(RangedAttack());
                    break;
            }

        }
        else if (Vector3.Distance(transform.position, playerStats.transform.position) <= 40f && !melee)
        {
            Vector3 enemyDirectionLocal = transform.InverseTransformPoint(playerStats.transform.position);
            melee = true;

            if (enemyDirectionLocal.x < 0)
            {
                animator.SetTrigger("MeleeRight");
                StartCoroutine(MeleeAttack());
            }
            else if (enemyDirectionLocal.x > 0)
            {
                animator.SetTrigger("MeleeLeft");
                StartCoroutine(MeleeAttack());
            }
        }
    }

    IEnumerator SpawnAttack()
    {
        yield return new WaitForSeconds(rangedDelay);
        enemySpawned = false;
    }

    IEnumerator RangedAttack()
    {
        yield return new WaitForSeconds(rangedDelay);
        firing = false;
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(meleeDelay);
        melee = false;
    }

    //Call from anim
    public void Puke()
    {
        Instantiate(enemyToSpawn, enemySpawnPos.position, Quaternion.identity);
    }

    //Call from anim
    public void Ranged()
    {
        bullet = Instantiate(projectile, projectilePosition.position, projectilePosition.rotation);
        projectileRGBD = bullet.GetComponent<Rigidbody>();
        bullet.transform.LookAt(playerStats.transform.position);

        projectileRGBD.AddForce(bullet.transform.forward * force, ForceMode.Impulse);
    }

    public void Melee()
    {
        AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.hitPlayerFX, 1f);
        playerStats.TakeDamage(meleeDamage);
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            if (level6Enemy) lvlManager.level6DeadEnemyCount++;
            Destroy(gameObject);
        }
    }
}
