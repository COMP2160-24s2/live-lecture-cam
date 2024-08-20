using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterAttack))]
public class MonsterBehaviour : MonoBehaviour
{
    private MonsterAttack monsterAttack;

    [SerializeField] private float rotationRate = 1f;
    private float rotationModifier = 0;

    [SerializeField] private float sleepInterval = 1f;
    [SerializeField] private float searchInterval = 1f;
    private float stateTimer = 0f;

    private PlayerMove player;
    private bool seesPlayer = false;

    enum State
    {
        Searching,
        Sleeping,
        Attacking
    };

    private State state;

    void Awake()
    {
        state = State.Searching;
        stateTimer = searchInterval;
        monsterAttack = GetComponent<MonsterAttack>();
    }

    void Start()
    {
       player = FindObjectOfType<PlayerMove>(); // Looks for a component
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationRate * rotationModifier * Time.deltaTime, Space.Self);

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
                Vector3 myRight = transform.TransformDirection(Vector3.right);
                Vector3 toTarget = Vector3.Normalize(player.transform.position - transform.position);
                float targetRelative = Vector3.Dot(myRight, toTarget);

                monsterAttack.DoAttack(toTarget);
                rotationModifier = targetRelative; // Keeps monster locked on player
                if (!seesPlayer)
                {
                    state = State.Searching;
                    Debug.Log("Switching state to Searching");
                }
                break;
        }
    }

// No need to check for layer of object, Monster layer only collides with Player layer as per matrix.
    void OnTriggerEnter()
    {
        seesPlayer = true;
    }

    void OnTriggerExit()
    {
        seesPlayer = false;
    }
}