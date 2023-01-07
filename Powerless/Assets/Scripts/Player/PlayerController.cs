using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : StateRunner<PlayerController>
{
    public PlayerAnimation playerAnimation;

    protected override void Awake()
    {
        base.Awake();
    }

}
