using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlagueAttackState : State
{
    private Plague plague;
    protected D_PlagueAttackState stateData;

    public PlagueAttackState(
        Entity entity,
        StateMachine stateMachine,
        string animBoolName,
        D_PlagueAttackState stateData,
        Plague plague
    ) : base(entity, stateMachine, animBoolName)
    {
        this.plague = plague;
        this.stateData = stateData;
    }

    public override void Enter()
    {
        base.Enter();
    }
}
