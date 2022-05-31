using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Injector : MonoBehaviour
{
    public float damage = 50f;
    BoxCollider cld;

    private void Awake()
    {
        cld = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            Enemy enemy = other.GetComponent<Enemy>();
            enemy.TakeDamage(damage);

            enemy.GetComponent<Rigidbody>().AddForce(new Vector3(0, enemy.transform.up.y * 2, transform.forward.z * 2) * 2, ForceMode.Impulse);
            enemy.hit = true;
            enemy.RestartHitState();
            cld.enabled = false;
        }
        else if (other.GetComponent<MeleeEnemy>())
        {
            MeleeEnemy enemy = other.GetComponent<MeleeEnemy>();
            enemy.TakeDamage(damage);

            enemy.GetComponent<Rigidbody>().AddForce(new Vector3(0, enemy.transform.up.y * 2, transform.forward.z * 2) * 2, ForceMode.Impulse);
            enemy.hit = true;
            enemy.RestartHitState();
            cld.enabled = false;
        }
    }
}
