using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] public int playerMaxHP = 1000;
    public int currentHP;
    public HealthBar healthBar;
    public int healthDrainRate = 15;
    void Start()
    {
        currentHP = playerMaxHP;
        healthBar.SetMaxHealth(playerMaxHP);
        // Start health drain
        StartCoroutine(DrainHealthOverTime());
    }

    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Clamp(currentHP, 0, playerMaxHP);
        healthBar.SetHealth(currentHP);
    }

    IEnumerator DrainHealthOverTime()
    {
        while (currentHP > 0)
        {
            yield return new WaitForSeconds(.3f); // Drains every second
            TakeDamage(healthDrainRate);
        }
    }
}
