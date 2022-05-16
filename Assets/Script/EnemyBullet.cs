using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float damage = 10f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            player.TakeDamage(damage);
            AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.hitPlayerFX, 1f);
        }
        Destroy(gameObject);
    }
}
