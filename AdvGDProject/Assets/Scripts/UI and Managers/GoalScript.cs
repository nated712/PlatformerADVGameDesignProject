using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Timer timer; // Drag and drop Timer object in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ensure the player is tagged as "Player"
        {
            if (timer != null)
            {
                timer.StopTimer(); // Stop the timer
                Debug.Log("Player reached the goal! Timer stopped.");
            }
        }
    }
}
