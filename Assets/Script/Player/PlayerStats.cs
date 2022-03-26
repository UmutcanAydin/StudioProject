using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] Slider healthSlider;
    [SerializeField] float maxHealth = 100f;
    [SerializeField] float currentHealth = 100f;

    [Header("Stamina Settings")]
    [SerializeField] Slider staminaSlider;
    [SerializeField] float maxStamina = 100f;
    [SerializeField] float currentStamina = 100f;
    [SerializeField] float runningStaminaWasteAmount = 0.1f;
    [SerializeField] float waitTimeBeforeRestorationStart = 1f;
    [SerializeField] float staminaRestorationAmount = 0.1f;
    [HideInInspector] public bool canRun = true;
    Coroutine restoreStaminaRoutine;

    bool startRestoringStamina = false;
    bool restore = false;

    FirstPersonController firstPersonController;

    private void Awake()
    {
        firstPersonController = GetComponent<FirstPersonController>();
    }

    private void Start()
    {
        //Setting up slider
        healthSlider.maxValue = maxHealth;
        staminaSlider.maxValue = maxStamina;

        //Setting up initial health and stamina
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }

    private void Update()
    {
        //Clamp health and stamina between 0 and max values
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        //Update slider values every frame
        healthSlider.value = currentHealth;
        staminaSlider.value = currentStamina;

        #region Running Stamina Settings
        if (!firstPersonController.m_IsWalking && firstPersonController.m_MoveDir.z != 0)
        {
            //if running start wasting stamina
            startRestoringStamina = false;
            restore = false;
            WasteStamina(runningStaminaWasteAmount);
        }
        else if (!startRestoringStamina)
        {
            //if stopped running and stamina restoration didn't start yet
            startRestoringStamina = true;
            restoreStaminaRoutine = StartCoroutine(RestoreStamina());
        }

        if (currentStamina > runningStaminaWasteAmount) canRun = true;
        #endregion

        //Stamina Restoration
        if (restore)
        {
            currentStamina += staminaRestorationAmount;
            if (currentStamina >= maxStamina) restore = false;
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
    }

    public void WasteStamina(float amount)
    {
        currentStamina -= amount;
        if (currentStamina <= runningStaminaWasteAmount) canRun = false;
    }

    IEnumerator RestoreStamina()
    {
        yield return new WaitForSeconds(waitTimeBeforeRestorationStart);
        restore = true;
    }
}