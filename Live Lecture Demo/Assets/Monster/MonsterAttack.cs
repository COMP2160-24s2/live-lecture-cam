using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MonsterBehaviour))]    
public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private LayerMask rayLayers;
    [SerializeField] private Projectile projectile;
    [SerializeField] private float attackInterval = 5f;
    private float timer = 0;

    internal void DoAttack(Vector3 toTarget)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, toTarget, out hit, Mathf.Infinity, rayLayers))
        {
            if (hit.collider.gameObject.layer == 6 && timer <= 0)
            {
                Shoot();
            }
        }
        timer -= Time.deltaTime;
    }

    void Shoot()
    {
        Projectile newProjectile = Instantiate(projectile, transform.position, transform.rotation);
        timer = attackInterval;
    }
}
