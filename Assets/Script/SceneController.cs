using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public static SceneController instance;
    [SerializeField] Animator transitionAnim;

    void Awake()
    {   
        transitionAnim = GetComponentInChildren<Animator>();
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }else
        {
            Destroy(gameObject);
        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        transitionAnim.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        transitionAnim.SetTrigger("End");
    }
}
