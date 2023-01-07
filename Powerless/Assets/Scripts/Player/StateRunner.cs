using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public abstract class StateRunner<T> : MonoBehaviour where T : MonoBehaviour
{

    [SerializeField]
    private List<State<T>> _states;
    private State<T> _activeState;

    protected virtual void Awake()
    {
        SetState(_states[0].GetType());
    }

    public void SetState(Type newStateType)
    {
        if (_activeState != null)
        {
            _activeState.Exit();
        }

        _activeState = _states.First(s => s.GetType() == newStateType);
        _activeState.Init(parent:GetComponent<T>());
    }

    private void Update()
    {
        _activeState.GetInput();
        _activeState.Update();
        _activeState.ChangeState();
    }

    private void FixedUpdate()
    {
        _activeState.FixedUpdate();
    }
}
