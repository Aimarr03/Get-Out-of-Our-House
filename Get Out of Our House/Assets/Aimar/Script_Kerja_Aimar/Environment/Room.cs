using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public enum RoomType
    {
        Livingroom,
        Dinner,
        Kitchen,
        ChildBedroom,
        ParentsBedroom,
        Basement,
        Attic
    }
    [SerializeField] private Environment_Door[] doors;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer floor;
    [SerializeField] private float boundMultiplier;
    public RoomType type;
    private void Start()
    {
        GetBackgroundHorizontalBound(out float minHorizontal, out float maxHorizontal);
    }
    public void GetBackgroundHorizontalBound(out float minHorizontal, out float maxHorizontal)
    {
        Bounds bounds = floor.bounds;
        minHorizontal = bounds.min.x;
        maxHorizontal = bounds.max.x;
        maxHorizontal *= boundMultiplier;
        minHorizontal *= boundMultiplier;
        Debug.Log($"min: {minHorizontal} max: {maxHorizontal}");
    }
    public Environment_Door GetRandomDoors()
    {
        int maxBound = doors.Length;
        return doors[Random.Range(0, maxBound)];
    }
}
