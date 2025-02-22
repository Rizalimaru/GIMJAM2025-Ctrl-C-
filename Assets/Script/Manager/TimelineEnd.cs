
using UnityEngine;
using UnityEngine.Playables;


public class Timeline : MonoBehaviour
{   
    public PlayableDirector timeline;  // Drag PlayableDirector ke sini
    public string nextSceneName;       // Masukkan nama scene tujuan

    void Start()
    {
        timeline.stopped += OnTimelineEnd;
    }

    void OnTimelineEnd(PlayableDirector director)
    {
        StartCoroutine(SceneController.instance.LoadScene(nextSceneName));
    }
}
