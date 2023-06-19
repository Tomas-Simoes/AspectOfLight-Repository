using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThePlagueAttackState : State
{
    private ThePlague plague;
    protected D_PlagueAttackState stateData;

    public ThePlagueAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlagueAttackState stateData,
        ThePlague plague
    )
        : base(entity, stateMachine, animBoolName)
    {
        this.plague = plague;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }
}
