using UnityEngine;

public class SwimTrigger : MonoBehaviour
{
    private Character character;

    private void Start()
    {
        character = GetComponent<Character>();
    }

    private void OnTriggerEnter(Collider other)
    {
	    Debug.Log(other);
        if (other.CompareTag("Water"))
        {
	        character.SetState(new SwimingState(character));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
	        character.SetState(new StandingState(character));
        }

    }
}
