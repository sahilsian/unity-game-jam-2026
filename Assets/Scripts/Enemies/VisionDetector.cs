using UnityEngine;

public class VisionDetector : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyBase.IsPlayerSpotted = true;
            
            gameObject.SetActive(false); 
            
            Debug.Log("Spotted! Vision cone hidden.");
        }
    }

}