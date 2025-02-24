using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class MiniGameTap : MonoBehaviour
{
    public Slider progressBar;
    public GameObject winImage;
    public RawImage handImage;
    public Texture handIdleTexture;
    public Texture handTapTexture1;
    public Texture handTapTexture2;
    public GameObject textEffectPrefab; // Prefab teks (+1)
    public Transform textEffectParent;  // Parent di dalam Canvas
    public GameObject audioPrefab;

    public List<string> exceptionObjects;

    private float timer = 0f;
    private float maxTime = 60f;   // Biar gampang dites
    private float tapBoost = 0.5f; // Tambahan progress per tap
    private float decayRate = 0.1f; // Progress turun perlahan
    private bool isFinished = false;
    private bool useFirstTapTexture = true; // Untuk mengatur gambar tangan

    void Start()
    {
        progressBar.maxValue = maxTime;
        progressBar.value = 0;
        winImage.SetActive(false);


        AudioManager.Instance.PlayBackgroundMusicWithTransition2("TapGame",0,7,0.8f);
    }

    void Update()
    {
        // Jika sudah selesai, hentikan input
        if (isFinished)
        {
            return;
        }

        // Jika tidak tap, progress turun perlahan
        if (!Input.anyKeyDown && timer > 0)
        {
            timer -= decayRate * Time.deltaTime;
            timer = Mathf.Max(timer, 0); // Mencegah nilai negatif
        }

        // Update progress bar
        progressBar.value = timer;

        if (Mathf.Abs(timer - maxTime) < 0.1f) // Gunakan toleransi
        {
            timer = maxTime; // Pastikan nilainya benar-benar maxTime
        }

        if (timer >= maxTime)
        {
            ShowWinScreen();
            isFinished = true;
            return;
        }


        // Tambahkan progress saat tap
        if (Input.anyKeyDown)
        {
            TapScreen();
        }
    }


    void TapScreen()
    {
        timer += tapBoost;
        timer = Mathf.Min(timer, maxTime);

        GameObject audioObject = Instantiate(audioPrefab, transform.position, Quaternion.identity);
        Destroy(audioObject, 2f); // Hancurkan setelah 2 detik agar tidak menumpuk

        // Bergantian antara dua gambar tap
        handImage.texture = useFirstTapTexture ? handTapTexture1 : handTapTexture2;
        useFirstTapTexture = !useFirstTapTexture; // Berubah setiap kali ditekan

        Invoke("ResetHand", 0.1f);

        if (textEffectPrefab && textEffectParent)
        {
            Vector2 localPoint;
            RectTransform canvasRect = textEffectParent as RectTransform;

            bool isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect, 
                Input.mousePosition, 
                null, 
                out localPoint
            );

            GameObject textObj = Instantiate(textEffectPrefab, textEffectParent);
            textObj.GetComponent<RectTransform>().anchoredPosition = localPoint;
            Destroy(textObj, 1f);
        }
    }

    void ResetHand()
    {
        handImage.texture = handIdleTexture; // Kembali ke tangan default
    }

    void ShowWinScreen()
    {
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        AchievementManager.instance.UnlockAchievement("not Predator yet tho...");



        AudioManager.Instance.StopBackgroundMusicWithTransition("TapGame", 1f);

        AudioManager.Instance.PlaySFX("WinMini",0);

        AudioManager.Instance.PlayBackgroundMusicWithTransition("Gameplay",0,1f);

        
        SaveSlotSystem.instance.ModifyProgress(currentSlot, 5);
        SceneManager.UnloadSceneAsync("TapTapBerhadiah");
        Scene scene = SceneManager.GetSceneByName("Gameplay");

        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if(!exceptionObjects.Contains(obj.name))
            {
                obj.SetActive(true);
            }
        }
        
    }
}
