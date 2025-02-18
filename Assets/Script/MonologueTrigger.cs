using UnityEngine;

public class MonologueTrigger : MonoBehaviour
{
    public MonologueManager monologueManager;
    public MonologueManager.Monologue monologue;

    private bool isMonologueActive = false; // Menandai apakah monolog sedang berlangsung

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.Space))
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
}
