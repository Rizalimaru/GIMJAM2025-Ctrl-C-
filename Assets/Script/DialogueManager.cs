using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [System.Serializable]
    public class Dialogue
    {
        public string characterName;  // Nama karakter yang berbicara
        [TextArea(3, 10)]
        public string[] sentences;  // Array dialog
    }

    public TextMeshProUGUI nameText;  // UI untuk nama karakter
    public TextMeshProUGUI dialogueText;  // UI untuk dialog
    public GameObject dialoguePanel;  // Panel dialog yang bisa diaktifkan atau dinonaktifkan

    private Queue<string> sentences;  // Menyimpan daftar dialog yang sedang berjalan
    private string currentCharacter;  // Nama karakter saat ini

    private void Start()
    {
        sentences = new Queue<string>();
        dialoguePanel.SetActive(false); // Pastikan panel dialog mati di awal
    }

    public void StartDialogue(Dialogue dialogue)
    {
        dialoguePanel.SetActive(true);  // Tampilkan panel dialog
        currentCharacter = dialogue.characterName;
        nameText.text = currentCharacter;  // Set nama karakter
        sentences.Clear();  // Hapus dialog sebelumnya

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);  // Masukkan dialog ke dalam antrian
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence)); // Animasi mengetik
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.02f); // Efek mengetik
        }
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}
