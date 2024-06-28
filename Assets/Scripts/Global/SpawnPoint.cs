using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void SpawnEnemy(GameObject enemy)
    {
        enemy.transform.position = transform.position;
        enemy.SetActive(true);
    }

    #endregion
}
