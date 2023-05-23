using System.Collections;
using UnityEngine;

public class CampfireFlicker : MonoBehaviour
{
    public Light campfireLight;

    private float minIntensity = 2f;
    private float maxIntensity = 2.75f;
    private float flickerSpeed = 0.01f;

    private Color color1 = new Color(1, 0.5f, 0); // Orange
    private Color color2 = new Color(0.5f, 0.1f, 0); // Dark Red

    void Start()
    {
        if (campfireLight == null)
        {
            Debug.LogError("No light component found on this gameobject. Please assign a light source.");
        }
        StartCoroutine(Flicker());
    }

    IEnumerator Flicker()
    {
        while (true)
        {
            float intensity = Random.Range(minIntensity, maxIntensity);
            campfireLight.intensity = intensity;

            // Change color between two values
            campfireLight.color = Color.Lerp(color1, color2, Mathf.PingPong(Time.time * flickerSpeed, 1));

            // This would wait for the end of frame after executing the instructions then loop back again
            yield return new WaitForSeconds(0.1f);
        }
    }
}