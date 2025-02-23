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


    [SerializeField] private GameObject[] buttonOptions;


    [SerializeField] private GameObject[] optionsDisplay;


    [SerializeField] private CanvasGroup mainMenu2CanvasGroup;
    [SerializeField] private GameObject mainMenu2CanvasGroupGameObject;

    [Header("----------- Function SETTINGS----------------")]


    public Toggle fullscreenCheck;
    public TextMeshProUGUI fullscreenStatusText; // UI Text untuk status

    [Header("----------- Journal----------------")]
        
    public GameObject jurnal;
    public GameObject[] contentJurnal;
   
    private bool isMainMenu2on = false;

    private bool displayOptions = false;
    private bool displayDelay = false;



    
    
    // Start is called before the first frame update
    void Start()
    {   
        AudioManager.Instance.PlayBackgroundMusicWithTransition("Mainmenu",0,2f);
        // Load setting sebelumnya
        bool isFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenCheck.isOn = isFullscreen;
        Screen.fullScreen = isFullscreen;

        // Update teks status awal
        UpdateFullscreenText(isFullscreen);

        // Tambahkan listener untuk Toggle
        fullscreenCheck.onValueChanged.AddListener(SetFullscreen);
        // Pastikan canvas game tidak terlihat pada awalnya
    }

    // Update is called once per frame
    void Update()
    {
        // Periksa apakah tombol apapun ditekan dan pastikan tidak ada transisi yang sedang berlangsung
        if (Input.anyKeyDown && !isTransitioning)
        {
            AudioManager.Instance.PlaySFX("Button",0);
            // Mengatur flag untuk menandakan transisi sedang berlangsung
            isTransitioning = true;

            // Mengubah alpha dari title screen menjadi 0 (sembunyi)
            StartCoroutine(FadeCanvasGroup2(titleCanvasGroup, 1, 0));

            StartCoroutine(TransitionToGameCanvas());
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if( displayOptions == true  ){

                SetButtonsInteractable(false);
                displayOptions = false;
                StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 1, 0));
                StartCoroutine(BackMainMenu());
            }
        }


    }

    public void StartGameplay(){
         SceneManager.LoadScene("GamePlay");
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void OpenMainMenu2(int index)
    {
        if (!isMainMenu2on)
        {
            // Menonaktifkan semua button agar tidak bisa diklik selama transisi
            // SetButtonsInteractable(false);

            // Pindah ke Main Menu 2
            StartCoroutine(TransitionToMainMenu2(index));
        }
    }


    private IEnumerator TransitionToMainMenu2(int index)
    {
        mainMenu2CanvasGroupGameObject.SetActive(true);
        ShowOptionDisplay(index);
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 0, 1));
        // Fade Out MainMenu1
        yield return new WaitForSeconds(1f);

        
        yield return new WaitForSeconds(1f); // Jeda
        SetButtonsInteractable(true);
        isMainMenu2on = true;

        // Tampilkan judul dan display yang sesuai
        
    }

    #region MAINMENU2

    public void ShowOptionDisplay(int index)
    {
        AudioManager.Instance.PlaySFX("Button",0);
        // Menonaktifkan semua display terlebih dahulu
        foreach (GameObject display in optionsDisplay)
        {
            display.SetActive(false);

        }

        // Mengaktifkan display sesuai dengan indeks yang dipilih
        if (index >= 0 && index < optionsDisplay.Length )
        {
            optionsDisplay[index].SetActive(true);

            if(index == 1){
                AudioManager.Instance.PlaySFX("Button",1);
            }

        }


        if( displayOptions == false)
        {   
            SetButtonsInteractable(false);
            StartCoroutine(TransitionDisplay());
        }
    }

    IEnumerator TransitionDisplay(){
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 0, 1));
        yield return new WaitForSeconds(1f);
        displayOptions = true;
        SetButtonsInteractable(true);

    }


    IEnumerator BackMainMenu(){

        //SetButtonsInteractable(false);
        yield return new WaitForSeconds(1.1f);
        foreach (GameObject display in optionsDisplay)
            {
                display.SetActive(false);
            }
        SetButtonsInteractable(true);
        //SetButtonsInteractable(true);
    }

    public void ShowJurnalUtama()
    {
        // Sembunyikan semua konten
        foreach (GameObject content in contentJurnal)
        {
            content.SetActive(false);
        }

        // Tampilkan jurnal utama
        jurnal.SetActive(true);
    }
    public void ShowContent(int index)
    {
        // Sembunyikan jurnal utama
        jurnal.SetActive(false);

        // Sembunyikan semua konten dulu
        foreach (GameObject content in contentJurnal)
        {
            content.SetActive(false);
        }

        // Tampilkan konten yang dipilih
        if (index >= 0 && index < contentJurnal.Length)
        {
            contentJurnal[index].SetActive(true);
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
        float duration = 0.8f; // Durasi fade dalam detik
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


    private IEnumerator FadeCanvasGroup2(CanvasGroup canvasGroup, float startAlpha, float endAlpha)
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
        foreach (GameObject buttonObj in buttonOptions)
        {
            ButtonSelector selector = buttonObj.GetComponent<ButtonSelector>();

            if (selector != null)
            {
                selector.SetButtonInteractable(state);
            }
        }
    }

    




    private void HapusTintButton()
    {
        foreach (GameObject btnObj in buttonOptions)
        {
            if (btnObj.TryGetComponent<ButtonSelector>(out ButtonSelector buttonSelector))
            {
                buttonSelector.DeselectButton();
            }
        }
    }



    private IEnumerator backMainMenu1(){
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 1, 0));
        yield return new WaitForSeconds(1f);
        mainMenu2CanvasGroupGameObject.SetActive(false);

        StartCoroutine(FadeCanvasGroup(mainMenu1CanvasGroup,0,1));
        SetButtonsInteractable(true);
    }

    // Coroutine untuk memberi jeda sebelum transisi ke game canvas
    private IEnumerator TransitionToGameCanvas()
    {
        yield return new WaitForSeconds(1f); // Jeda 1 detik
        titleCanvasGroupGameObject.SetActive(false);
        mainMenu1CanvasGroupGameObject.SetActive(true);

        SetButtonsInteractable(true);

    }


}
