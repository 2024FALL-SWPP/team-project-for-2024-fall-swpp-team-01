using System;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] public static float maxHP = 100;
    [SerializeField] public static float maxSP = 100;
    [SerializeField] private float SPIncrementSpeed = 15f;
    
    private float currentHP;
    private float currentSP;

    public Stage_UIManager UIManager;
    
    public event Action<float> OnHealthChanged;
    public event Action OnDeath;
    
    // Start is called before the first frame update
    void Start()
    {
        if(UIManager == null)
            Debug.LogError("UIManager Not Found");
        currentHP = maxHP;
        currentSP = maxSP;
    }

    // Update is called once per frame
    void Update()
    {
        RefillStamina();
    }

    void takeDamage(float amount)
    {
        currentHP -= amount;
        // Invoke();
        // if (currentHealth <= 0)
    }

    public float getMaxHP()
    {
        return maxHP;
    }

    public float getCurrentHP()
    {
        return currentHP;
    }

    public float getMaxSP()
    {
        return maxSP;
    }

    public float getCurrentSP()
    {
        return currentSP;
    }

    public void updateCurrentHP(float value, bool isAbsolute)
    {
        currentHP = isAbsolute ? value : currentHP + value;
        UIManager.UpdateCurrentHP();
        return;
    }

    public void updateCurrentSP(float value, bool isAbsolute)
    {
        currentSP = isAbsolute ? value : currentSP + value;
        UIManager.UpdateSP();
        return;
    }

    private void RefillStamina()
    {
        updateCurrentSP(SPIncrementSpeed * Time.deltaTime, false);
        currentSP = Mathf.Clamp(currentSP, 0, maxSP);
        return;
    }
}
