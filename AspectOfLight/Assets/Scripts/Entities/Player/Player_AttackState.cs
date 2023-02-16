using UnityEngine;
using UnityEngine.InputSystem;

public class Player_AttackState : State
{
    public Player player;
    public D_PlayerAttackState stateData;

    [SerializeField]
    private float attackTime;

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

        if(attackTime >= stateData.maxAttackTime){
            // can't charge more
            Debug.Log("Can't charge more");
            stateMachine.ChangeState(player.moveState);
        } else {
            // still charging
            attackTime += Time.deltaTime;
            Debug.Log("still charging");
        }

        if(!player.inputHandler.pressingAttack){          
            GameObject _bullet = entity.CreatePrefab(
            player.bulletPrefab,
            player.attackPosition.transform.position,
            player.bulletsParent
        );

        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = 10;

        _bullet.GetComponent<Bullet>().targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        stateMachine.ChangeState(player.moveState);
        }
        
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
