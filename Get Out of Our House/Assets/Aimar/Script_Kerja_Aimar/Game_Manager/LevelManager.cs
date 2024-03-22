using DialogueEditor;
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
        //EndGame("Tragedy");
    }

    private void Instance_endDialogue()
    {
        throw new System.NotImplementedException();
    }

    public void EndGame(NPCConversation dialogue)
    {
        hasEnded = true;
        backgroundImage.enabled = true;
        StartCoroutine(Delay(dialogue));
    }
    private IEnumerator Delay(NPCConversation npcConversation)
    {
        Debug.Log("Delay");
        yield return new WaitForSeconds(1.1f);
        Debug.Log("assigned Dialouge");
        ConversationManager.Instance.StartConversation(npcConversation);
    }
}
