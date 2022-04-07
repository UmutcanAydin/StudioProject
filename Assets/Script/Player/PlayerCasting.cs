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

    [Header("Projectile Settings")]
    public Transform projectilePosition;
    public GameObject projectile;
    public float shootCoolDown = 1f;
    float coolDown;
    public float force = 20f;
    bool firing = false;
    bool fired = false;
    Coroutine fireRoutine;
    Rigidbody projectileRGBD;
    GameObject bullet;

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

        //Shooting
        if(coolDown > 0) coolDown -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && coolDown <= 0)
        {
            firing = true;
            fireRoutine = StartCoroutine(Fire());
        }
        else if (Input.GetMouseButtonUp(0))
        {
            firing = false;
            if(fireRoutine != null) StopCoroutine(fireRoutine);
            if(fired) coolDown = shootCoolDown;
            fired = false;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit))
        {
            toTarget = hit.distance;
            distanceFromTarget = hit.distance;            
        }
    }

    IEnumerator Fire()
    {
        while (firing)
        {
            bullet = Instantiate(projectile, projectilePosition.position, projectilePosition.rotation);
            projectileRGBD = bullet.GetComponent<Rigidbody>();

            projectileRGBD.AddForce(bullet.transform.forward * force, ForceMode.Impulse);
            fired = true;
            yield return new WaitForSeconds(shootCoolDown);
        }
    }
}
