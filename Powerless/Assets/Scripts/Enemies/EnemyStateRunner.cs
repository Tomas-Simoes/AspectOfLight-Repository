using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyStateRunner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField]
    private List<EnemyState<T>> _states;
    private readonly Dictionary<Type, EnemyState<T>> _stateByType = new();
    private EnemyState<T> _activeState;

    protected virtual void Awake()
    {
        _states.ForEach(s => _stateByType.Add(s.GetType(), s));
        SetState(_states[0].GetType());
    }

    public void SetState(Type newStateType)
    {
        if (_activeState != null)
        {
            _activeState.Exit();
        }

        _activeState = _states.First(s => s.GetType() == newStateType);
        _activeState.Init(parent: GetComponent<T>());
    }

    private void Update()
    {
        _activeState.Update();
        _activeState.ChangeState();
    }

    private void FixedUpdate()
    {
        _activeState.FixedUpdate();
    }
}
