using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlashing : MonoBehaviour
{
    public float flashSpeed = 1f;
    public Color startColor = Color.white;
    public Color endColor = Color.red;

    private TextMeshProUGUI textMesh;
    private bool isFlashing = false;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        // Start flashing when the object is enabled
        StartFlashing();
    }

    private void OnDisable()
    {
        // Stop flashing when the object is disabled
        StopFlashing();
    }

    public void StartFlashing()
    {
        if (!isFlashing)
        {
            isFlashing = true;
            StartCoroutine(FlashText());
        }
    }

    public void StopFlashing()
    {
        if (isFlashing)
        {
            isFlashing = false;
            StopAllCoroutines();
            ResetText();
        }
    }

    private IEnumerator FlashText()
    {
        while (isFlashing)
        {
            // Flash from start color to end color
            yield return LerpColor(startColor, endColor, flashSpeed);

            // Flash from end color to start color
            yield return LerpColor(endColor, startColor, flashSpeed);
        }
    }

    private IEnumerator LerpColor(Color fromColor, Color toColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Color lerpedColor = Color.Lerp(fromColor, toColor, t);
            textMesh.color = lerpedColor;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMesh.color = toColor;
    }

    private void ResetText()
    {
        textMesh.color = startColor;
    }
}