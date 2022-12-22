using UnityEngine;

public class InWaterTrigger : MonoBehaviour
{
    private Character character;

    private void Start()
    {
        character = GetComponentInParent<Character>(); 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Water") && character != null)
        {
            character.isInWater = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water") && character != null)
        {
            character.isInWater = false;
        }

    }
}
