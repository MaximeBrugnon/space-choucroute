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
	    Debug.Log(other.ToString());
        if (other.CompareTag("Water"))
        {
	        character.stateMachine.ChangeState(character.swiming);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
	        character.stateMachine.ChangeState(character.standing);
        }

    }
}
