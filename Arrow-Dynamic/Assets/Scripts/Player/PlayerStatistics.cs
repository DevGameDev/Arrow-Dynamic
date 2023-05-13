using UnityEngine;

/// <summary>
/// Manages player statistics
/// </summary>
public class PlayerStatistics : MonoBehaviour
{
    public PlayerStatistics Instance { get; set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            GameManager.Instance.HandleGameQuit(false, "Duplicate PlayerStats's");
        }
    }
}