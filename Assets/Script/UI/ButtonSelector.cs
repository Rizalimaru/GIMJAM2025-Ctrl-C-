using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSelector : MonoBehaviour
{
    public GameObject objectSettings;
    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(objectSettings.activeSelf){
            button.Select();

            button.transition = Selectable.Transition.None;
        }
        else{
            button.OnDeselect(null);
            button.transition = Selectable.Transition.ColorTint;
        }
        
    }
}
