using UnityEngine;

public class PlatformMovementController : MonoBehaviour
{
    public float roationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, roationSpeed, 0);
    }
}
