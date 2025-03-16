using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetStageOnCollision : MonoBehaviour
{





	void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }


}
