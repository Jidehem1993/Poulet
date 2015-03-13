using UnityEngine;
using System.Collections;

public class Trap : MonoBehaviour {

	void Start ()
	{
		if (GetComponent <Collider> ())
		{
			GetComponent <Collider> ().isTrigger = true;
		}
	}

}
