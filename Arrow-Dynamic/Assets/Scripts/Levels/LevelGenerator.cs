using UnityEngine;

/// <summary>
/// Generates the level layout and connects rooms together.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    public Grid grid;
    public int startingX;
    public int startingY;

    void Start()
    {
        // Generate the level layout and connect the rooms
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        // Implement level generation logic here, e.g., random walk, procedural generation, etc.

        // Example: set the starting room as active
        Room startingRoom = grid.GetRoom(startingX, startingY);
        if (startingRoom != null)
        {
            startingRoom.SetActive(true);
        }
    }
}