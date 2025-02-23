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
        Time.timeScale = 1f;
        handSpriteRenderer.sprite = readyHandSprite;
        gameOverUI.SetActive(false); // Sembunyikan UI Game Over di awal
        restartButton.onClick.AddListener(RestartGame); // Mengganti restartScene dengan RestartGame
        
        gameTimer = gameTimeLimit; // Set timer mulai dari 5 detik
        UpdateTimerUI();
    }

    void Update()
    {
        if (maling == null) 
        {
            return;
        }

        if(gameTimer >= 0)
        {
            gameTimer -= Time.deltaTime;
            UpdateTimerUI();
        }

        // Jika timer habis, Game Over
        if (gameTimer <= 0) 
        {
            GameOver();
            return;
        }

        if (isAiming && !isOnCooldown)
        {
            Vector3 targetPosition = maling.position + new Vector3(
                Mathf.Sin(Time.time * aimSpeed) * aimRange,
                Mathf.Cos(Time.time * aimSpeed) * aimRange,
                0
            );
            crosshair.transform.position = Vector3.Lerp(crosshair.transform.position, targetPosition, Time.deltaTime * aimSpeed);

            if (Input.GetKeyDown(KeyCode.Space) && crosshairCollider.IsTouching(malingCollider))
            {
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPowering = false;
                powerBar.gameObject.SetActive(false);
                ThrowSandal(power);
                handSpriteRenderer.sprite = throwHandSprite;
                StartCoroutine(ThrowCooldown());
            }
        }

        // Jika maling kena dan hilang, hentikan timer
        if (maling == null) 
        {
            gameObject.SetActive(false); // Matikan script jika maling sudah kena

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
        Time.timeScale = 0f;
        gameOverUI.SetActive(true); // Tampilkan UI Game Over
        isAiming = false;
        isPowering = false;
        if (copetMove != null) copetMove.Disable();
    }

    void RestartGame()
    {
        // Reset timer and UI
        gameTimer = gameTimeLimit;
        UpdateTimerUI();

        // Reset game objects and states here
        if (copetMove != null)
        {
            copetMove.Enable();
        }

        // Reset other states if necessary (for example, position, animation, etc.)
        handSpriteRenderer.sprite = readyHandSprite;
        powerBar.gameObject.SetActive(false);

        // Set the gameOverUI inactive to hide it after restart
        gameOverUI.SetActive(false);

        // Reset game logic
        isAiming = true;
        isPowering = false;
        isOnCooldown = false;
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
