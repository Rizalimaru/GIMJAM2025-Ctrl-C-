using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        public string characterName;
        [TextArea(3, 10)]
        public string sentence;
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueLine> lines;
    }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public GameObject dialoguePanel;
    public bool dialogEnd;
    [HideInInspector]public int lineBeforeLoadScene;
    [HideInInspector]public string nextSceneName;

    [HideInInspector] public Image leftImage;
    [HideInInspector] public Image rightImage;
    [HideInInspector] public string namaKiri;
    [HideInInspector] public string namaKanan;

    private Queue<DialogueLine> dialogueQueue;
    public static int currentLineIndex = -1;

    private bool sceneChanged;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        dialogEnd = false;
        dialogueQueue = new Queue<DialogueLine>();
        dialoguePanel.SetActive(false);
        currentLineIndex = -1;
        sceneChanged = false;
    }

    public void StartDialogue(Dialogue dialogue, string leftName, string rightName, Sprite leftSprite, Sprite rightSprite)
    {
        Debug.Log("Starting dialogue with " + leftName + " and " + rightName);
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = false;
        currentLineIndex = -1;
        dialoguePanel.SetActive(true);
        dialogueQueue.Clear();
        
        foreach (DialogueLine line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        currentLineIndex = NPC_Interaction.savedLineIndex; // Gunakan index yang tersimpan
        sceneChanged = false;

        namaKiri = leftName;
        namaKanan = rightName;

        leftImage.sprite = leftSprite;
        rightImage.sprite = rightSprite;

        Debug.Log($"Melanjutkan dari dialog index: {currentLineIndex}");
        ContinueDialogueFromSavedIndex(); 
    }   

    private void ContinueDialogueFromSavedIndex()
    {   
        
        for (int i = 0; i < currentLineIndex; i++)
        {
            if (dialogueQueue.Count > 0)
            {
                dialogueQueue.Dequeue();
            }
        }

        DisplayNextSentence();
    }


    public void DisplayNextSentence()
    {
        NPC_Interaction npc = FindObjectOfType<NPC_Interaction>();
        if (npc != null && npc.puzzleActive) return; 

        Debug.Log("DisplayNextSentence: Current Index = " + currentLineIndex + ", Queue Count = " + dialogueQueue.Count);

        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (currentLineIndex == lineBeforeLoadScene && !sceneChanged && nextSceneName != null)
        {
            sceneChanged = true;
            return;
        }

        if (dialogueQueue.Count > 0)
        {
            DialogueLine currentLine = dialogueQueue.Dequeue();
            nameText.text = currentLine.characterName;
            StopAllCoroutines();
            StartCoroutine(TypeSentence(currentLine.sentence));

            UpdateCharacterOpacity(currentLine.characterName);
            
            currentLineIndex++;
            Debug.Log("Setelah DisplayNextSentence: Current Index = " + currentLineIndex);
        }
    }


    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }
    }

    private void UpdateCharacterOpacity(string speaker)
    {
        if (speaker == namaKiri)
        {
            leftImage.color = new Color(leftImage.color.r, leftImage.color.g, leftImage.color.b, 1f);
            rightImage.color = new Color(rightImage.color.r, rightImage.color.g, rightImage.color.b, 0.5f);
        }
        else if (speaker == namaKanan)
        {
            leftImage.color = new Color(leftImage.color.r, leftImage.color.g, leftImage.color.b, 0.5f);
            rightImage.color = new Color(rightImage.color.r, rightImage.color.g, rightImage.color.b, 1f);
        }
    }

    public void EndDialogue()
    {   
        PlayerController player = FindObjectOfType<PlayerController>();
        player.canMove = true;
        dialogEnd = true;
        dialoguePanel.SetActive(false);
        NPC_Interaction.savedLineIndex = -1;
        currentLineIndex = -1;
    }



}