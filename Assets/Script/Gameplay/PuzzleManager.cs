using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private const string PUZZLE_CLEAR_KEY = "PuzzleClear";

    public static void SetPuzzleClear(bool isClear)
    {
        PlayerPrefs.SetInt(PUZZLE_CLEAR_KEY, isClear ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static bool GetPuzzleClear()
    {
        return PlayerPrefs.GetInt(PUZZLE_CLEAR_KEY, 0) == 1;
    }
}