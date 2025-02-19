using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : MonoBehaviour
{
    public GameObject panelconvoLog;

    public void Mainmenu(){
        SceneManager.LoadScene("Mainmenu");
    }
    public void CloseConvoLog(){
        panelconvoLog.SetActive(false);
    }
}
