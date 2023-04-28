using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : Singleton<EventManager>
{
    private Dictionary<string, Action<Dictionary<string, object>>> eventDictionary;

    public override void Init()
    {
        base.Init();

        Instance.eventDictionary = new Dictionary<string, Action<Dictionary<string, object>>>();
    }

    public static void StartListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (Instance.eventDictionary == null)
            Instance.Init();

        Action<Dictionary<string, object>> thisEvent;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent += listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
        else
        {
            thisEvent += listener;
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, Action<Dictionary<string, object>> listener)
    {
        if (Instance == null)
            return;

        Action<Dictionary<string, object>> thisEvent;

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent -= listener;
            Instance.eventDictionary[eventName] = thisEvent;
        }
    }

    public static void TriggerEvent(
        string eventName,
        bool isUIEvent,
        Dictionary<string, object> message
    )
    {
        Action<Dictionary<string, object>> thisEvent = null;

        if (isUIEvent)
            UIManager.Instance.Init();

        if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
        {
            thisEvent.Invoke(message);
        }
    }
}
