using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static int playerHP = 100;
    public TextMeshProUGUI playerHPText;
    public static bool isGameOver;

    void Start()
    {
        isGameOver = false;
    }

    void Update()
    {
        playerHPText.text = "+" + playerHP;
        if (isGameOver)
        {
            //Game over sequence.
        }

    }

    public static void TakeDamage(int damageAmount)
    {
        playerHP -= damageAmount;
        if (playerHP <= 0)
        {
            isGameOver = true;
        }
    }
}
