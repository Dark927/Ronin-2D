using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/GameEvent")]
public class GameEvent : ScriptableObject
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    List<GameEventListener> listeners = new();

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void RegisterListener(GameEventListener listener)
    {
        listeners.Add(listener);
    }

    public void UnRegisterListener(GameEventListener listener)
    {
        listeners.Remove(listener);
    }

    public void Raise()
    {
        for(int i = listeners.Count - 1; i >= 0; --i)
        {
            listeners[i].OnEventRaised();
        }
    }

    #endregion
}
