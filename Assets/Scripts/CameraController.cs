using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour 
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters 

    [SerializeField] Transform playerTransform;

    [SerializeField] float clampLeft;
    [SerializeField] float clampRight;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods


    private void LateUpdate()
    {
        bool followPlayer = (playerTransform.position.x < clampRight) && (playerTransform.position.x > clampLeft);

        if (followPlayer)
        {
            transform.position = new Vector3(playerTransform.position.x, transform.position.y, transform.position.z);
        }
    }

    #endregion
}
