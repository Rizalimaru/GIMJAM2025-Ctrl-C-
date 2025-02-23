using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JumbledSentenceGame : MonoBehaviour
{
    public TextMeshProUGUI sentenceDisplay;
    public GameObject wordButtonPrefab;
    public Transform wordButtonContainer;
    public Button resetButton;

    private List<string> correctSentence = new List<string> { "I", "love", "playing", "games" };
    private List<string> jumbledWords;
    private List<string> selectedWords = new List<string>();

    void Start()
    {
        GenerateJumbledWords();
        CreateWordButtons();
        resetButton.onClick.AddListener(ResetGame);
    }

    void GenerateJumbledWords()
    {
        jumbledWords = new List<string>(correctSentence);
        for (int i = 0; i < jumbledWords.Count; i++)
        {
            string temp = jumbledWords[i];
            int randomIndex = Random.Range(i, jumbledWords.Count);
            jumbledWords[i] = jumbledWords[randomIndex];
            jumbledWords[randomIndex] = temp;
        }
    }

    void CreateWordButtons()
    {
        foreach (string word in jumbledWords)
        {
            GameObject buttonObj = Instantiate(wordButtonPrefab, wordButtonContainer);
            Button button = buttonObj.GetComponent<Button>();
            Text buttonText = buttonObj.GetComponentInChildren<Text>();
            buttonText.text = word;
            button.onClick.AddListener(() => SelectWord(word, button));
        }
    }

    void SelectWord(string word, Button button)
    {
        selectedWords.Add(word);
        button.interactable = false;
        UpdateSentenceDisplay();
    }

    void UpdateSentenceDisplay()
    {
        sentenceDisplay.text = string.Join(" ", selectedWords);
        if (selectedWords.Count == correctSentence.Count)
        {
            CheckSentence();
        }
    }

    void CheckSentence()
    {
        if (string.Join(" ", selectedWords) == string.Join(" ", correctSentence))
        {
            sentenceDisplay.text += "\nCorrect!";
        }
        else
        {
            sentenceDisplay.text += "\nTry Again!";
        }
    }

    void ResetGame()
    {
        selectedWords.Clear();
        sentenceDisplay.text = "";
        foreach (Transform child in wordButtonContainer)
        {
            Destroy(child.gameObject);
        }
        GenerateJumbledWords();
        CreateWordButtons();
    }
}
