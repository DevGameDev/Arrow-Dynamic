using System.Collections;
using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreChangeText : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float fadeSpeed = 1f;

    private TextMeshProUGUI penaltyText;

    private void Awake()
    {
        penaltyText = GetComponent<TextMeshProUGUI>();
    }

    public IEnumerator ScoreChangeTextEffect(float scoreChange)
    {
        if (scoreChange != 0)
        {
            Vector3 initialPosition = transform.position;

            Color initialColor;
            if (scoreChange < 0)
            {
                initialColor = Color.red;
                penaltyText.text = "- " + scoreChange.ToString();
            }
            else
            {
                initialColor = Color.green;
                penaltyText.text = "+ " + scoreChange.ToString();
            }
            penaltyText.color = initialColor;

            float elapsedTime = 0f;
            while (elapsedTime < fadeSpeed)
            {
                // Fade the text color to transparent
                float progress = elapsedTime / fadeSpeed;
                Color newColor = Color.Lerp(initialColor, Color.clear, progress);
                transform.position = Vector3.Lerp(initialPosition, initialPosition + (Vector3.up * (elapsedTime * moveSpeed)), progress);
                penaltyText.color = newColor;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }

        // Destroy the text object
        Destroy(gameObject);
    }
}