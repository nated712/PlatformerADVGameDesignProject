using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public float roationSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, roationSpeed, 0);
    }
}
