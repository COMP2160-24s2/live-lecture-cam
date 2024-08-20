using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    PlayerMove player;
    int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerMove>();
        currentHealth = player.CurrentHealth;
    }

    void Update()
    {
        if (currentHealth != player.CurrentHealth)
        {
            currentHealth = player.CurrentHealth;
            Debug.Log("Player's health is now: " + currentHealth);
        }
    }
}
