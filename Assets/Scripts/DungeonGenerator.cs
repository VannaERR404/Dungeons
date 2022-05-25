using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] GameObject nullPrefab;
    public List<List<GameObject>> grid = new List<List<GameObject>>();
    public int columns, rows;
    public List<GameObject> allRooms = new List<GameObject>();    
    void Start() => columns = columns + 2;   
    public void GenerateButton()
    {
        if(grid.Count == 0 || allRooms.Count == 0)
        {
            Generate();
            return;
        }
        ClearGrid();
    }
    void ClearGrid()
    {
        for (int col = 0; col < columns; col++)
            grid[col].Clear();

        grid.Clear();

        foreach (GameObject room in allRooms)
            Destroy(room);
        allRooms.Clear();
        Generate();
    }
    void Generate()
    {
        bool isEven;
        if (Random.value > 0.5f)
            isEven = true;
        else
            isEven = false;
                    
        for (int col = 0; col < columns; col++)
        {
            grid.Add(new List<GameObject>());
            for (int row = 0; row < rows; row++)
            {

                int rand = Random.Range(0, 100);
                if (col == columns - 2 || col == 1)
                    rand = Random.Range(0, 80);
                if (col == 0 || col == columns-1)
                {
                    if (row == Mathf.FloorToInt(rows/2))
                    {
                        MakeNewRoom(col, row);
                        continue;
                    }
                    grid[col].Add(null);
                    continue;
                }

                if (col == Mathf.FloorToInt(columns/2) || rand < 40 ||( isEven && row % 2 == 0 )||( !isEven && row % 2 != 0 )||( row == Mathf.FloorToInt(rows/2) && col == columns-2 )||( row == Mathf.FloorToInt(rows/2) && col == 1))
                    MakeNewRoom(col, row);
                else
                    grid[col].Add(null);
            }

        }
        foreach (GameObject room in allRooms)
        {
            RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();
            bool[] doorsToOpen = new bool[] { false, false, false, false };
            if (roomBehaviour.gridCords.y+1 != rows)
            {
                if(grid[roomBehaviour.gridCords.x][roomBehaviour.gridCords.y+ 1] != null)
                    doorsToOpen[0] = true;
            }

            if (roomBehaviour.gridCords.y != 0)
            { 
                if(grid[roomBehaviour.gridCords.x][roomBehaviour.gridCords.y - 1] != null)
                    doorsToOpen[1] = true;
            }
            if (roomBehaviour.gridCords.x+1 != columns)
            {
                if(grid[roomBehaviour.gridCords.x + 1][roomBehaviour.gridCords.y] != null)
                    doorsToOpen[2] = true;
            }

            if (roomBehaviour.gridCords.x != 0)
            {
                if(grid[roomBehaviour.gridCords.x - 1][roomBehaviour.gridCords.y] != null)
                    doorsToOpen[3] = true;
            }
            roomBehaviour.UpdateRoom(doorsToOpen);
        }
    }
    void MakeNewRoom(int col, int row)
    {
        GameObject newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], Vector3.zero, Quaternion.identity);
        RoomBehaviour roomBehaviour = newRoom.GetComponent<RoomBehaviour>();
        Vector3 pos = new Vector3(col*24, row*10, 0);
        newRoom.transform.position = pos;
        grid[col].Add(newRoom);
        allRooms.Add(newRoom);
        roomBehaviour.gridCords = new Vector2Int(col,row);
        if (col == 0)
            roomBehaviour.isStart = true;
        if (col == columns-1)
            roomBehaviour.isEnding = true;
    }
}
