using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static private GameManager instance;
    static public GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("There is no GameManager instance in this scene.");
            }
            return instance;
        }
    }

    private PlayerMove player;
    private int currentHealth;
    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }
    }

    private int totalDeaths = 0;
    public int TotalDeaths
    {
        get
        {
            return totalDeaths;
        }
    }

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one GameManager in the scene. Destroying additional");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }
    }

    void PlayerDamaged()
    {
        currentHealth = player.CurrentHealth;
        Debug.Log("Player's health is now: " + currentHealth);
        if (currentHealth <= 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        totalDeaths += 1;
        Scene scene = SceneManager.GetActiveScene();
        Debug.Log("Total deaths is: " + totalDeaths);
        SceneManager.LoadScene(scene.buildIndex, LoadSceneMode.Single);
    }

    void FindPlayer()
    {
        Debug.Log("Player object is missing, searching now");
        player = FindObjectOfType<PlayerMove>();
        player.TakeDamageEvent += PlayerDamaged;
        currentHealth = player.CurrentHealth;
    }
}
