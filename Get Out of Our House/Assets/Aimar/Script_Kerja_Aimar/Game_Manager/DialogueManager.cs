using System;
using System.Collections;
using TMPro;
using UnityEngine;
using VIDE_Data;

public class DialogueManager : MonoBehaviour
{
    public event Action beginDialogue;
    public event Action endDialogue;
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
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(false);
    }
    private void OnDialogueStop()
    {
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(false);
        PlayerControllerManager.instance.InvokeInterract -= OnInterractDialogue;
        endDialogue?.Invoke();
    }
    private void OnDialogueStart()
    {
        beginDialogue?.Invoke();
        PlayerControllerManager.instance.InvokeInterract += OnInterractDialogue;
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
        if(dialogueContent.text == VD.nodeData.comments[VD.nodeData.commentIndex])
        {
            VD.Next();
        }
        StartCoroutine(ShowDialogueContent());
    }
    private IEnumerator ShowDialogueContent()
    {
        if (dialogueContent.text != VD.nodeData.comments[VD.nodeData.commentIndex])
        {
            string text = string.Empty;
            dialogueContent.text = text;
            nameContent.text = VD.nodeData.tag;
            while (text.Length < VD.nodeData.comments[VD.nodeData.commentIndex].Length)
            {
                text += VD.nodeData.comments[VD.nodeData.commentIndex][text.Length];
                dialogueContent.text = text;
                yield return new WaitForSeconds(0.01f);
            }
        }
        else
        {
            dialogueContent.text = VD.nodeData.comments[VD.nodeData.commentIndex];
        }
    }
}
