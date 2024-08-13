using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Break this down into Movement and Combat scripts.

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float rotationRate = 1f;
    private float rotationModifier = 0;
    [SerializeField] private Transform target;
    
    [SerializeField] private LayerMask rayLayers;
    [SerializeField] private Fireball fireball;
    [SerializeField] private float attackInterval = 5f;
    private float timer = 0;

    [SerializeField] private float sleepInterval = 1f;
    [SerializeField] private float searchInterval = 1f;
    private float stateTimer = 0f;

    private bool seesPlayer = false;

    enum State
    {
        Searching,
        Sleeping,
        Attacking
    };

    [SerializeField] private State state;

    void Awake()
    {
        timer = attackInterval;
        state = State.Searching;
        stateTimer = searchInterval;
    }

    void Update()
    {
        transform.Rotate(
            Vector3.up * rotationRate * rotationModifier * Time.deltaTime, Space.Self);

        switch (state)
        {
            case State.Searching:
                rotationModifier = 1f; // Keeps monster rotating in a consistent direction
                stateTimer -= Time.deltaTime;

                if (seesPlayer)
                {
                    state = State.Attacking;
                    Debug.Log("Switching state to Attacking");
                }

                if (stateTimer <= 0)
                {
                    state = State.Sleeping;
                    stateTimer = sleepInterval;
                }
                break;

            case State.Sleeping:
                rotationModifier = 0f; // Stops monster movement
                stateTimer -= Time.deltaTime;
                
                if (stateTimer <= 0)
                {
                    state = State.Searching;
                    stateTimer = searchInterval;
                }
                break;

            case State.Attacking:
                DoAttack();
                if (!seesPlayer)
                {
                    state = State.Searching;
                    Debug.Log("Switching state to Searching");
                }
                break;
        }
    }

    void Shoot()
    {
        Fireball newFireball = Instantiate(fireball, transform.position, transform.rotation);
        timer = attackInterval;
    }

    void DoAttack()
    {
        Vector3 myRight = transform.TransformDirection(Vector3.right);
        Vector3 toTarget = Vector3.Normalize(target.position - transform.position);
        float targetRelative = Vector3.Dot(myRight, toTarget);
        rotationModifier = targetRelative; // Keeps monster locked on player

        // WARNING: ALL PHYSICS SHOULD GO IN FIXED UPDATE
        RaycastHit hit;
        if (Physics.Raycast(transform.position, toTarget, out hit, Mathf.Infinity, rayLayers))
        {
            if (hit.collider.gameObject.layer == 6 && timer <= 0)
            {
                Debug.Log("Shooting");
                Shoot();
            }
        }
        timer -= Time.deltaTime;
    }

    void OnTriggerEnter()
    {
        seesPlayer = true;
    }

    void OnTriggerExit()
    {
        seesPlayer = false;
    }
}