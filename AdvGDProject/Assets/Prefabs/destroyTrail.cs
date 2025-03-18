using UnityEngine;

public class destroyTrail : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy(gameObject,.05f);
    }
}
