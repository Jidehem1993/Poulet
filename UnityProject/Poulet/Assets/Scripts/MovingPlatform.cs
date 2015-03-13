using UnityEngine;
using System.Collections;

public class MovingPlatform : Platform
{

	bool start;
	bool linked;
	float cXTime;
	float cXSpeed;
	float cYTime;
	float cYSpeed;
	float cZTime;
	float cZSpeed;
	Vector3 startingPoint;
	public bool startAtContact;
	public float xTime;
	public float xSpeed;
	public float yTime;
	public float ySpeed;
	public float zTime;
	public float zSpeed;
	
	void Start ()
	{
		startingPoint = transform.position;
		Spawn ();
		AddAudioSource ();
	}
	
	void FixedUpdate ()
	{
		if (!startAtContact || start) {
			if (cXTime > 0f) {
				cXTime -= Time.fixedDeltaTime;
				if (cXTime <= 0f) {
					ChangeXDirection ();
				}
			}
			if (cYTime > 0f) {
				cYTime -= Time.fixedDeltaTime;
				if (cYTime <= 0f) {
					ChangeYDirection ();
					if (player && linked) {
						player.UpdateFriction (cYSpeed);
					}
				}
			}
			if (cZTime > 0f) {
				cZTime -= Time.fixedDeltaTime;
				if (cZTime <= 0f) {
					ChangeZDirection ();
				}
			}
			transform.position += new Vector3 (cXSpeed * Time.fixedDeltaTime, cYSpeed * Time.fixedDeltaTime, cZSpeed * Time.fixedDeltaTime);
		}
		if (player && linked) {
			player.transform.position += new Vector3 (cXSpeed * Time.fixedDeltaTime, 0f, cZSpeed * Time.fixedDeltaTime);
		}
		if (startAtContact && (IsRespawnTriggered () || (!dontRespawnOnDeath && !IsPlayerAlive ()))) {
			Spawn ();
		}
	}
	
	void ChangeXDirection ()
	{
		cXTime = xTime;
		cXSpeed = -cXSpeed;
	}
	
	void ChangeYDirection ()
	{
		cYTime = yTime;
		cYSpeed = -cYSpeed;
	}
	
	void ChangeZDirection ()
	{
		cZTime = zTime;
		cZSpeed = -cZSpeed;
	}
	
	protected override void Spawn ()
	{
		Unassign ();
		start = !startAtContact;
		linked = false;
		transform.position = startingPoint;
		cXTime = xTime;
		cXSpeed = xSpeed;
		cYTime = yTime;
		cYSpeed = ySpeed;
		cZTime = zTime;
		cZSpeed = zSpeed;
	}
	
	public override void Land (Character lander)
	{
		base.Land (lander);
		lander.UpdateFriction (cYSpeed);
		linked = true;
		if (startAtContact) {
			start = true;
		}
	}

	public void UpdateCharacter (Character target)
	{
		player = target;
	}

	public void Unlink ()
	{
		linked = false;
	}
}
