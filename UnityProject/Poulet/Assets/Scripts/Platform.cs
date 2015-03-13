using UnityEngine;
using System.Collections;

public abstract class Platform : MonoBehaviour
{

	protected bool landed;
	protected AudioSource au;
	protected Character player;

	public bool dontRespawnOnDeath;
	public bool muteLand;
	public bool muteWalk;
	public AudioClip [] surfaceSounds;
	public RespawnTrigger respawnTrigger;
	
	protected virtual void Spawn ()
	{
		landed = false;
		Unassign ();
	}
	
	protected void AddAudioSource ()
	{
		if (surfaceSounds.Length > 0) {
			au = gameObject.AddComponent <AudioSource> ();
			au.playOnAwake = false;
		}
	}
	
	protected bool IsPlayerAlive ()
	{
		return landed == (player && player.IsAlive ());
	}
	
	protected bool IsRespawnTriggered ()
	{
		return respawnTrigger && respawnTrigger.IsTriggered ();
	}
	
	public virtual void Land (Character lander)
	{
		player = lander;
		landed = true;
		if (!muteLand) {
			Step ();
		}
	}
	
	public void Unassign ()
	{
		player = null;
	}

	public void Step ()
	{
		if (player != null && surfaceSounds.Length > 0) {
			au.PlayOneShot (surfaceSounds [Random.Range (0, surfaceSounds.Length)]);
		}
	}
}
