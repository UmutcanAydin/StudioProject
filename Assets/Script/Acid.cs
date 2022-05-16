using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Acid : MonoBehaviour
{
    public Transform respawnPoint;

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
            CharacterController player = other.GetComponent<CharacterController>();
            player.enabled = false;
            player.transform.position = respawnPoint.position;
            player.enabled = true;
        }
        //Destroy(gameObject);
    }
}
