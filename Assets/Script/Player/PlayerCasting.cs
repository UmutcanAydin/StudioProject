using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int ammoNum = 5;
    public TextMeshProUGUI ammoText;
    public Transform projectilePosition;
    public Bullet projectile;
    public float shootCoolDown = 1f;
    float coolDown;
    public float force = 20f;
    bool firing = false;
    bool fired = false;
    Coroutine fireRoutine;
    Rigidbody projectileRGBD;
    Bullet bullet;

    [Header("Melee Settings")]
    public BoxCollider injector;
    Animator animator;
    bool canAttack = true; 

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        UpdateText();
    }

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

        if (Input.GetMouseButton(0) && coolDown <= 0 && !firing)
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            MeleeAttack();
        }
    }

    IEnumerator Fire()
    {
        while (firing)
        {
            if (ammoNum <= 0) yield break;

            bullet = Instantiate(projectile, projectilePosition.position, projectilePosition.rotation);
            projectileRGBD = bullet.GetComponent<Rigidbody>();
            bullet.player = this;

            projectileRGBD.AddForce(bullet.transform.forward * force, ForceMode.Impulse);
            ammoNum--;
            UpdateText();
            fired = true;
            yield return new WaitForSeconds(shootCoolDown);
        }
    }

    public void UpdateText()
    {
        ammoText.text = ammoNum.ToString();
    }

    public void MeleeAttack()
    {
        if (canAttack)
        {
            canAttack = false;
            animator.Play("Attack");
            AudioManager.Instance.PlayWithoutPitch(AudioManager.Instance.syringeFX, 0.2f);
        }
    }

    //Called from animation
    public void EnableMeleeCollider()
    {
        injector.enabled = true;
        StartCoroutine(DisableMeleeCollider());
    }

    IEnumerator DisableMeleeCollider()
    {
        yield return new WaitForSeconds(0.2f);
        injector.enabled = false;
        canAttack = true;
    }
}
