using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ManagerMainmenu : MonoBehaviour
{
    [Header("----------- Function Mainmenu1----------------")]
 
    [SerializeField] private CanvasGroup titleCanvasGroup; // CanvasGroup untuk title screen
    [SerializeField] private GameObject titleCanvasGroupGameObject;
    [SerializeField] private CanvasGroup mainMenu1CanvasGroup; // CanvasGroup untuk game screen
    [SerializeField] private GameObject mainMenu1CanvasGroupGameObject;


    private bool isTransitioning = false; // Flag untuk mencegah spam klik

    [Header("----------- Function Mainmenu2----------------")]

    public TextMeshProUGUI setJudulTeks;

    [SerializeField] private GameObject[] buttonOptions;


    [SerializeField] private GameObject[] optionsDisplay;

    private string[] judulOptions = { "Load", "Jurnal", "Pengaturan", "Tentang" };



    [SerializeField] private CanvasGroup mainMenu2CanvasGroup;
    [SerializeField] private GameObject mainMenu2CanvasGroupGameObject;

    [Header("----------- Function SETTINGS----------------")]


    public Toggle fullscreenCheck;
    public TextMeshProUGUI fullscreenStatusText; // UI Text untuk status
   
    private bool isMainMenu2on = false;


    
    // Start is called before the first frame update
    void Start()
    {   
        // Load setting sebelumnya
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenCheck.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        // Update teks status awal
        UpdateFullscreenText(isFullscreen);

        // Tambahkan listener untuk Toggle
        fullscreenCheck.onValueChanged.AddListener(SetFullscreen);
        // Pastikan canvas game tidak terlihat pada awalnya
        mainMenu1CanvasGroup.alpha = 0;
        mainMenu1CanvasGroup.interactable = false;
        mainMenu1CanvasGroup.blocksRaycasts = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Periksa apakah tombol apapun ditekan dan pastikan tidak ada transisi yang sedang berlangsung
        if (Input.anyKeyDown && !isTransitioning)
        {
            // Mengatur flag untuk menandakan transisi sedang berlangsung
            isTransitioning = true;

            // Mengubah alpha dari title screen menjadi 0 (sembunyi)
            StartCoroutine(FadeCanvasGroup(titleCanvasGroup, 1, 0));

            // Menunggu sebentar sebelum memulai transisi canvas game
            StartCoroutine(TransitionToGameCanvas());
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(mainMenu2CanvasGroupGameObject.activeSelf && isMainMenu2on == true){
                isMainMenu2on = false;
                StartCoroutine(backMainMenu1());
            }
        }


    }

    public void StartGameplay(){
         SceneManager.LoadScene("GamePlay");
    }


    public void OpenMainMenu2(int index)
    {
        if (!isMainMenu2on)
        {
            // Menonaktifkan semua button agar tidak bisa diklik selama transisi
            SetButtonsInteractable(false);

            // Pindah ke Main Menu 2
            StartCoroutine(TransitionToMainMenu2(index));
        }
    }

    private IEnumerator TransitionToMainMenu2(int index)
    {
        ShowOptionDisplay(index);
        // Fade Out MainMenu1
        StartCoroutine(FadeCanvasGroup(mainMenu1CanvasGroup, 1, 0));
        yield return new WaitForSeconds(1f);
        mainMenu1CanvasGroupGameObject.SetActive(false);

        // Aktifkan Main Menu 2
        mainMenu2CanvasGroupGameObject.SetActive(true);
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 0, 1));

        yield return new WaitForSeconds(1f); // Jeda

        isMainMenu2on = true;

        // Tampilkan judul dan display yang sesuai
        
    }

    #region MAINMENU2

    public void ShowOptionDisplay(int index)
    {
        // Menonaktifkan semua display terlebih dahulu
        foreach (GameObject display in optionsDisplay)
        {
            display.SetActive(false);
        }

        // Mengaktifkan display sesuai dengan indeks yang dipilih
        if (index >= 0 && index < optionsDisplay.Length)
        {
            optionsDisplay[index].SetActive(true);

            // Mengubah teks judul sesuai indeks
            setJudulTeks.text = judulOptions[index];
        }
    }

    #endregion


    #region SETTINGS
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        // Update teks status saat toggle berubah
        UpdateFullscreenText(isFullscreen);
    }

    void UpdateFullscreenText(bool isFullscreen)
    {
        fullscreenStatusText.text = isFullscreen ? "ON" : "OFF";
    }

    #endregion

    // Coroutine untuk mengubah alpha dengan smooth
    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
    {
        float duration = 0.9f; // Durasi fade dalam detik
        float timeElapsed = 0f;

        // Menentukan alpha awal dan alpha tujuan
        canvasGroup.alpha = startAlpha;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        // Melakukan interpolasi alpha
        while (timeElapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Menyelesaikan alpha
        canvasGroup.alpha = endAlpha;

        // Setelah selesai fade, kita bisa mengatur interaksi dan raycasts jika diperlukan
        if (endAlpha == 0)
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    private void SetButtonsInteractable(bool state)
    {
        foreach (GameObject button in buttonOptions)
        {
            button.GetComponent<UnityEngine.UI.Button>().interactable = state;
        }
    }


    private IEnumerator backMainMenu1(){
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 1, 0));
        yield return new WaitForSeconds(1f);
        mainMenu2CanvasGroupGameObject.SetActive(false);

        mainMenu1CanvasGroupGameObject.SetActive(true);

        StartCoroutine(FadeCanvasGroup(mainMenu1CanvasGroup,0,1));
        SetButtonsInteractable(true);
    }

    // Coroutine untuk memberi jeda sebelum transisi ke game canvas
    private IEnumerator TransitionToGameCanvas()
    {
        yield return new WaitForSeconds(1f); // Jeda 1 detik
        titleCanvasGroupGameObject.SetActive(false);
        mainMenu1CanvasGroupGameObject.SetActive(true);

        // Mengubah alpha dari game canvas menjadi 1 (tampil)
        StartCoroutine(FadeCanvasGroup(mainMenu1CanvasGroup, 0, 1));
        SetButtonsInteractable(true);

    }


}
