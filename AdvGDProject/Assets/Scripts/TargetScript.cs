using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public Target GreaterParent;
    public PlayerHealthManager phm;
    public void TakeDamage (float amount)
    {
        if (GreaterParent != null)
        {
            GreaterParent.TakeDamage(amount);
        }
        else
        {
            health -= amount;
            if (health <= 0f)
            {
                phm.TakeDamage(-300);
                Destroy(gameObject);

            }
        }
    }
}
