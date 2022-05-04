using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public GameObject door;
    public Transform openedPos;
    [HideInInspector] public bool keyFound = false;
    bool open = false;

    public void Interact()
    {
        if(keyFound) open = true;
    }

    private void Update()
    {
        if (!open) return;
        door.transform.position = Vector3.MoveTowards(door.transform.position, openedPos.position, Time.deltaTime * 10f);
    }
}
