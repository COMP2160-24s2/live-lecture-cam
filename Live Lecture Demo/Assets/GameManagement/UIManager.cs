using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    static private UIManager instance;
    static public UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                Debug.LogError("There is no UIManager instance in this scene.");
            }
                return instance;
        }
    }

    private int currentHealth;
    private int maxHealth;
    private int totalDeaths;

    [SerializeField] private TextMeshProUGUI healthDisplay;
    [SerializeField] private TextMeshProUGUI deathDisplay;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("There is more than one UI Manager in the scene. Destroying additional");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        currentHealth = GameManager.Instance.CurrentHealth;
        totalDeaths = GameManager.Instance.TotalDeaths;
        healthDisplay.text = string.Format("Health:<br>{0}",currentHealth);
        deathDisplay.text = string.Format("Total Deaths:<br>{0}",totalDeaths);
        Debug.Log(currentHealth);
        Debug.Log(totalDeaths);
    }
}