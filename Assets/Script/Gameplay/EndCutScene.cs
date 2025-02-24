using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class EndCutScene : MonoBehaviour
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
