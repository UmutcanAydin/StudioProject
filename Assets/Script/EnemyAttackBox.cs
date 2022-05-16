using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackBox : MonoBehaviour
{
    public float damage = 10f;
    BoxCollider cld;

    private void Awake()
    {
        cld = GetComponent<BoxCollider>();
        cld.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            PlayerStats player = other.GetComponent<PlayerStats>();
            player.TakeDamage(damage);
            AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.hitPlayerFX, 1f);
        }
        cld.enabled = false;
    }
}
