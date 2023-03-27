// ----------------------------------------------------------------------------
// Unite 2017 - Game Architecture with Scriptable Objects
// 
// Author: Ryan Hipple
// Date:   10/04/17
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/GameEvent")]
public class GameEvent : ScriptableObject
{
    private List<GameEventListener> listeners = new List<GameEventListener>();

    public void Raise() 
    {
        for (int i = listeners.Count - 1; i >= 0; i--)
        {
            listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listenerToAdd) 
    {
        if (!listeners.Contains(listenerToAdd)) 
        {
            listeners.Add(listenerToAdd);
        }
    }

    public void UnregisterListener(GameEventListener listenerToRemove) 
    {
        if (listeners.Contains(listenerToRemove)) 
        {
            listeners.Remove(listenerToRemove);
        }
    }
}
