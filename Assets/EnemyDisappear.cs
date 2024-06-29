using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisappear : MonoBehaviour
{
    // --------------------------------------------------------------------------
    // Parameters
    // --------------------------------------------------------------------------

    #region Parameters

    [SerializeField] float timeDelay = 2f;
    [SerializeField] float timeToDissapear = 4f;

    Enemy currentEnemy;
    SpriteRenderer spriteRenderer;
    Material originalMaterial;

    Coroutine activeDisappear = null;

    #endregion


    // --------------------------------------------------------------------------
    // Private Methods
    // --------------------------------------------------------------------------

    #region Private Methods

    private void Awake()
    {
        currentEnemy = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    private void Update()
    {
        ConfigureDisappear();
    }

    private void ConfigureDisappear()
    {
        bool needDisappear = (currentEnemy.IsDead) && (activeDisappear == null);

        if (needDisappear)
        {
            activeDisappear = StartCoroutine(DisappearRoutine());
        }
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(timeDelay);

        float elapsedTime = 0f;
        Color materialColor = originalMaterial.color;

        // Linearly changing alpha to complete disappear

        while(elapsedTime < timeToDissapear)
        {
            elapsedTime += Time.deltaTime;

            float newAlpha = Mathf.Lerp(1, 0, elapsedTime / timeToDissapear);

            materialColor.a = newAlpha;
            originalMaterial.color = materialColor;

            yield return null;
        }

        materialColor.a = 0;
        originalMaterial.color = materialColor;
        
        // Deactivate enemy and reset color

        gameObject.SetActive(false);
        materialColor.a = 1;
        originalMaterial.color = materialColor;

        activeDisappear = null;
    }

    #endregion

}
