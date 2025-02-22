using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    public MonologueManager monologueManager;
    public MonologueManager.Monologue monologue;
    private bool objectReady = false;

    private bool isMonologueActive = false; // Menandai apakah monolog sedang berlangsung

    private void Update()
    {   
        
        if(objectReady && Input.GetMouseButtonDown(0))
        {
            if(!isMonologueActive)
            {
                monologueManager.StartMonologue(monologue);
                isMonologueActive = true;
            }
            else
            {
                monologueManager.DisplayNextSentence();
            }
        }
    }

    void OnMouseEnter()
    {
        objectReady = true;
    }

    void OnMouseExit()
    {
        objectReady = false;
    }
}
