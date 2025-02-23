using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;

public class JumbledSentencesGame : MonoBehaviour
{
    public TextMeshProUGUI foreignSentenceText;
    public Transform wordContainer;
    public Transform slotContainer;
    public GameObject wordPrefab;
    public GameObject slotPrefab;

    public Button resetButton;

    private List<string> correctWords = new List<string>
    {
        "Can", "you", "recommend", "some", "easy", "books", "about",
        "this", "nation's", "history", "or", "culture", "so", "I", "can",
        "also", "learn", "the", "language", "?"
    };
    private List<string> shuffledWords;

    void Start()
    {
        resetButton.onClick.AddListener(RestartGame);
            if (wordPrefab == null)
            {
                Debug.LogError("wordPrefab belum diassign di Inspector!");
                return;
            }
            
            if (wordContainer == null)
            {
                Debug.LogError("wordContainer belum diassign di Inspector!");
                return;
            }
        // Set teks bahasa asing di atas
        foreignSentenceText.text = "Saya sedang memainkan permainan ini.";

        // Acak kata-kata
        shuffledWords = correctWords.OrderBy(x => Random.value).ToList();

        float slotSpacingX = 200f; // Jarak horizontal antar slot
        float slotSpacingY = -50; // Jarak vertikal antar slot
        int maxPerRow = 5; // Maksimal 5 prefab per baris

        // Buat slot untuk jawaban
        for (int i = 0; i < correctWords.Count; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotContainer);
            slotObj.GetComponent<DropSlotSentences>().correctWord = correctWords[i];

            // Hitung posisi dalam grid
            int row = i / maxPerRow;
            int col = i % maxPerRow;

            RectTransform rect = slotObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(col * slotSpacingX, row * slotSpacingY);
        }

        // Buat kata-kata acak yang bisa diseret
        float wordSpacingX = 200f; // Jarak horizontal antar kata
        float wordSpacingY = -50f; // Jarak vertikal antar kata

        for (int i = 0; i < shuffledWords.Count; i++)
        {
            GameObject wordObj = Instantiate(wordPrefab, wordContainer);
            wordObj.GetComponentInChildren<TextMeshProUGUI>().text = shuffledWords[i];
            wordObj.name = shuffledWords[i];

            // Hitung posisi dalam grid
            int row = i / maxPerRow;
            int col = i % maxPerRow;

            RectTransform rect = wordObj.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(col * wordSpacingX, row * wordSpacingY);
        }





    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
