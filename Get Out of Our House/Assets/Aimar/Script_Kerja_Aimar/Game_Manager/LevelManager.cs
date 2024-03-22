using DialogueEditor;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Animator animatorEnding;
    public bool hasEnded;
    public Image backgroundImage;
    public Sprite Ending_Fear;
    public Sprite Ending_Murder;
    public Sprite Lose;
    public static LevelManager instance;

    public void Awake()
    {
        if(instance == null)
        instance = this;
        animatorEnding = GetComponent<Animator>();
    }
    private void Update()
    {

    }

    private void Instance_endDialogue()
    {
        throw new System.NotImplementedException();
    }

    public void EndGame(DialogueScriptableObject dialogue)
    {
        SoundManager.instance.StopPlayingAll();
        switch (EndingManager.instance.currentEndingType)
        {
            case EndingManager.EndingType.Ending_Fear:
                backgroundImage.sprite = Ending_Fear;
                break;
            case EndingManager.EndingType.Ending_Murder: 
                backgroundImage.sprite= Ending_Murder;
                break;
            case EndingManager.EndingType.Lose:
                backgroundImage.sprite = Lose;
                break;
        }
        hasEnded = true;
        backgroundImage.gameObject.SetActive(true);
        StartCoroutine(Delay(dialogue));
    }
    private IEnumerator Delay(DialogueScriptableObject dialogue)
    {
        Debug.Log("Delay");
        yield return new WaitForSeconds(1.1f);
        animatorEnding.SetBool("HasEnded", true);
        Debug.Log("assigned Dialouge");
        DialogueManager.instance.AssignDialogue(dialogue);
    }
}
