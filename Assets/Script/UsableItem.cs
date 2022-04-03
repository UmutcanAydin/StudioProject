using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        print("Using item: " + gameObject.name);
    }
}
