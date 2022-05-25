using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    [SerializeField] GameObject[] doors;
    public Vector2Int gridCords;
    public bool isStart;
    public bool isEnding;

    public void UpdateRoom(bool[] status)
    {
        for (int i = 0; i < doors.Length; i++)
            doors[i].SetActive(!status[i]);
        if (isStart)
            GameObject.Find("Player").transform.position = transform.position;
    }

}
