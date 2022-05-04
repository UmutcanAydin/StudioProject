using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject door;
    public Transform openedPos;
    bool open = false;
    
    public void Interact()
    {
        open = true;
    }

    private void Update()
    {
        if (!open) return;
        door.transform.position = Vector3.MoveTowards(door.transform.position, openedPos.position, Time.deltaTime * 10f);
    }
}
