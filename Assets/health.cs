using UnityEngine;

public class health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float healthPoints = 100f;
    public enemy enemyScript; // Reference to the enemy script
    
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("bullet"))
        {
            healthPoints -= 100f; // Decrease health by 10
            if (healthPoints <= 0f)
            {
                if(enemyScript)
                {
                    enemyScript.Dead(transform.position);
                }
            }
            Destroy(other.gameObject);
        }

    }
}
