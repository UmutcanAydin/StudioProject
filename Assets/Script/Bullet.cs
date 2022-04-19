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
            Vector3 playerPos = new Vector3(player.transform.position.x, player.transform.position.y + 1.5f, player.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, playerPos, 40 * Time.deltaTime);
            if (Vector3.Distance(transform.position, playerPos) < 1f)
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
