using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public Door connectedDoor;
    LevelManager levelManager;

    private void Awake()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerStats>())
        {
            connectedDoor.keyFound = true;
            connectedDoor.Interact();
            levelManager.checkPointIndex++;
            levelManager.Save();
            Destroy(gameObject);
        }
    }
}
