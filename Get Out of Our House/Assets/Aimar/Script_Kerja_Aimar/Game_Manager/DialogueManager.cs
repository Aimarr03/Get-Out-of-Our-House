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
    private int iteration;

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
        if (LevelManager.instance.hasEnded)
        {
            Debug.Log("Change into main menu");
            SceneLoader.LoadScene(0);
        }
    }
    private void OnDialogueStart()
    {
        isActive = true;
        beginDialogue?.Invoke();
        PlayerControllerManager.instance.InvokeInterract += OnInterractDialogue;
        dialogueContent.text = "";
        nameContent.text = "";
        
        dialogueHolder.SetActive(true);
        OnInterractDialogue();
    }
    public void AssignDialogue(DialogueScriptableObject dialogueName)
    {
        currentDialogue = dialogueName;
        currentLine = -1;
        OnDialogueStart();
    }
    private void OnInterractDialogue()
    {
        currentLine++;
        iteration++;
        if (currentLine < currentDialogue.entireDialogue.Count)
        {
            StopAllCoroutines();
            StartCoroutine(DisplayDialogue());
        }
        else
        {
            OnDialogueStop();
        }
        if(iteration > currentDialogue.entireDialogue.Count)
        {
            OnDialogueStop();
            iteration = 0;
        }
    }
    private IEnumerator DisplayDialogue()
    {
        string text = currentDialogue.entireDialogue[currentLine].name;
        string name = currentDialogue.entireDialogue[currentLine].comment;
        nameContent.text = name;
        string currentText = "";
        int index = 0;
        while (currentText != text)
        {
            currentText += text[index];
            dialogueContent.text = currentText;
            index++;
            yield return new WaitForSeconds(0.05f);
        }
        
    }
}
