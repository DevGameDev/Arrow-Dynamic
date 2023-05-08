using UnityEngine;

/// <summary>
/// Manages player statistics
/// </summary>
public class PlayerStatistics : MonoBehaviour
{
    public PlayerStatistics Instance { get; set; }

    private GameSettings settings;
    private GameState state;

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

    private void Start()
    {
        settings = GameManager.GetSettings();
        state = GameManager.GetState();
    }
}