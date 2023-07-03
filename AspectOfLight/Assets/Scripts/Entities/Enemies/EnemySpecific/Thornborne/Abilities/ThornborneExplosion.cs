using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornborneExplosion : MonoBehaviour
{
    [Header("Explosion Information")]
    private bool hasAlreadyHitted = false;

    [SerializeField]
    private float explosionDamage;

    [SerializeField]
    private float poisonDamage;

    [SerializeField]
    private float poisonTime;

    [SerializeField]
    private float tickRate;

    private float _animationTime = 0;

    void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAlreadyHitted)
        {
            Entity player = other.GetComponent<Entity>();

            player.TakeDamage(explosionDamage);
            player.TakePoisonDamage(poisonDamage, poisonTime, tickRate);

            hasAlreadyHitted = true;
        }
    }
}
