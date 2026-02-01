using UnityEngine;
using System.Collections;

public class VisionDetector : MonoBehaviour
{
    private Coroutine spotCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            spotCoroutine = StartCoroutine(SpotPlayerWithDelay(2f));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (spotCoroutine != null)
            {
                StopCoroutine(spotCoroutine);
                spotCoroutine = null;
                Debug.Log("Player escaped! Aggro cancelled.");
            }
        }
    }

    private IEnumerator SpotPlayerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        
        EnemyBase.IsPlayerSpotted = true;
        gameObject.SetActive(false);
        Debug.Log("Spotted! Vision cone hidden.");
    }

}