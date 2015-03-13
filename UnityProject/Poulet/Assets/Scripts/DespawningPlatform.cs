using UnityEngine;
using System.Collections;

public abstract class DespawningPlatform : Platform {
	
	protected float cRespawnTime;
	public float respawnTime;
	
	protected virtual void Despawn ()
	{
		if (GetComponent <Collider> ()) {
			GetComponent<Collider> ().isTrigger = true;
		}
		if (GetComponent <Renderer> ()) {
			GetComponent <Renderer> ().enabled = false;
		}
		cRespawnTime = respawnTime;
	}
	
	protected override void Spawn ()
	{
		if (GetComponent <Collider> ()) {
			GetComponent<Collider> ().isTrigger = false;
		}
		if (GetComponent <Renderer> ()) {
			GetComponent <Renderer> ().enabled = true;
		}
		cRespawnTime = 0f;
		Unassign ();
	}
}
