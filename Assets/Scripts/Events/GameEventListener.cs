using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] GameEvent eventData;
    [SerializeField] UnityEvent response;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods


    private void OnEnable()
    {
        eventData.RegisterListener(this);
    }

    private void OnDisable()
    {
        eventData.UnRegisterListener(this);
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void OnEventRaised()
    {
        response.Invoke();
    }

    #endregion
}
