using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 50f;
    public Target GreaterParent;

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
                Destroy(gameObject);

            }
        }
    }
}
