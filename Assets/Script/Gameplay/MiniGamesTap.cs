using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiniGameTap : MonoBehaviour
{
    public Slider progressBar;
    public GameObject winImage;
    public RawImage handImage;
    public Texture handTapTexture;
    public Texture handIdleTexture;
    public GameObject textEffectPrefab; // Prefab teks (+1)
    public Transform textEffectParent;  // Parent di dalam Canvas

    private float timer = 0f;
    private float maxTime = 60f;     // Biar gampang dites
    private float tapBoost = 1f;     // Tambahan progress per tap
    private float decayRate = 0.1f;  // Progress turun perlahan
    private bool isTapping = false;  // Deteksi apakah sedang tap

    private bool isFinished = false;

    void Start()
    {
        progressBar.maxValue = maxTime;
        progressBar.value = 0;
        winImage.SetActive(false);
    }

    void Update()
    {
        // Deteksi apakah tombol ditekan
        isTapping = Input.GetMouseButton(0);

        // Jika tidak tap, progress turun
        if (!isTapping && timer > 0 && isFinished == false)
        {
            timer -= decayRate * Time.deltaTime;
            timer = Mathf.Max(timer, 0); // Mencegah nilai negatif
        }

        // Update progress bar
        progressBar.value = timer;

        // Jika progress penuh, tampilkan kemenangan
        if (timer >= maxTime)
        {
            ShowWinScreen();
            isFinished = true;
        }

        // Tambahkan progress saat tap
        if (Input.GetMouseButtonDown(0) & isFinished == false)
        {
            TapScreen();
        }
    }

    void TapScreen()
    {
        timer += tapBoost;
        timer = Mathf.Min(timer, maxTime);

        handImage.texture = handTapTexture;
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
        handImage.texture = handIdleTexture; // Kembali ke tangan normal
    }

    void ShowWinScreen()
    {
        winImage.SetActive(true); // Tampilkan layar kemenangan
        Debug.Log("Menang! Dialog muncul.");
    }
}
