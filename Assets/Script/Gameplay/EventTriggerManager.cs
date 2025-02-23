using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventTriggerManager : MonoBehaviour
{
    public string sceneName;
    public string triggerKey = "HasTriggered"; // Kunci unik untuk menyimpan status

    private void Awake()
    {
 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && PlayerPrefs.GetInt(triggerKey, 0) == 0)
        {
            Debug.Log("Player masuk ke trigger");
            PlayerPrefs.SetInt(triggerKey, 1); // Menandai trigger sudah terjadi
            PlayerPrefs.Save(); // Simpan data agar tetap ada setelah restart scene
            StartCoroutine(SceneController.instance.LoadScene(sceneName));
        }
    }
}
