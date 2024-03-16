using System.Collections;
using TMPro;
using UnityEngine;
using VIDE_Data;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueContent;
    [SerializeField] private TextMeshProUGUI nameContent;

    [SerializeField] GameObject dialogueHolder;

    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        
    }
    private void Start()
    {
        OnDialogueStop();
    }
    private void OnDialogueStop()
    {
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(false);
    }
    private void OnDialogueStart()
    {
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(true);
    }
    public void AssignDialogue(string dialogueName)
    {
        OnDialogueStart();
        VD.OnNodeChange += ChangeDialogueNodeAction;
        VD.OnEnd += EndDialogue;
        VD.BeginDialogue(dialogueName);
    }
    private void ChangeDialogueNodeAction(VD.NodeData nodeData)
    {
        StartCoroutine(ShowDialogueContent());
    }
    private void EndDialogue(VD.NodeData nodeData)
    {
        OnDialogueStop();
        VD.OnNodeChange -= ChangeDialogueNodeAction;
        VD.OnEnd -= EndDialogue;
    }
    private void OnInterractDialogue()
    {

    }
    private IEnumerator ShowDialogueContent()
    {
        string text = string.Empty;
        dialogueContent.text = text;
        while (text.Length < VD.nodeData.comments[VD.nodeData.commentIndex].Length)
        {
            text += VD.nodeData.comments[VD.nodeData.commentIndex][text.Length];
            dialogueContent.text = text;
            yield return new WaitForSeconds(0.01f);
        }

        //Automatically call next.
        yield return new WaitForSeconds(1f);
        VD.Next();
    }
}
