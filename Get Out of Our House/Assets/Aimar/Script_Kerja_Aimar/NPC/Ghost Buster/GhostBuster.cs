using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Windows;

public class GhostBuster : MonoBehaviour,I_InterractableVisual
{
    public enum StateDirection
    {
        Left,
        Right
    }
    public StateDirection currentDirection = StateDirection.Left;
    private int sanity;
    private bool isVunerable;
    private int sanityArmor;
    private int maxSanityArmor;
    private GhostBuster_Move_Action moveAction;
    private Animator ghostBusterAnimator;
    public bool Insane;
    [SerializeField] private Room currentRoom;
    private Ghost ghost;
    private ParticleSystem agitatedEffect;
    [SerializeField] private float maxUndetectedDurationGhost;
    [SerializeField] private GameObject VisualAid;
    public event Action<bool, Room> GhostDetected;
    private float maxTimerGhostGone;
    public Transform CenterPosition;
    private void Awake()
    {
        Insane = false;
        moveAction = GetComponent<GhostBuster_Move_Action>();
        ghostBusterAnimator = transform.GetChild(0).GetComponent<Animator>();
        agitatedEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        CenterPosition = transform.GetChild(2);
        VisualAid = transform.GetChild(3).gameObject;
        VisualAid.SetActive(false);
        isVunerable = false;
        maxSanityArmor = 1;
        maxTimerGhostGone = 1.5f;
        sanityArmor = maxSanityArmor;
        sanity = 3;
    }
    private void Start()
    {
        Ghost.PosessingSomething += Ghost_PosessingSomething;
    }

    private void Ghost_PosessingSomething(Ghost input)
    {
        StopCoroutine(GhostDetectionLogicTimer(input));
        StartCoroutine(GhostDetectionLogicTimer(input));
    }
    private IEnumerator GhostDetectionLogicTimer(Ghost input)
    {
        if (Ghost.isPosessing && ghost != null && input.GetCurrentRoom() == GetCurrentRoom())
        {
            ghost = null;
            SetTrigger("Confused");
            yield return new WaitForSeconds(maxTimerGhostGone);
        }
        else
        {
            if (input.GetCurrentRoom() == GetCurrentRoom())
            {
                ghost = input;
                SetTrigger("Surprised");
            }
        }
        GhostDetected?.Invoke(ghost != null, null);
    }

    private void Update()
    {
        
    }
    public GhostBuster_Move_Action GetMoveAction() => moveAction;
    public void SetCurrentRoom(Room room)
    {
        currentRoom = room;
    }
    public void GetBoundsRoom(out float min, out  float max)
    {
        currentRoom.GetGroundHorizontalBound(out min, out max);
    }
    public Room GetCurrentRoom()
    {
        return currentRoom;
    }
    public Ghost IsGhostDetected()
    {
        return ghost;
    }
    public void SetGhostDetected(Ghost ghost)
    {
        Room currentGhostRoom = null;
        if (this.ghost != null) currentGhostRoom = this.ghost.GetCurrentRoom();
        Debug.Log(currentGhostRoom);
        //ghostBusterAnimator.SetTrigger("Surprised");
        this.ghost = ghost;
        GhostDetected?.Invoke(this.ghost != null, currentGhostRoom);
    }
    public bool Surprised()
    {
        Debug.Log("Ghost Buster is Surprised");
        sanityArmor--;
        ghostBusterAnimator.SetTrigger("Low Damage");
        if(sanityArmor <= 0)
        {
            isVunerable = true;
        }
        return sanityArmor <= 0;
    }
    public async void Fear()
    {
        Debug.Log("Ghost is disturbed");
        if(isVunerable)
        {
            ghostBusterAnimator.SetTrigger("Mid Damage");
            var mainModule = agitatedEffect.main;
            agitatedEffect.Stop();
            await Task.Delay(700);
            mainModule.duration -= 0.24f;
            agitatedEffect.Play();
            sanity--;
            sanityArmor = maxSanityArmor;
            isVunerable= false;
            if(sanity <= 0)
            {
                Insane = true;
                agitatedEffect.Stop();
                ghostBusterAnimator.SetBool("Dead", true);
                await Task.Delay(1200);
                LevelManager.instance.EndGame("Ghost_Buster_Defeated");
            }
        }
        
    }
    public bool CheckVunerability()
    {
        return isVunerable;
    }
    public void RemoveGhostDetection(Ghost ghost)
    {
        Room currentGhostRoom = ghost.GetCurrentRoom();
        Debug.Log(currentGhostRoom);
        this.ghost = null;
        GhostDetected?.Invoke(this.ghost != null, currentGhostRoom);
    }
    public Environment_Door GetDoorTowardsCertainRoom(Room room)
    {
        Debug.Log(room);
        return currentRoom.GetDoorToRoom(room);
    }
    public Animator GetAnimator()
    {
        return ghostBusterAnimator;
    }
    public void FlippingSprite(Vector3 comparison)
    {
        Vector3 direction = transform.position - comparison;
        if (direction.x != 0)
        {
            float rotation_y = direction.x > 0 ? 0 : 180;
            currentDirection = rotation_y == 0 ? StateDirection.Left : StateDirection.Right;
            transform.rotation = Quaternion.Euler(new Vector3(0, rotation_y, 0));
        }
    }
    public void SetTrigger(string trigger)
    {
        ghostBusterAnimator.SetTrigger(trigger);
    }

    public void SetLightInterractableVisual(bool input)
    {
        if (Insane) return;
        VisualAid.gameObject.SetActive(input);
    }
}
