using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField] public float playerHP = 100f;
    public TextMeshProUGUI playerHPText;
    public bool isGameOver;

    void Start()
    {
        playerHPText.text = (playerHP).ToString();
        isGameOver = false;
    }

    void Update()
    {
        if (isGameOver)
        {
            playerHPText.text = "Game Over";
        }

    }

    public void TakeDamage(float damageAmount)
    {
        playerHP -= damageAmount;
        playerHPText.text = (playerHP).ToString();
        if (playerHP <= 0)
        {
            isGameOver = true;
        }
    }
}
