using UnityEngine;

/// <summary>
/// Represents a grid of rooms and handles grid-related operations.
/// </summary>
public class Grid : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public GameObject roomPrefab;
    private Room[,] grid;

    // Initialize the grid with empty rooms
    void Start()
    {
        grid = new Room[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Instantiate a room prefab and assign its position based on the grid coordinates
                GameObject roomObject = Instantiate(roomPrefab, new Vector3(x * 10, 0, y * 10), Quaternion.identity);
                Room room = roomObject.GetComponent<Room>();
                grid[x, y] = room;
            }
        }
    }

    /// <summary>
    /// Returns the room at the given grid coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the room.</param>
    /// <param name="y">The y-coordinate of the room.</param>
    /// <returns>The room at the given coordinates, or null if the coordinates are invalid.</returns>
    public Room GetRoom(int x, int y)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return grid[x, y];
        }
        return null;
    }
    /// <summary>
    /// Sets the room at the given grid coordinates.
    /// </summary>
    /// <param name="x">The x-coordinate of the room.</param>
    /// <param name="y">The y-coordinate of the room.</param>
    /// <param name="room">The room to set at the given coordinates.</param>
    public void SetRoom(int x, int y, Room room)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            grid[x, y] = room;
        }
    }

    /// <summary>
    /// Returns the grid coordinates of the room containing the given position.
    /// </summary>
    /// <param name="position">The position to check.</param>
    /// <returns>The grid coordinates of the room containing the position, or (-1, -1) if the position is not inside any room.</returns>
    public Vector2Int GetRoomCoordinates(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x / 10);
        int y = Mathf.FloorToInt(position.z / 10);

        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight)
        {
            return new Vector2Int(x, y);
        }
        return new Vector2Int(-1, -1);
    }
}