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
        public string characterName;  // Nama MC
        public Sprite characterAvatar; // Avatar MC
        [TextArea(3, 10)]
        public string[] sentences;  // Dialog MC
    }

    public TextMeshProUGUI nameText; // UI Nama MC
    public TextMeshProUGUI dialogueText; // UI Dialog MC
    public Image avatarImage; // UI Avatar MC
    public GameObject dialoguePanel; // Panel dialog
    public GameObject convoLogPanel; // Panel log percakapan
    public TextMeshProUGUI convoLogText; // UI untuk menampilkan log
    public Button convoLogButton; // Tombol untuk membuka log percakapan
    public Button skipButton; // Tombol untuk skip dialog
    public ScrollRect convoLogScrollRect; // ScrollRect untuk log percakapan
    public RectTransform convoLogContent; // Content dari log percakapan

    private Queue<string> sentences; // Antrian dialog
    private List<(string characterName, string sentence)> dialogueHistory; // Log percakapan dengan nama karakter
    private string currentCharacter; // Nama karakter saat ini

    private void Start()
    {
        sentences = new Queue<string>();
        dialogueHistory = new List<(string characterName, string sentence)>();
        dialoguePanel.SetActive(false); // Panel dialog tersembunyi di awal
        convoLogPanel.SetActive(false); // Panel log juga tersembunyi

        // Tambahkan event listener ke tombol
        convoLogButton.onClick.AddListener(OpenConvoLog);
        skipButton.onClick.AddListener(SkipDialogue);
    }

    public void StartMonologue(Monologue monologue)
    {
        dialoguePanel.SetActive(true);  // Tampilkan panel dialog
        currentCharacter = monologue.characterName;
        nameText.text = currentCharacter;  // Set nama MC
        avatarImage.sprite = monologue.characterAvatar; // Tampilkan avatar MC
        sentences.Clear();  // Hapus dialog sebelumnya

        foreach (string sentence in monologue.sentences)
        {
            sentences.Enqueue(sentence);  // Tambahkan dialog ke antrian
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndMonologue();
            return;
        }

        string sentence = sentences.Dequeue();
        dialogueHistory.Add((currentCharacter, sentence)); // Simpan ke log dialog dengan nama karakter
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence)); // Efek mengetik
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

    private void SkipDialogue()
    {
        StopAllCoroutines();
        if (sentences.Count > 0)
        {
            while (sentences.Count > 1) // Hapus semua kecuali terakhir
            {
                dialogueHistory.Add((currentCharacter, sentences.Dequeue()));
            }
            string lastSentence = sentences.Dequeue();
            dialogueHistory.Add((currentCharacter, lastSentence)); // Tambahkan dialog terakhir ke log
            dialogueText.text = lastSentence; // Tampilkan dialog terakhir
        }
        else
        {
            EndMonologue();
        }
    }

    private void OpenConvoLog()
    {
        convoLogPanel.SetActive(true);
        convoLogText.text = "";
        foreach (var entry in dialogueHistory)
        {
            string formattedSentence = FormatSentence(entry.sentence);
            convoLogText.text += $"<b>{entry.characterName}</b>     {formattedSentence}\n";
        }

        // Tunggu frame berikutnya sebelum scroll ke bawah
        StartCoroutine(ScrollToBottom());
    }
    

    private string FormatSentence(string sentence)
    {
        string[] words = sentence.Split(' ');
        string formatted = "";
        string line = "";
        int maxLength = 45; // Maksimal karakter dalam satu baris sebelum turun
        
        foreach (string word in words)
        {
            if ((line + word).Length > maxLength)
            {
                formatted += line.TrimEnd() + "\n                 "; // Tambah tabulasi untuk kelanjutan kalimat
                line = "";
            }
            line += word + " ";
        }
        
        formatted += line.TrimEnd();
        return formatted;
    }

    private IEnumerator ScrollToBottom()
    {
        yield return null; // Tunggu satu frame
        convoLogScrollRect.verticalNormalizedPosition = 0f; // Scroll ke bawah
    }

    private void EndMonologue()
    {
        dialoguePanel.SetActive(false);
    }
}
