using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public bool hasEnded;
    public Image backgroundImage;
    public static LevelManager instance;

    public void Awake()
    {
        if(instance == null)
        instance = this;
    }
    private void Start()
    {
        DialogueManager.instance.endDialogue += Instance_endDialogue;
    }

    private void Instance_endDialogue()
    {
        throw new System.NotImplementedException();
    }

    public async void EndGame(string dialogue)
    {
        hasEnded = true;
        backgroundImage.enabled = true;
        await Task.Delay(1000);
        DialogueManager.instance.AssignDialogue(dialogue);
    }
}
