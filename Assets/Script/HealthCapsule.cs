using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCapsule : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            other.GetComponent<PlayerStats>().AddHealthItem(1);
            Destroy(gameObject);
        }
    }
}
