using UnityEngine;
using UnityEngine.Events;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager instance;
    
    public UnityEvent<string> OnAchievementUnlocked = new UnityEvent<string>();

    private string[] achievements = {
        "Productivity? Whaz dat?",
        "Meow!",
        "Even When the Dualinggo Birds are Dead...",
        "Small Action Matters",
        "AIM Master!",
        "not Predator yet tho...",
        "One Step at a Time",
        "Theres Meaning Behind Every Existence"
    };

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    

    void Start()
    {
        if (!PlayerPrefs.HasKey("AchievementsInitialized"))
        {
            ResetAchievements();
            PlayerPrefs.SetInt("AchievementsInitialized", 1);
            PlayerPrefs.Save();
        }

        UnlockAchievement("Theres Meaning Behind Every Existence");
    }

    public void DebugUnlockedAchievements()
    {
        Debug.Log("=== Achievement yang Sudah Didapatkan ===");
        
        foreach (string achievement in achievements)
        {
            if (PlayerPrefs.HasKey(achievement))
            {
                Debug.Log("✅ " + achievement);
            }
            else
            {
                Debug.Log("❌ " + achievement);
            }
        }
    }


    public void UnlockAchievement(string achievementName)
    {
        Debug.Log($"Mencoba unlock achievement: '{achievementName}'"); // Debug nama yang masuk

        if (!PlayerPrefs.HasKey(achievementName))
        {
            PlayerPrefs.SetInt(achievementName, 1);
            PlayerPrefs.Save();
            Debug.Log($"✅ Achievement Unlocked: '{achievementName}'");
        }
        else
        {
            Debug.Log($"⚠️ Achievement '{achievementName}' sudah pernah didapat!");
        }
    }



    public void DebugAllPlayerPrefs()
    {
        Debug.Log("=== Semua PlayerPrefs yang Tersimpan ===");

        foreach (string achievement in achievements)
        {
            if (PlayerPrefs.HasKey(achievement))
            {
                int value = PlayerPrefs.GetInt(achievement);
                Debug.Log($"🔹 {achievement} = {value}");
            }
            else
            {
                Debug.Log($"❌ {achievement} TIDAK DITEMUKAN di PlayerPrefs");
            }
        }
    }



    public bool IsAchievementUnlocked(string achievementName)
    {
        bool unlocked = PlayerPrefs.HasKey(achievementName);
       // Debug.Log($"Cek Achievement: {achievementName}, Status: {unlocked}");
        return unlocked;
    }

    public void ResetAchievements()
    {
        foreach (string achievement in achievements)
        {
            PlayerPrefs.DeleteKey(achievement);
        }
        PlayerPrefs.DeleteKey("AchievementsInitialized");
        PlayerPrefs.Save();
    }
}
