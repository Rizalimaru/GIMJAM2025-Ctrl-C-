using UnityEngine;
using TMPro;

public class UIAchievementManager : MonoBehaviour
{
    public TextMeshProUGUI[] achievementTexts; // Assign di Inspector

    private string[] achievementNames = {
        "Productivity? Whaz dat?",
        "Meow!",
        "Even When the Dualinggo Birds are Dead...",
        "Small Action Matters",
        "AIM Master!",
        "not Predator yet tho...",
        "One Step at a Time",
        "Theres Meaning Behind Every Existence"
    };

    private void Start()
    {

        if (AchievementManager.instance != null)
        {
            AchievementManager.instance.OnAchievementUnlocked.AddListener(UpdateAchievementText);
            Debug.Log("Berhasil subscribe ke OnAchievementUnlocked");
        }
        else
        {
            Debug.LogError("AchievementManager tidak ditemukan di scene!");
        }


        // Cek achievement yang sudah didapatkan saat UI muncul
        UpdateAchievementUI();
        

    }


    void UpdateAchievementUI()
    {
        for (int i = 0; i < achievementNames.Length; i++)
        {
            if (AchievementManager.instance.IsAchievementUnlocked(achievementNames[i]))
            {
                achievementTexts[i].text = achievementNames[i]; // Tampilkan nama achievement
             
            }
            else
            {
                achievementTexts[i].text = "???"; // Jika belum dapat, tampilkan tanda tanya
            }
        }
    }

    void UpdateAchievementText(string unlockedAchievement)
    {
        for (int i = 0; i < achievementNames.Length; i++)
        {
            if (achievementNames[i] == unlockedAchievement)
            {
                achievementTexts[i].text = unlockedAchievement; // Update teks achievement
            }
        }
    }
}
