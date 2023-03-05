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

  public Transform _distatination;

  protected Transform _magicBullet;
  public bool IsGrounded = false;

  public bool IsTravelling = true;

  private bool MovePosition;

  private AbilityManager abilityManager;

  public Player player;

  public override void Start()
  {
    base.Start();
    rb = GetComponent<Rigidbody2D>();

    Debug.Log(player.abilityManager.GetPlayerAbilitySlot("Magic Seed"));

    rb.velocity = (targetPosition - transform.position).normalized * speed;

    IsTravelling = true;
  }

  public override void Update()
  {
    base.Update();

    if (player.inputHandler.pressedAbilityOne && IsTravelling)
    {
      IsTravelling = true;
      IsGrounded = false;
      setVelocity();
    }

    if (player.inputHandler.pressedAbilityOne && IsGrounded)
    {
      IsTravelling = false;
      IsGrounded = true;
      teleport();
      Destroy(this);
    }
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
    player.rb.MovePosition(_magicBullet.position);
    //transform.position = new Vector2(_distatination.position.x, _distatination.position.y);
  }

  void setVelocity()
  {
    speed = 0;
  }
}
