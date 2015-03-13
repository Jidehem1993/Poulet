using UnityEngine;
using System.Collections;

public class CheckpointPlatform : StaticPlatform {

	bool used;

	void Start ()
	{
		used = false;
		AddAudioSource ();
	}

	public void Use ()
	{
		used = true;
		muteLand = true;
	}

	public bool IsUsed ()
	{
		return used;
	}
}
