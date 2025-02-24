using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class EndingCutScene : MonoBehaviour
{
    public PlayableDirector Timeline;
    public string namaScene;
    public List<string> exceptionObjects;

    void Start()
    {
        Timeline.stopped += OnTimelineEnd;
    }
    void OnTimelineEnd(PlayableDirector director)
    {   
        SceneManager.UnloadSceneAsync(namaScene);
        Scene scene = SceneManager.GetSceneByName("Gameplay");


        foreach (GameObject obj in scene.GetRootGameObjects())
        {
            if(!exceptionObjects.Contains(obj.name))
            {
                obj.SetActive(true);
            }
        }
    }
}
