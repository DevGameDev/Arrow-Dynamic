using UnityEngine;

/// <summary>
/// Abstract base class for unique arrow effects.
/// </summary>
public abstract class ArrowEffect : MonoBehaviour
{
    /// <summary>
    /// Apply the effect when the arrow collides with an object.
    /// </summary>
    /// <param name="collision">Collision data from the arrow.</param>
    public abstract void ApplyEffect(Collision collision);
}