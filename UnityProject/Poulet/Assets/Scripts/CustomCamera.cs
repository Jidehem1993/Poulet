using UnityEngine;
using System.Collections;

public class CustomCamera : MonoBehaviour
{

	bool control;
	float angle;
	float aspectRatio;
	Transform target;
	Character character;
	public float speed;
	public float distanceY;
	public float distanceZ;
	
	void Start ()
	{
		control = false;
		angle = 0f;
		aspectRatio = 0f;
		Cursor.visible = false;
	}
	
	void FixedUpdate ()
	{
		if (character) {
			if (control) {
				angle = speed * Input.GetAxis ("Mouse X");
			}
			aspectRatio = (float)Screen.width / (float)Screen.height;
			character.Turn (angle * aspectRatio);
			transform.position = target.position + target.up * distanceY + target.forward * -distanceZ;
			transform.LookAt (target);

		}
	}

	public void SetTarget (Transform tran)
	{
		target = tran;
		character = tran.GetComponent <Character> ();
	}
	
	public void SetControl (bool value) {
		control = value;
	}
}