using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
  protected StateMachine stateMachine;
  protected Entity entity;
  protected string animBoolName;
  protected float startTime; // i don't know if this will be useful, but it will be at what time entered this state

  public State(Entity entity, StateMachine stateMachine, string animBoolName)
  {
    this.stateMachine = stateMachine;
    this.entity = entity;
    this.animBoolName = animBoolName;
  }

  public virtual void Enter()
  {
    startTime = Time.time;
    entity.anim.SetBool(animBoolName, true);
  }

  public virtual void Exit()
  {
    entity.anim.SetBool(animBoolName, false);
  }

  public virtual void LogicUpdate() { }

  public virtual void PhysicsUpdate() { }

}
