using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTime
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    int _seconds;
    int _minutes;

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public CustomTime(int seconds = 0, int minutes = 0)
    {
        _seconds = seconds;
        _minutes = minutes;
    }

    public int Seconds
    {
        get { return _seconds; }
    }

    public int Minutes
    {
        get { return _minutes; }
    }


    public void UpdateTime(int actualTimeSec)
    {
        _seconds = actualTimeSec;

        if (Seconds >= 60)
        {
            _minutes += Seconds / 60;
            _seconds %= 60;
        }

        _minutes = (int)(actualTimeSec / 60);
    }

    #endregion
}
