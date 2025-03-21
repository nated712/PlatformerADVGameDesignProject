using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class TurretCountManager : MonoBehaviour
{
    private int activeTurrets; // Track the number of active turrets

    public Timer timer; // Reference to the Timer script to reduce time
    public TextMeshProUGUI turretCountText; // Reference to the TextMeshPro UI element

    void Start()
    {
        // Initialize the count of active turrets
        UpdateTurretCount();
        UpdateTurretCountDisplay(); // Update the display at the start
    }

    // This method is called when a turret is destroyed
    public void OnTurretDestroyed()
    {
        activeTurrets--; // Decrease the active turret count when one is destroyed

        // Print the current active turret count for debugging
        Debug.Log("Active Turrets: " + activeTurrets);

        // Update the UI
        UpdateTurretCountDisplay();

        // If all turrets are destroyed, reduce the player's time
        if (activeTurrets <= 0)
        {
            timer.ReduceTime(); // Call ReduceTime method in Timer
        }
    }

    // This method updates the count of active turrets at runtime
    public void UpdateTurretCount()
    {
        activeTurrets = GameObject.FindGameObjectsWithTag("Turret").Length; // Count turrets by tag
    }

    // This method updates the TextMeshPro UI element with the current turret count
    public void UpdateTurretCountDisplay()
    {
        if (turretCountText != null)
        {
            turretCountText.text = "Turrets: " + activeTurrets; // Update the UI text
        }
    }
}
