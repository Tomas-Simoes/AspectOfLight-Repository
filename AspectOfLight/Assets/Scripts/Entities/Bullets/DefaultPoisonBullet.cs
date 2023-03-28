using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultPoisonBullet : Bullet
{
    [Header("Posion Bullet Information")]
    [SerializeField]
    private float tickRate;

    [SerializeField]
    private float poisonTime;

    public override void  Start() {
        base.Start();
        rb.velocity = (targetPosition - transform.position).normalized * speed;

        Destroy(this.gameObject, lifeTime);

    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(targatableTag)){
            other.gameObject.GetComponent<Entity>().TakePoisonDamage(damage, poisonTime, tickRate);
            Destroy(this.gameObject);
        }

        if(other.CompareTag("Wall"))
            Destroy(this.gameObject);
    }
}
