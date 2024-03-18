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
        Attic,
        SecondFloorHall
    }
    [SerializeField] private Environment_Door[] doors;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer floor;
    [SerializeField] private float boundMultiplier;
    [SerializeField] private ParticleSystem dustParticleSystem;
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
        minHorizontal += 1.5f;
        maxHorizontal -= 1.5f;
        Debug.Log($"min: {minHorizontal} max: {maxHorizontal}");
    }
    public float GetFloorVerticalBound()
    {
        Bounds bounds = floor.bounds;
        float maxVertical = bounds.max.y;
        return maxVertical;
    }
    public Environment_Door GetRandomDoors()
    {
        int maxBound = doors.Length;
        return doors[Random.Range(0, maxBound)];
    }
    public void PlayParticleSystem(bool input)
    {
        if (input) dustParticleSystem.Play();
        else dustParticleSystem.Stop();
    }
}
