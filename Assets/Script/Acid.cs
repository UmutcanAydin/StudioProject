using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(1000);
        }
        else if (other.GetComponent<MeleeEnemy>())
        {
            MeleeEnemy meleeEnemy = other.GetComponent<MeleeEnemy>();
            meleeEnemy.TakeDamage(1000);
        }
        else if (other.GetComponent<PlayerStats>())
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            player.TakeDamage(1000);
        }
        //Destroy(gameObject);
    }
}
