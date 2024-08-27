using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public delegate void TakeDamage();
    public event TakeDamage TakeDamageEvent;

    [SerializeField] private float moveSpeed = 1f;
    private Vector3 moveDirection = new Vector3(0f,0f,0f);

    private Rigidbody rb;

    private PlayerInput actions;
    private InputAction movementAction;

    [SerializeField] private int maxHealth = 10;
    private int currentHealth;
    public int CurrentHealth
    {
        get 
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;
        }
    }

    void Awake()
    {
        actions = new PlayerInput();
        movementAction = actions.GameWorld.Movement;
        currentHealth = maxHealth;
    }
    void OnEnable()
    {
        movementAction.Enable();
    }

    void OnDisable()
    {
        movementAction.Disable();
    }

    void Update()
    {
        moveDirection.x = movementAction.ReadValue<Vector2>().x;
        moveDirection.z = movementAction.ReadValue<Vector2>().y;
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.Self);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            Projectile thisProjectile = other.gameObject.GetComponent<Projectile>();
            if (thisProjectile != null)
            {
                int damageAmount = thisProjectile.Damage;
                GetHit(damageAmount);
            }
            else
            {
                Debug.LogError("Projectile object has no Projectile script attached.");
            }
        }
    }

    void GetHit(int damage)
    {
        currentHealth -= damage;
        TakeDamageEvent?.Invoke();
    }
}
