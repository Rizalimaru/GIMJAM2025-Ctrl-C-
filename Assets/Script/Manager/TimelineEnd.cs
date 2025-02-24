
using UnityEngine;
using UnityEngine.Playables;


public class Timeline : MonoBehaviour
{   
    public PlayableDirector timeline;  // Drag PlayableDirector ke sini
    public string nextSceneName;       // Masukkan nama scene tujuan

    void Start()
    {
        timeline.stopped += OnTimelineEnd;
        if(nextSceneName == "Kamar"){
            AudioManager.Instance.PlayBackgroundMusicWithTransition("Intro",0,1f);
        }

        if(nextSceneName == "GamePlay"){
            AudioManager.Instance.StopBackgroundMusicWithTransition("Gameplay",0.4f);
        }

    void OnTimelineEnd(PlayableDirector director)
    {
        StartCoroutine(SceneController.instance.LoadScene(nextSceneName));
        if(nextSceneName == "Kamar"){
            AudioManager.Instance.StopBackgroundMusicWithTransition("Intro",1f);
        }


        if(nextSceneName == "MainMenu"){
            AudioManager.Instance.StopBackgroundMusicWithTransition("End",1f);
        }
        }
    }
}
