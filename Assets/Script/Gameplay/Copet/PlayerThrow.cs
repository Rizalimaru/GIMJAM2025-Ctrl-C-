using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PThrow : MonoBehaviour
{
    public CopetMove copetMove;
    public GameObject sandalPrefab;
    public Transform throwPoint;
    public GameObject crosshair;
    public Slider powerBar;
    public Transform maling;
    public Collider2D crosshairCollider;
    public Collider2D malingCollider;
    public SpriteRenderer handSpriteRenderer;
    public Sprite readyHandSprite;
    public Sprite throwHandSprite;
    
    public GameObject gameOverUI; // UI Game Over
    public Button restartButton; // Tombol restart
    public TextMeshProUGUI timerText; // UI untuk menampilkan timer

    private bool isGameOver = false; // 🔹 Flag untuk menandakan game sudah berakhir


    private bool isAiming = true;
    private bool isPowering = false;
    private float power = 0f;
    private bool increasing = true;
    private Vector3 lockedTarget;
    private bool isOnCooldown = false;
    public float aimSpeed = 2f;
    public float aimRange = 1.5f;
    private float gameTimer;
    private float gameTimeLimit = 5f; // Timer mundur dari 5 detik
    public List<string> exceptionObjects;

    void Start()
    {
        AudioManager.Instance.PlayBackgroundMusicWithTransition2("Maling",0,1f,0.8f);
        Time.timeScale = 1f;
        handSpriteRenderer.sprite = readyHandSprite;
        gameOverUI.SetActive(false); // Sembunyikan UI Game Over di awal
        restartButton.onClick.AddListener(RestartGame); // Mengganti restartScene dengan RestartGame
        
        gameTimer = gameTimeLimit; // Set timer mulai dari 5 detik
        UpdateTimerUI();
    }

void Update()
{
    if (isGameOver) return; // 🔹 Cegah semua input jika game over

    if (gameTimer >= 0)
    {
        gameTimer -= Time.deltaTime;
        UpdateTimerUI();
    }

    if (gameTimer <= 0) 
    {
        GameOver();
        return;
    }

    if (isAiming && !isOnCooldown)
    {
        if (maling != null)
        {
            Vector3 targetPosition = maling.position + new Vector3(
                Mathf.Sin(Time.time * aimSpeed) * aimRange,
                Mathf.Cos(Time.time * aimSpeed) * aimRange,
                0
            );
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, targetPosition, Time.deltaTime * aimSpeed);
        }
        else
        {
            return;
        }

        if (!isGameOver && Input.GetKeyDown(KeyCode.Space) && crosshairCollider.IsTouching(malingCollider))
        {
            AudioManager.Instance.PlaySFX("Maling1",2);
            copetMove.Disable();
            isAiming = false;
            isPowering = true;
            powerBar.gameObject.SetActive(true);
            lockedTarget = crosshair.transform.position;
        }
    }
    else if (isPowering)
    {
        if (increasing)
            power += Time.deltaTime * 1.5f;
        else
            power -= Time.deltaTime * 1.5f;

        if (power >= 1f) increasing = false;
        if (power <= 0f) increasing = true;

        powerBar.value = power;

        if (!isGameOver && Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.Instance.PlaySFX("Maling1",0);
            isPowering = false;
            powerBar.gameObject.SetActive(false);
            ThrowSandal(power);
            handSpriteRenderer.sprite = throwHandSprite;
            StartCoroutine(ThrowCooldown());
        }
    }

    if (maling == null) 
    {
        ReturnToGame();
        return;
    }
}


    void ThrowSandal(float power)
    {
        Vector3 spawnPos = throwPoint.position;
        Vector3 direction = (lockedTarget - spawnPos).normalized;
        GameObject sandal = Instantiate(sandalPrefab, spawnPos, Quaternion.identity);
        sandal.GetComponent<Sandal>().SetDirection(direction, power);
    }

    IEnumerator ThrowCooldown()
    {
        if (copetMove != null && copetMove.gameObject != null) 
        {
            copetMove.Disable();
        }

        isOnCooldown = true;
        yield return new WaitForSeconds(2f);
        handSpriteRenderer.sprite = readyHandSprite;
        isAiming = true;
        isOnCooldown = false;

        if (copetMove != null && copetMove.gameObject != null) 
        {
            copetMove.Enable();
        }
    }

    void GameOver()
    {
    
        if (isGameOver) return;
        Debug.Log("🔴 GameOver() dipanggil! Musik seharusnya berhenti.");

        AudioManager.Instance.StopBackgroundMusicWithTransition("Maling",1f);

        isGameOver = true;
        Time.timeScale = 0f;
        gameOverUI.SetActive(true);
        isAiming = false;
        isPowering = false;
        isOnCooldown = false;

        powerBar.gameObject.SetActive(false);

        if (copetMove != null) copetMove.Disable();

        // 🔹 Nonaktifkan tombol restart sementara
        restartButton.interactable = false;
        StartCoroutine(EnableRestartButton());
    }

    IEnumerator EnableRestartButton()
    {
        yield return new WaitForSecondsRealtime(2f); // 🔹 Delay sebelum restart bisa ditekan
        restartButton.interactable = true;
}



void RestartGame()
{
    
    if (!gameOverUI.activeSelf) return;


    AudioManager.Instance.PlayBackgroundMusicWithTransition2("Maling",0,1f,0.8f);

    Debug.Log("🔄 RestartGame() dipanggil!");

    // Hapus semua sandal yang sudah dilempar
    foreach (GameObject sandal in GameObject.FindGameObjectsWithTag("Sandal"))
    {
        Destroy(sandal);
    }

    

    // Reset timer dan UI
    gameTimer = gameTimeLimit;
    UpdateTimerUI();
    Time.timeScale = 1f;

    // 🔹 Reset game objects dan states
    if (copetMove != null)
    {
        copetMove.Enable();
    }

    handSpriteRenderer.sprite = readyHandSprite;
    powerBar.gameObject.SetActive(false);

    // Sembunyikan UI Game Over setelah restart
    gameOverUI.SetActive(false);

    // 🔹 Reset game logic
    isAiming = true;   // ✅ Aktifkan kembali aim
    isPowering = false;
    isOnCooldown = false;
    isGameOver = false; // ✅ Pastikan game tidak dalam status game over lagi

    // 🔹 Pastikan crosshair bergerak kembali
    if (maling != null)
        {
            Vector3 targetPosition = maling.position + new Vector3(
                Mathf.Sin(Time.time * aimSpeed) * aimRange,
                Mathf.Cos(Time.time * aimSpeed) * aimRange,
                0
            );

            targetPosition.z -= 1f; // Sesuaikan angka 1f dengan seberapa jauh ingin mengurangi Z

            crosshair.transform.position = targetPosition; // 🔥 LANGSUNG SET POSISI CROSSHAIR
            Debug.Log("🔄 Crosshair di-reset ke posisi target: " + targetPosition);
        }

    

    
}


    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.Ceil(gameTimer).ToString(); // Tampilkan angka bulat
        }
    }

    public void ReturnToGame()
    {   
        int currentSlot = PlayerPrefs.GetInt("SelectedSaveSlot", 0);

        

        AudioManager.Instance.StopBackgroundMusicWithTransition("Maling",1f);

        AudioManager.Instance.PlayBackgroundMusicWithTransition("Gameplay",0,1f);
    
        SaveSlotSystem.instance.ModifyProgress(currentSlot, 2);
        SceneManager.UnloadSceneAsync("CopetMaling");
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
