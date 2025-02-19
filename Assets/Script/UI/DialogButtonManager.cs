using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogButtonManager : MonoBehaviour
{
    public GameObject convoLog;


    public void closeConvoLog()
    {
        convoLog.SetActive(false);
    }
}
