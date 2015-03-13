using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
	
	bool canJump;
	bool jumping;
	bool alive;
	bool finish;
	int cx;
	int cz;
	float frictionY;
	float stepDelay;
	float jumpDelay;
	float controlsDelay;
	float restartDelay;
	Vector3 checkpointPos;
	Quaternion checkpointRot;
	Rigidbody rig;
	Animation ani;
	Platform platform;
	public float jumpPower;
	public float airControl;
	public float gravity;
	public float speed;
	public float jumpMaxDelay;
	public float controlsMaxDelay;
	public float restartMaxDelay;
	public Texture finishScreen;
	public CustomCamera cam;
	public Particles glitter;
	
	void Start ()
	{
		canJump = true;
		jumping = true;
		alive = true;
		finish = false;
		cx = 0;
		cz = 0;
		frictionY = 0f;
		stepDelay = 0f;
		controlsDelay = controlsMaxDelay;
		SetCheckpoint ();
		rig = GetComponent <Rigidbody> ();
		ani = GetComponent <Animation> ();
		if (cam) {
			cam.SetTarget (transform);
		}
		if (glitter) {
			glitter.SetColorLerping (true);
		}
		platform = null;
		jumpPower = Mathf.Abs (jumpPower);
		gravity = Mathf.Abs (gravity);
		speed = Mathf.Abs (speed);
	}
	
	void FixedUpdate ()
	{
		float jump;
		float sideways;
		float updown;
		Vector3 movement;

		if (rig) {
			if (controlsDelay > 0f) {
				controlsDelay -= Time.fixedDeltaTime;
				if (controlsDelay <= 0f) {
					alive = true;
					if (cam) {
						cam.SetControl (true);
					}
				}
			} 
			if (!finish) {
				if (jumpDelay > 0f) {
					jumpDelay -= Time.fixedDeltaTime;
				} 
				jump = 0f;
				if (controlsDelay <= 0f) {
					jump = Input.GetAxis ("Saut");
				}
				sideways = 0f;
				if (controlsDelay <= 0f) {
					sideways = Input.GetAxis ("Horizontal");
				}
				updown = 0f;
				if (controlsDelay <= 0f) {
					updown = Input.GetAxis ("Vertical");
				}
				if (jump == 1f && !jumping && jumpDelay <= 0f && canJump) {
					jumping = true;
					Animate ("Jump", true);
					rig.velocity = Vector3.zero;
					rig.AddForce (new Vector3 (0f, jumpPower, 0f), ForceMode.Impulse);
				} else {
					movement = transform.forward * speed * updown + transform.right * speed * sideways + transform.up * rig.velocity.y;
					movement = new Vector3 (cx * movement.x < 0f ? 0f : movement.x, movement.y, cz * movement.z < 0f ? 0f : movement.z);
					rig.velocity = movement;
				}
				if (jumping && rig.velocity.y < 0f) {
					if (jump > 0f) {
						ani.Play ("Fly");
					}
					rig.AddForce (new Vector3 (0f, -gravity + airControl * jump, 0f));
				} else {
					rig.AddForce (new Vector3 (0f, -gravity, 0f));
				}
				transform.position = new Vector3 (transform.position.x, transform.position.y + frictionY * Time.fixedDeltaTime, transform.position.z);
				if (stepDelay > 0f) {
					stepDelay -= Time.fixedDeltaTime;
				}
				if (!jumping && stepDelay <= 0f && (sideways != 0f || updown != 0f)) {
					Animate ("Walk");
					stepDelay = 3f / speed;
					if (!platform.muteWalk) {
						platform.Step ();
					}
				}
				if (sideways == 0f && updown == 0f && ani.IsPlaying ("Walk")) {
					ani.Stop ();
				}
				if (!jumping && controlsDelay <= 0f && Input.GetAxis ("Action") > 0f) {
					Animate ("Peck");
				}
			} else {
				rig.velocity = Vector3.zero;
				if (restartDelay > 0f) {
					restartDelay -= Time.fixedDeltaTime;
				} else if (Input.GetAxis ("Saut") > 0f) {
					Application.LoadLevel (Application.loadedLevel);
				}
			}
		}
	}
	
	void OnCollisionEnter (Collision c)
	{
		if (CY (c) == 1) {
			canJump = true;
			if (platform = c.gameObject.GetComponent <Platform> ()) {
				Animate ("Zero");
				platform.Land (this);
				jumpDelay = jumpMaxDelay;
				if (c.gameObject.GetComponent <GlassPlatform> ()) {
					if (platform.GetComponent <Collider> ().isTrigger) {
						canJump = false;
					}
				}
				if (c.gameObject.GetComponent <CheckpointPlatform> () && !c.gameObject.GetComponent <CheckpointPlatform> ().IsUsed ()) {
					if (c.gameObject.CompareTag ("Finish")) {
						finish = true;
						cam.SetControl (false);
						restartDelay = restartMaxDelay;
						glitter.Play ();
					}
					c.gameObject.GetComponent <CheckpointPlatform> ().Use ();
					SetCheckpoint ();
				}
			}
		}
	}
	
	void OnCollisionStay (Collision c)
	{
		cx = CX (c);
		cz = CZ (c);
		if (CY (c) == 1) {
			jumping = false;
			if (c.gameObject.GetComponent <MovingPlatform> ()) {
				c.gameObject.GetComponent <MovingPlatform> ().UpdateCharacter (this);
			}
		}
	}
	
	void OnCollisionExit (Collision c)
	{
		cx = 0;
		cz = 0;
		jumping = true;
		if (c.gameObject.GetComponent <MovingPlatform> ()) {
			c.gameObject.GetComponent <MovingPlatform> ().Unlink ();
			frictionY = 0f;
		}
	}
	
	void OnTriggerEnter (Collider c)
	{
		if (c.gameObject.GetComponent <Trap> ()) {
			Die ();
		}
	}

	void OnGUI ()
	{
		if (finish) {
			GUI.DrawTexture (new Rect (0f, 0f, Screen.width, Screen.height), finishScreen);
		}
	}

	void Animate (string animName, bool fromDefault = false) {
		if (fromDefault) {
			ani.Stop ();
			ani.Play ("Zero");
			ani.Stop ();
		}
		ani.Play (animName);
	}
	
	int CX (Collision c)
	{
		return Mathf.RoundToInt (c.contacts [0].normal.x);
	}
	
	int CY (Collision c)
	{
		return Mathf.RoundToInt (c.contacts [0].normal.y);
	}
	
	int CZ (Collision c)
	{
		return Mathf.RoundToInt (c.contacts [0].normal.z);
	}
	
	void Die ()
	{
		alive = false;
		LoadCheckpoint ();
	}
	
	void LoadCheckpoint ()
	{
		cx = 0;
		cz = 0;
		transform.position = checkpointPos;
		transform.rotation = checkpointRot;
		rig.velocity = Vector3.zero;
		controlsDelay = controlsMaxDelay;
		cam.SetControl (false);
	}

	public void UpdateFriction (float friction)
	{
		frictionY = friction;
	}
	
	public void SetCheckpoint ()
	{
		checkpointPos = transform.position;
		checkpointRot = transform.rotation;
	}
	
	public bool IsAlive ()
	{
		return alive;
	}
	
	public void Turn (float angle)
	{
		if (!jumping) {
			if (Mathf.Abs (angle) > 0.5f) {
				Animate ("Turn");
			} else if (ani.IsPlaying ("Turn")) {
				ani.Stop ();
			}
		}
		transform.Rotate (new Vector3 (0f, angle, 0f));
	}
}
