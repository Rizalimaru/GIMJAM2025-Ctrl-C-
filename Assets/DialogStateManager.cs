using UnityEngine;

public class DialogueStateManager : MonoBehaviour
{
    public static DialogueStateManager Instance;

    private int currentLineIndex = 0; // Menyimpan indeks dialog terakhir

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveCurrentLineIndex(int index)
    {
        currentLineIndex = index;
        PlayerPrefs.SetInt("CurrentDialogIndex", index);
        PlayerPrefs.Save();
    }

    public int LoadCurrentLineIndex()
    {
        return PlayerPrefs.GetInt("CurrentDialogIndex", 0);
    }

    public void ResetDialogueIndex()
    {
        currentLineIndex = 0;
        PlayerPrefs.SetInt("CurrentDialogIndex", 0);
        PlayerPrefs.Save();
    }
}
