using UnityEngine;
using System.Collections;

public class RespawnTrigger : MonoBehaviour
{
	
	bool triggered;
	
	void OnTriggerEnter (Collider c)
	{
		if (c.gameObject.GetComponent <Character> ()) {
			triggered = true;
		}
	}
	
	void OnTriggerExit (Collider c)
	{
		if (c.gameObject.GetComponent <Character> ()) {
			triggered = false;
		}
	}
	
	public bool IsTriggered ()
	{
		return triggered;
	}
}