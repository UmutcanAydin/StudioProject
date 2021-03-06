using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public int healthCollectibleCount = 0;
    public float amountToCure = 10f;
    public TextMeshProUGUI healthItemText;

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

    SceneManagement mngr;
    FirstPersonController firstPersonController;

    private void Awake()
    {
        mngr = FindObjectOfType<SceneManagement>();
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

        healthItemText.text = healthCollectibleCount.ToString();
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

        //Using Health Item
        if (Input.GetKeyDown(KeyCode.Q) && healthCollectibleCount > 0)
        {
            healthCollectibleCount--;
            healthItemText.text = healthCollectibleCount.ToString();
            RestoreHealth(amountToCure);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            mngr.RestartLevel();
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
    }

    public void AddHealthItem(int amount)
    {
        healthCollectibleCount += amount;
        healthItemText.text = healthCollectibleCount.ToString();
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