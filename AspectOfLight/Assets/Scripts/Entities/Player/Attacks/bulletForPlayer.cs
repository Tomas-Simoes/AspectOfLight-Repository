using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletForPlayer : MonoBehaviour
{
    public Entity bulletParent;

    [Header("Bullet Stats")]
    [SerializeField]
    public float bulletSpeed = 3f;

    [SerializeField]
    public float lifeTime = 4f;
    public D_EnemyMeleeAttackState stateData;
    private Rigidbody2D rb;

    public void Update()
    {
        transform.Translate(Vector2.right * bulletSpeed * Time.deltaTime);
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
