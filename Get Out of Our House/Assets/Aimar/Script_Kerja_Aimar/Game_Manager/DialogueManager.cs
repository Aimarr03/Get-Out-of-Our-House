using System;
using System.Collections;
using TMPro;
using UnityEngine;
using VIDE_Data;

public class DialogueManager : MonoBehaviour
{
    public bool isActive;
    public event Action beginDialogue;
    public event Action endDialogue;
    public DialogueScriptableObject currentDialogue;
    [SerializeField] private TextMeshProUGUI dialogueContent;
    [SerializeField] private TextMeshProUGUI nameContent;
    
    [SerializeField] GameObject dialogueHolder;
    public static DialogueManager instance;
    private int currentLine;
    private int currentSegment;

    private void Awake()
    {
        if (instance != null) return;
        instance = this;
        //VD.LoadDialogues();
        isActive = false;
        
    }
    private void Start()
    {
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(false);
    }
    private void OnDialogueStop()
    {
        isActive = false;
        dialogueContent.text = "";
        nameContent.text = "";
        dialogueHolder.SetActive(false);
        PlayerControllerManager.instance.InvokeInterract -= OnInterractDialogue;
        endDialogue?.Invoke();
    }
    private void OnDialogueStart()
    {
        isActive = true;
        beginDialogue?.Invoke();
        PlayerControllerManager.instance.InvokeInterract += OnInterractDialogue;
        dialogueContent.text = "";
        nameContent.text = "";
        currentLine = 0;
        dialogueHolder.SetActive(true);
        OnInterractDialogue();
    }
    public void AssignDialogue(DialogueScriptableObject dialogueName)
    {
        currentDialogue = dialogueName;
        OnDialogueStart();
    }
    private void OnInterractDialogue()
    {
        if(currentLine < currentDialogue.entireDialogue.Count)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayDialogue());
            currentLine++;
        }
        else
        {
            OnDialogueStop();
        }
        
    }
    private IEnumerator DisplayDialogue()
    {
        string text = currentDialogue.entireDialogue[currentLine].comment;
        string name = currentDialogue.entireDialogue[currentLine].name;
        nameContent.text = name;
        string currentText = "";
        int index = 0;
        while (currentText != text)
        {
            currentText += text[index];
            dialogueContent.text = currentText;
            index++;
            yield return new WaitForSeconds(0.07f);
        }
        
    }
}
