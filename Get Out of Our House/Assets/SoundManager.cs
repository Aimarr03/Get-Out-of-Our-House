using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource sucking;
    public AudioSource walking;
    public AudioSource door;
    public AudioSource normalAmbient;
    public AudioSource ghostBusterAmbient;
    public AudioSource ambilPisau;

    public void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

}
