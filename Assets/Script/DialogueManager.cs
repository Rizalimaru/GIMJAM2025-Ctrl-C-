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
    private int currentLineIndex;
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
        
        dialoguePanel.SetActive(true);
        dialogueQueue.Clear();
        
        foreach (DialogueLine line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        currentLineIndex = -1;
        sceneChanged = false;

        // Atur nama karakter di sini (tidak lagi dari variabel global)
        namaKiri = leftName;
        namaKanan = rightName;

        // Set gambar NPC secara dinamis
        leftImage.sprite = leftSprite;
        rightImage.sprite = rightSprite;

        DisplayNextSentence();
    }



    public void DisplayNextSentence()
    {
        NPC_Interaction npc = FindObjectOfType<NPC_Interaction>();
        if (npc != null && npc.puzzleActive) return; 


        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        if (currentLineIndex == lineBeforeLoadScene && !sceneChanged && nextSceneName != null)
        {
            sceneChanged = true;
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Additive);
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();
        nameText.text = currentLine.characterName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));

        UpdateCharacterOpacity(currentLine.characterName);
        currentLineIndex++;
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

    public void ContinueDialogueAfterSceneLoad()
    {
        sceneChanged = false;
        DisplayNextSentence();
    }

    public void EndDialogue()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        SaveSlotSystem.instance.SaveNPCInteraction(currentSlot, gameObject.name); 

        SaveSlotSystem.instance.AutoSaveSlot();

        Invoke("LoadInteraction",0.2f);
        

        dialogEnd = true;
        dialoguePanel.SetActive(false);
    }
    public void LoadInteraction()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);
        
        Debug.Log("Memuat interaksi NPC untuk slot: " + currentSlot);

        SaveSlotSystem.instance.LoadNPCInteractions(currentSlot);
        SaveSlotSystem.instance.LoadSaveSlots();
        SaveSlotSystem.instance.LoadAutoSaveSlot();
    }

}