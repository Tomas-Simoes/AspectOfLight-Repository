using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public abstract class StateRunner<T> : MonoBehaviour where T : MonoBehaviour
{

    [SerializeField]
    private List<State<T>> _States;
    private readonly Dictionary<Type, State<T>> _StateByType = new();
    private State<T> _activeState;

    protected virtual void awake()
    {

    }



}
