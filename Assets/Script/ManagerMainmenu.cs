using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

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

    [SerializeField] private CanvasGroup mainMenu2CanvasGroup;
    [SerializeField] private GameObject mainMenu2CanvasGroupGameObject;
   
    private bool isMainMenu2on = false;


    
    // Start is called before the first frame update
    void Start()
    {
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
         SceneManager.LoadScene("Gameplay");
    }


    public void LoadGame(){
        if(isMainMenu2on == false){
            SetButtonsInteractable(false);

            mainMenu2CanvasGroupGameObject.SetActive(true);
            
            StartCoroutine(TransitionToMainMenu2Canvas());
            
        }

    }

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

    private IEnumerator TransitionToMainMenu2Canvas()
    {
        StartCoroutine(FadeCanvasGroup(mainMenu1CanvasGroup,1,0));
        yield return new WaitForSeconds(1f); // Jeda 1 detik
        mainMenu1CanvasGroupGameObject.SetActive(false);

        // Mengubah alpha dari game canvas menjadi 1 (tampil)
        StartCoroutine(FadeCanvasGroup(mainMenu2CanvasGroup, 0, 1));
        yield return new WaitForSeconds(1f); // Jeda 1 detik
        isMainMenu2on = true;

    }
}
