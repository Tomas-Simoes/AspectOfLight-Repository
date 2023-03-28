using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornborneExplosion : MonoBehaviour
{
    //TODO when the animation is done update the '_animationTime' to use the GetCurrentAnimatorStateInfo


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

    void Start() { }

    void Update()
    {
        _animationTime += Time.deltaTime;
    }

    void Die()
    {
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasAlreadyHitted)
        {
            Debug.Log("Hitted Player");

            Entity player = other.GetComponent<Entity>();

            player.TakeDamage(explosionDamage);
            player.TakePoisonDamage(poisonDamage, poisonTime, tickRate);

            hasAlreadyHitted = true;
        }
    }
}
