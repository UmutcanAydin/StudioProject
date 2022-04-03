using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCasting : MonoBehaviour
{
    [SerializeField] string selectableTag = "Selectable";
    [SerializeField] Material highlightMaterial;
    [SerializeField] float selectionRaycastDistance = 3f;

    Material defaultMaterial;
    Transform currentSelection;
    Renderer rend;
    IInteractable item;

    public static float distanceFromTarget;
    public float toTarget;

    void Update()
    {
        //Deselect
        if(currentSelection != null)
        {
            rend = currentSelection.GetComponent<Renderer>();
            rend.material = defaultMaterial;
            currentSelection = null;
            defaultMaterial = null;
            item = null;
        }

        //Select
        Ray selectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit selectionHit;
        if (Physics.Raycast(selectionRay, out selectionHit, selectionRaycastDistance))
        {
            Transform selection = selectionHit.transform;
            if (selection.CompareTag(selectableTag))
            {
                rend = selection.GetComponent<Renderer>();
                if (rend != null)
                {
                    defaultMaterial = rend.material;
                    rend.material = highlightMaterial;

                    currentSelection = selection;
                    if(item == null) item = selection.GetComponent<IInteractable>();
                    if (Input.GetButtonDown("Interact"))
                    {
                        if(item != null)item.Interact();
                    }
                }
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            toTarget = hit.distance;
            distanceFromTarget = hit.distance;            
        }
    }
}
