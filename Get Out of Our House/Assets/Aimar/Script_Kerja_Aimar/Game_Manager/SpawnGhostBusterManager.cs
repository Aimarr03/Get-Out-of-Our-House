using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGhostBusterManager : MonoBehaviour
{
    [SerializeField] private Transform GhostBuster;
    //[SerializeField] private Transform SpawnPosition;

    private void Awake()
    {
        GhostBuster.gameObject.SetActive(false);
        //SpawnPosition.gameObject.SetActive(false);
    }
    public void SpawnGhostBuster()
    {
        Debug.Log("Ghost Buster is Spawned");
        GhostBuster.gameObject.SetActive(true);
    }
}
