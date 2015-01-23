using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ScreenBoundary))]
public class PlayerMovementLimit : MonoBehaviour 
{
	[SerializeField]
	private string tagCheck;

	[SerializeField]
	private Vector2 lockedMovementVector;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.tag == tagCheck)
		{
			Avatar player = other.gameObject.GetComponent<Avatar> ();
			if(player != null)
			{
				player.ForbidMovement(lockedMovementVector);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.gameObject.tag == tagCheck)
		{
			Avatar player = other.gameObject.GetComponent<Avatar> ();
			if(player != null)
			{
				player.AllowMovement(lockedMovementVector);
			}
		}
	}
}
