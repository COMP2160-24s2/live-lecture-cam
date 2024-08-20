using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float range = 10f;
    [SerializeField] private int damage = 4;
    private Vector3 startPosition;

    void Awake()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime, Space.Self);

        Vector3 distanceFromStart = transform.position - startPosition;
        float distance = distanceFromStart.magnitude;
        if (distance > range)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            PlayerMove player = other.gameObject.GetComponent<PlayerMove>();
            if (player != null)
            {
                player.CurrentHealth = (player.CurrentHealth - damage);
            }
        }
        Destroy(gameObject);
        Debug.Log("Collided with: " + other.gameObject);
    }
}
