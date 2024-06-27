using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] Material mainFlashMaterial;
    [SerializeField] Material secondaryFlashMaterial;
    [SerializeField] float flashDuration = 0.5f;

    SpriteRenderer spriteRenderer;
    Material originalMaterial;
    Coroutine activeFlashRoutine;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private IEnumerator FlashRoutine(Material flashMaterial)
    {
        spriteRenderer.material = flashMaterial;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.material = originalMaterial;

        activeFlashRoutine = null;
    }

    #endregion


    // --------------------------------------------------------------------------
    // Public Methods
    // --------------------------------------------------------------------------

    #region Public Methods

    public void Flash(bool useSecondaryMaterial = false)
    {
        // Check if flash effect activated 

        if (activeFlashRoutine != null)
        {
            StopCoroutine(activeFlashRoutine);
        }

        // Activate flash effect

        if (!useSecondaryMaterial)
        {
            activeFlashRoutine = StartCoroutine(FlashRoutine(mainFlashMaterial));
        }
        else
        {
            activeFlashRoutine = StartCoroutine(FlashRoutine(secondaryFlashMaterial));
        }
    }

    #endregion
}
