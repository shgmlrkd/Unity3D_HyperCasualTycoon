using System;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoSingleton<EventManager>
{
    public enum EventType
    {
        OnGoldChanged,
        OnReputationChanged,
        OnGameStateChanged,
        OnVisitorCountChanged,
        OnFestivalTriggered
    }

    private readonly Dictionary<EventType, Action<object>> _eventDictionary = new Dictionary<EventType, Action<object>>();

    public void Subscribe(EventType eventType, Action<object> listener)
    {
        if (!_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] = null;
        }
        _eventDictionary[eventType] += listener;
    }

    public void Unsubscribe(EventType eventType, Action<object> listener)
    {
        if (_eventDictionary.ContainsKey(eventType))
        {
            _eventDictionary[eventType] -= listener;
        }
    }

    public void Publish(EventType eventType, object param = null)
    {
        if (_eventDictionary.TryGetValue(eventType, out var thisEvent))
        {
            thisEvent?.Invoke(param);
        }
    }
}