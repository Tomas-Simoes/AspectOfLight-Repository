using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBullet : Bullet
{
  public override void Start()
  {
    base.Start();
    rb.velocity = (targetPosition - transform.position).normalized * speed;

    Destroy(this.gameObject, lifeTime);
  }

  public override void Update()
  {
    base.Update();
  }

  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.CompareTag(targatableTag))
    {
      other.GetComponent<Entity>().TakeDamage(damage);
      Destroy(this.gameObject);
    }

    if (other.CompareTag("Wall"))
    {
      Destroy(this.gameObject);
    }
  }

}
