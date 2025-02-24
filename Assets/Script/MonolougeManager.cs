using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonologueManager : MonoBehaviour
{
    [System.Serializable]
    public class Monologue
    {
        public string characterName;
        public Sprite characterAvatar;
        [TextArea(3, 10)]
        public string[] sentences;
    }

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Image avatarImage;
    public GameObject dialoguePanel;

    private Queue<string> sentences;
    private bool isTyping = false;

    private void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false);

        // Mengaktifkan auto-size agar teks menyesuaikan ukuran area
        dialogueText.enableAutoSizing = true;
        dialogueText.fontSizeMin = 10f; // Ukuran minimal agar tetap terbaca
        dialogueText.fontSizeMax = 36f; // Ukuran maksimal
    }

    public void StartMonologue(Monologue monologue)
    {
        dialoguePanel.SetActive(true);
        nameText.text = monologue.characterName;
        avatarImage.sprite = monologue.characterAvatar;
        sentences.Clear();

        foreach (string sentence in monologue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (!isTyping)
            {
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndMonologue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        dialogueText.text = sentence; // Tetapkan teks penuh dulu agar auto-size dapat menyesuaikan
        yield return null; // Tunggu satu frame untuk pembaruan ukuran

        dialogueText.text = ""; // Kosongkan sebelum mulai mengetik efek TypeWriter

        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f);
        }

        isTyping = false;
    }

    private void EndMonologue()
    {
        dialoguePanel.SetActive(false);
    }
}
