using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 10f;
    public PlayerCasting player;
    CapsuleCollider cld;
    Rigidbody rgbd;
    bool goToPlayer = false;

    private void Awake()
    {
        cld = GetComponentInChildren<CapsuleCollider>();
        rgbd = GetComponent<Rigidbody>();
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
            AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.hitEnemyFX, 1f);
        }
        else if (other.GetComponent<MeleeEnemy>())
        {
            MeleeEnemy meleeEnemy = other.GetComponent<MeleeEnemy>();
            meleeEnemy.TakeDamage(damage);

            meleeEnemy.GetComponent<Rigidbody>().AddForce(new Vector3(0, meleeEnemy.transform.up.y * 5, transform.forward.z * 2), ForceMode.Impulse);
            meleeEnemy.hit = true;
            meleeEnemy.RestartHitState();
            AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.hitEnemyFX, 1f);
        }
        cld.enabled = false;
        rgbd.velocity = Vector3.zero;
        rgbd.AddForce(Vector3.up * 5, ForceMode.Impulse);
        StartCoroutine(GoToPlayer());
        //Destroy(gameObject);
    }

    void Update()
    {
        if (goToPlayer)
        {
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 2.5f, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 50 * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerPos) < 2f)
            {
                player.ammoNum++;
                player.UpdateText();
                Destroy(gameObject);
            }
        }

    }

    IEnumerator GoToPlayer()
    {
        yield return new WaitForSeconds(1f);
        goToPlayer = true;
    }
}
