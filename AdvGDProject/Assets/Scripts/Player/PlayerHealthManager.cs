using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.SceneManagement;
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
    void Update()
    {
        // Check for "R" key press to reset the stage
        if (Input.GetKeyDown(KeyCode.R))
        {
            Die();
        }
    }
    public void TakeDamage(int damageAmount)
    {
        currentHP -= damageAmount;
        currentHP = Mathf.Clamp(currentHP, 0, playerMaxHP);
        healthBar.SetHealth(currentHP);

        // Check if health is 0 or below, and if so, call Die method
        if (currentHP <= 0)
        {
            Die();
        }
    }

    IEnumerator DrainHealthOverTime()
    {
        while (currentHP > 0)
        {
            yield return new WaitForSeconds(.3f); // Drains every second
            TakeDamage(healthDrainRate);
        }
    }

    public void Die(){

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
