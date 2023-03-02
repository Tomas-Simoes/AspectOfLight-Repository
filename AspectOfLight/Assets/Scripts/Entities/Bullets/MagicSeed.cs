using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSeed : Bullet
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

  public Transform _distatination;

  public GameObject _entity;

  private bool IsGrounded = false;

  private bool IsTravelling = false;

  private Player player;

  private bool MovePosition;

  public override void Start()
  {
    base.Start();
    rb = GetComponent<Rigidbody2D>();

    rb.velocity = (targetPosition - transform.position).normalized * speed;

  }

  public override void Update()
  {
    base.Update();
    //if (player.inputHandler.press)
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

  void teleport()
  {
    // if(magicseedIsGrounded)
    //player.rb.MovePosition(bullet.position);
    //transform.position = new Vector2(_distatination.position.x, _distatination.position.y);
  }


}
