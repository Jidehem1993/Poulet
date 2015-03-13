using UnityEngine;
using System.Collections;

public class GlassPlatform : DespawningPlatform
{
	
	bool cracked;
	int crack;
	ParticleSystem shatter;
	public bool preCracked;
	public Material glass;
	public Material [] crackedGlass;
	public AudioClip [] crackSounds;
	public AudioClip [] breakSounds;
	public GameObject particles;
	
	void Start ()
	{
		cRespawnTime = 0f;
		cracked = preCracked;
		crack = Random.Range (0, crackedGlass.Length);
		if (particles) {
			shatter = particles.GetComponent <ParticleSystem> ();
		}
		AddAudioSource ();
		ResetTexture ();
	}
	
	void Update ()
	{
		if (IsRespawnTriggered () || (!dontRespawnOnDeath && !IsPlayerAlive ())) {
			Spawn ();
		}
		if (cRespawnTime > 0f) {
			cRespawnTime -= Time.deltaTime;
			if (cRespawnTime <= 0f) {
				Spawn ();
			}
		}
	}
	
	void Crack ()
	{
		if (crackSounds.Length > 0) {
			au.PlayOneShot (crackSounds [Random.Range (0, crackSounds.Length)]);
		}
		if (crackedGlass.Length > 0) {
			SetMaterial (crackedGlass [crack]);
		}
		cracked = true;
	}
	
	void SetMaterial (Material mat)
	{
		if (GetComponent <Renderer> ()) {
			GetComponent <Renderer> ().material = mat;
		}
	}
	
	void ResetTexture ()
	{
		if (!preCracked) {
			SetMaterial (glass);
		} else if (crackedGlass.Length > 0) {
			SetMaterial (crackedGlass [crack]);
		}
	}
	
	protected override void Spawn ()
	{
		base.Spawn ();
		cracked = preCracked;
		ResetTexture ();
	}
	
	protected override void Despawn ()
	{
		base.Despawn ();
		if (breakSounds.Length > 0) {
			au.PlayOneShot (breakSounds [Random.Range (0, breakSounds.Length)]);
		}
		if (shatter) {
			shatter.Play ();
		}
	}
	
	public override void Land (Character lander)
	{
		landed = true;
		player = lander;
		if (!cracked) {
			Crack ();
		} else {
			Despawn ();
		}
	}
}
