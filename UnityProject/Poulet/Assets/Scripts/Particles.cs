using UnityEngine;
using System.Collections;

public class Particles : MonoBehaviour
{
	
	bool colorLerping;
	ParticleSystem par;
	Renderer ren;
	public float colorSpeed;

	void Start ()
	{ 
		par = GetComponent <ParticleSystem> ();
		if (par) {
			ren = par.GetComponent <Renderer> ();
		}
	}

	void Update ()
	{
		if (colorLerping) {
			ColorLerp ();
		}
	}
	
	void ColorLerp ()
	{
		float cR;
		float cG;
		float cB;
		float rSpeed;

		if (ren) {
			cR = ren.material.GetColor ("_TintColor").r;
			cG = ren.material.GetColor ("_TintColor").g;
			cB = ren.material.GetColor ("_TintColor").b;
			rSpeed = Time.deltaTime * colorSpeed;
			if (cR == 1f) {
				if (cG == 0f) {
					if (cB < 1f) {
						cB += rSpeed;
					} else {
						cR -= rSpeed;
					}
				} else {
					cG -= rSpeed;
				}
			} else if (cG == 1f) {
				if (cR < 1f) {
					if (cB > 0f) {
						cB -= rSpeed;
					} else {
						cR += rSpeed;
					}
				} else {
					cG -= rSpeed;
				}
			} else if (cB == 1f) {
				if (cR > 0f) {
					cR -= rSpeed;
				} else {
					cG += rSpeed;
				}
			}
			if (cR > 1f) {
				cR = 1f;
			} else if (cR < 0f) {
				cR = 0f;
			}
			if (cG > 1f) {
				cG = 1f;
			} else if (cG < 0f) {
				cG = 0f;
			}
			if (cB > 1f) {
				cB = 1f;
			} else if (cB < 0f) {
				cB = 0f;
			}
			ren.material.SetColor ("_TintColor", new Color (cR, cG, cB));
		}
	}
	
	public void Play ()
	{
		if (par) {
			par.Play ();
		}
	}
	
	public void Stop ()
	{
		if (par) {
			par.Stop ();
		}
	}
	
	public void SetColorLerping (bool state)
	{
		colorLerping = state;
		if (colorLerping && ren) {
			ren.material.SetColor ("_TintColor", new Color (1f, 0f, 0f));
		}
	}
}