using UnityEngine;
using UnityEngine.InputSystem;

public class Player_AttackState : State
{
  public Player player;
  public D_PlayerAttackState stateData;

  public Bullet _bullet;

  [SerializeField]
  private float attackTime;

  private float normalDamage;

  public Player_AttackState(
      Entity entity,
      StateMachine stateMachine,
      string animBoolName,
      D_PlayerAttackState stateData,
      Player player,
      D_PlayerCoolDown coolDownData
  )
      : base(entity, stateMachine, animBoolName)
  {
    this.player = player;
    this.stateData = stateData;
  }

  public override void Enter()
  {
    base.Enter();
    attackTime = 0;
    player.timeSinceAttack = 0;

    entity.rb.velocity = Vector2.zero;
  }

  public override void Exit()
  {
    base.Exit();
  }

  public override void LogicUpdate()
  {
    base.LogicUpdate();

    if (attackTime >= stateData.maxAttackTime)
    {
      // can't charge more

      Debug.Log("Can't charge more");
    }
    else
    {
      // still charging
      attackTime += Time.deltaTime;
      //Debug.Log("still charging");
    }


    if (!player.inputHandler.pressingAttack)
    {
      GameObject _bullet = entity.CreatePrefab(
      player.bulletPrefab,
      player.attackPosition.transform.position,
      player.bulletsParent
  );
      Vector3 mousePosition = Mouse.current.position.ReadValue();
      mousePosition.z = 10;

      Bullet _bulletComponent = _bullet.GetComponent<Bullet>();

      _bulletComponent.targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);


      if (attackTime >= 1)
      {

        _bulletComponent.damage = 1;
      }
      if (attackTime >= 3)
      {
        _bulletComponent.damage = 3;
      }
      if (attackTime >= 3)
      {
        _bulletComponent.damage = 5;
      }
      // Debug.Log(attackTime);
      Debug.Log(_bulletComponent.damage);
      stateMachine.ChangeState(player.moveState);
    }

  }

  public override void PhysicsUpdate()
  {
    base.PhysicsUpdate();
  }

}
