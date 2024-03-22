using System;
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
    public event Action playerEnterRoom;
    [SerializeField] private Environment_Door[] doors;
    [SerializeField] private SpriteRenderer background;
    [SerializeField] private SpriteRenderer floor;
    [SerializeField] private float boundMultiplier;
    [SerializeField] private ParticleSystem dustParticleSystem;
    private List<GameObject> charactersInRoom;
    public RoomType type;
    private void Awake()
    {
        charactersInRoom = new List<GameObject>();
    }
    private void Start()
    {
        GetGroundHorizontalBound(out float minHorizontal, out float maxHorizontal);
    }
    public void GetGroundHorizontalBound(out float minHorizontal, out float maxHorizontal)
    {
        Bounds bounds = floor.bounds;
        minHorizontal = bounds.min.x;
        maxHorizontal = bounds.max.x;
        minHorizontal += 1.5f;
        maxHorizontal -= 1.5f;
        //Debug.Log($"min: {minHorizontal} max: {maxHorizontal}");
    }
    public void GetGroundHorizontalBoundForPlayer(out float minHorizontal, out float maxHorizontal)
    {
        Bounds bounds = floor.bounds;
        minHorizontal = bounds.min.x;
        maxHorizontal = bounds.max.x;
        minHorizontal += 1.4f;
        maxHorizontal -= 1.4f;
        //Debug.Log($"min: {minHorizontal} max: {maxHorizontal}");
    }
    public void GetGroundHorizontalBoundForCamera(out float minHorizontal, out float maxHorizontal)
    {
        Bounds bounds = floor.bounds;
        minHorizontal = bounds.min.x;
        maxHorizontal = bounds.max.x;
        //Debug.Log($"min: {minHorizontal} max: {maxHorizontal}");
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
        return doors[UnityEngine.Random.Range(0, maxBound)];
    }
    public void PlayParticleSystem(bool input)
    {
        if (input) dustParticleSystem.Play();
        else dustParticleSystem.Stop();
    }
    public void ExecutePlayerEnterRoomEvent()
    {
        playerEnterRoom?.Invoke();
    }
    public void AddCharacter(GameObject gameObject)
    {
        if(gameObject.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
        {
            foreach(GameObject currentGameObject in charactersInRoom)
            {
                if(currentGameObject.TryGetComponent<Ghost>(out Ghost ghostCurrent))
                {
                    if (Ghost.isPosessing) 
                    {
                        return;
                    } 
                    ghostBuster.SetGhostDetected(ghostCurrent);
                }
            }
        }
        else if(gameObject.TryGetComponent<Ghost>(out Ghost GhostCome))
        {
            foreach (GameObject currentGameObject in charactersInRoom)
            {
                if (currentGameObject.TryGetComponent(out GhostBuster GhostCurrentInTheSameRoom))
                {
                    GhostCurrentInTheSameRoom.SetGhostDetected(GhostCome);
                }
            }
        }
        //if (CheckCharacter(gameObject)) return;
        charactersInRoom.Add(gameObject);
    }
    public void RemoveCharacter(GameObject gameObject)
    {
        if (gameObject.TryGetComponent<Ghost>(out Ghost Ghost))
        {
            for (int i = charactersInRoom.Count - 1; i >= 0; i--)
            {
                GameObject currentGameObject = charactersInRoom[i];
                if (currentGameObject.TryGetComponent<GhostBuster>(out GhostBuster ghostBuster))
                {
                    ghostBuster.RemoveGhostDetection(Ghost);
                }
            }
        }
        if (CheckCharacter(gameObject)) charactersInRoom.Remove(gameObject);
    }
    public Environment_Door GetDoorToRoom(Room targetRoom)
    {
        foreach(Environment_Door environment_Door in doors)
        {
            Debug.Log("Environment Door " + environment_Door.GetRoomNextDoor());
            if(environment_Door.GetRoomNextDoor() == targetRoom)
            {
                Debug.Log(environment_Door);
                return environment_Door;
            }
        }
        return null;
    }
    public List<GameObject> GetPeople() => charactersInRoom;
    public bool CheckCharacter(GameObject gameObject) => charactersInRoom.Contains(gameObject);
}
