using UnityEngine;
using System.Collections;

public class FadingPlatform : DespawningPlatform
{
	
	float cFadingTime;
	Renderer ren;
	public float fadingTime;

	protected override void Spawn ()
	{
		base.Spawn ();
		if (ren) {
			ren.material.color = new Color (ren.material.color.r, ren.material.color.g, ren.material.color.b, 1f);
		}
	}
	
	void Start ()
	{
		ren = GetComponent <Renderer> ();
		cRespawnTime = 0f;
		cFadingTime = 0f;
		AddAudioSource ();
	}
	
	void Update ()
	{
		if (IsRespawnTriggered () || (!dontRespawnOnDeath && !IsPlayerAlive ())) {
			Spawn ();
		}
		if (cFadingTime > 0f) {
			cFadingTime -= Time.deltaTime;
			if (cFadingTime <= 0f) {
				Despawn ();
			}
		}
		if (cRespawnTime > 0f) {
			cRespawnTime -= Time.deltaTime;
			if (cRespawnTime <= 0f) {
				Spawn ();
			}
		}
		if (landed && ren) {
			ren.material.color = new Color (ren.material.color.r, ren.material.color.g, ren.material.color.b, ren.material.color.a - Time.deltaTime / fadingTime);
		}
	}

	public override void Land (Character lander)
	{
		base.Land (lander);
		cFadingTime = fadingTime;
	}
}
