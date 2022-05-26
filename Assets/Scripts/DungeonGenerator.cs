using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [SerializeField] GameObject[] roomPrefabs;
    [SerializeField] GameObject tunnelPrefab;
    public int offset;
    public List<List<GameObject>> grid = new List<List<GameObject>>();
    public int columns, rows;
    public List<GameObject> allRooms = new List<GameObject>();    
    public List<GameObject> allTunnels = new List<GameObject>();  
    void Start() => columns = columns + 2;   
    private GameObject newRoom;
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

        foreach (GameObject tunnel in allTunnels)
            Destroy(tunnel);
        allTunnels.Clear();
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
                GameObject roomToCheck = grid[roomBehaviour.gridCords.x][roomBehaviour.gridCords.y + 1];
                if(roomToCheck != null)
                {
                    doorsToOpen[0] = true;
                    MakeDoors(room, roomToCheck, doorsToOpen);
                    MakeNewTunnel(room, roomToCheck);
                }
            }

            if (roomBehaviour.gridCords.y != 0)
            { 
                GameObject roomToCheck = grid[roomBehaviour.gridCords.x][roomBehaviour.gridCords.y - 1];
                if(roomToCheck != null)
                {
                    doorsToOpen[1] = true;
                    MakeDoors(room, roomToCheck, doorsToOpen);
                    MakeNewTunnel(room, roomToCheck);
                }
            }
            if (roomBehaviour.gridCords.x+1 != columns)
            {
                GameObject roomToCheck = grid[roomBehaviour.gridCords.x + 1][roomBehaviour.gridCords.y];
                if(roomToCheck != null)
                {
                    doorsToOpen[2] = true;   
                    MakeDoors(room, roomToCheck, doorsToOpen);
                    MakeNewTunnel(room, roomToCheck);
                }
            }

            if (roomBehaviour.gridCords.x != 0)
            {
                GameObject roomToCheck = grid[roomBehaviour.gridCords.x - 1][roomBehaviour.gridCords.y];
                if(roomToCheck != null)
                {
                    doorsToOpen[3] = true;
                    MakeDoors(room, roomToCheck, doorsToOpen);
                    MakeNewTunnel(room, roomToCheck);
                }
            }
        }
    }
    void MakeDoors(GameObject room, GameObject roomToCheck, bool[] doorsToOpen)
    {
        RoomBehaviour roomBehaviour = room.GetComponent<RoomBehaviour>();
        roomBehaviour.UpdateRoom(doorsToOpen);
    }
    void MakeNewRoom(int col, int row)
    {
        newRoom = Instantiate(roomPrefabs[Random.Range(0, roomPrefabs.Length)], Vector3.zero, Quaternion.identity);
        RoomBehaviour roomBehaviour = newRoom.GetComponent<RoomBehaviour>();
        Vector3 pos = new Vector3((col+offset)*(roomBehaviour.roomSize.x+offset), (row+offset)*(roomBehaviour.roomSize.y+offset), 0);
        newRoom.transform.position = pos;
        grid[col].Add(newRoom);
        allRooms.Add(newRoom);
        roomBehaviour.gridCords = new Vector2Int(col,row);
        if (col == 0)
            roomBehaviour.isStart = true;
        if (col == columns-1)
            roomBehaviour.isEnding = true;
    }
    void MakeNewTunnel(GameObject room1, GameObject room2)
    {
        bool isHorizontal;
        if (Mathf.RoundToInt(room1.transform.position.x) == Mathf.RoundToInt(room2.transform.position.x))
            isHorizontal = false;
        else
            isHorizontal = true;
        
        if (isHorizontal)
        {
            GameObject newTunnel = Instantiate(tunnelPrefab, Vector3.zero, Quaternion.Euler(0, 0, 90));
            MoveTunnel(newTunnel, room1.transform.position, room2.transform.position, true);
        }

        else
        {
            GameObject newTunnel = Instantiate(tunnelPrefab, Vector3.zero, Quaternion.identity);
            MoveTunnel(newTunnel, room1.transform.position, room2.transform.position, false);
        }
        


    }
    void MoveTunnel(GameObject tunnel, Vector3 room1, Vector3 room2, bool isHorizontal)
    {
        if(isHorizontal)
        {
            tunnel.transform.position = new Vector3((room1.x + room2.x)/2, room1.y, 0);
        }
        else
        {
            tunnel.transform.position = new Vector3(room1.x, (room1.y + room2.y) / 2, 0);
        }
        allTunnels.Add(tunnel);
    }
}
