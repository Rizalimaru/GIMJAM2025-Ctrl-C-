using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    [HideInInspector] public Image leftImage;
    [HideInInspector] public Image rightImage;
    [HideInInspector] public string namaKiri;
    [HideInInspector] public string namaKanan;

    private Queue<DialogueLine> dialogueQueue;

    private void Start()
    {
        dialogEnd = false;
        dialogueQueue = new Queue<DialogueLine>();
        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);
        dialogueQueue.Clear();

        foreach (DialogueLine line in dialogue.lines)
        {
            dialogueQueue.Enqueue(line);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine currentLine = dialogueQueue.Dequeue();
        nameText.text = currentLine.characterName;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine.sentence));

        UpdateCharacterOpacity(currentLine.characterName);
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
        dialogEnd = true;
        dialoguePanel.SetActive(false);
    }
}