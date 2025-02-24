
using UnityEngine;

public class StopBack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
            AudioManager.Instance.StopBackgroundMusicWithTransition("Gameplay",1f);


                AudioManager.Instance.PlayBackgroundMusicWithTransition("End",0,1f);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
