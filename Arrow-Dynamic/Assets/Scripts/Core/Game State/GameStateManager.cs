using System.IO;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    private string gameStateFilePath;
    public GameStateData gameStateData;

    void Awake()
    {
        // Make this a singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        gameStateFilePath = Application.persistentDataPath + "/gameState.json";
    }

    void Start()
    {
        LoadGameState();
    }

    public void SaveGameState()
    {
        string json = JsonUtility.ToJson(gameStateData);
        File.WriteAllText(gameStateFilePath, json);
    }

    public void LoadGameState()
    {
        if (File.Exists(gameStateFilePath))
        {
            string json = File.ReadAllText(gameStateFilePath);
            gameStateData = JsonUtility.FromJson<GameStateData>(json);
        }
        else
        {
            gameStateData = new GameStateData();
            SaveGameState();
        }
    }
}