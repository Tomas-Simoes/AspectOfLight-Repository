using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSeedBullet : Bullet
{
    /*
      travelState
      groundedState
  
      setVelocity();
  
      abilityManager.playerAbilities.FindSlotMagicSeed  *1, 2, 3*
  
    
      if(player.input.pressedAbilityKey && travelState){
        travelState = false;
        groundedState = true;
      }
  
      if(player.input.pressedAbilityKey && grounedState){
        TeleportPlayer();
        Destroy(this);
      }
    */


    private bool isGrounded = false;

    private bool isTravelling = false;

    private AbilityManager abilityManager;

    public Player player;

    public override void Start()
    {
        base.Start();
        rb.velocity = (targetPosition - transform.position).normalized * speed;

        isTravelling = true;
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
