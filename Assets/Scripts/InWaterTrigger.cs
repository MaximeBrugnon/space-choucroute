using UnityEngine;

public class InWaterTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Character>() != null)
        {
            other.GetComponent<Character>().isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && other.GetComponent<Character>() != null)
        {
            other.GetComponent<Character>().isInWater = false;
        }

    }
}
