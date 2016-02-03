using UnityEngine;
using System.Collections;

public class ButtonAlphaControl : MonoBehaviour {

	private float alpha = 0.4f;
	private SpriteRenderer render;
	// Use this for initialization
	void Start () {
		render = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (alpha > 0.4f) {
			alpha -= 0.02f;
		}
		render.color = new Color (render.color.r, render.color.g, render.color.b, alpha);
	}

	public void AlphaMax(){
		alpha = 1.0f;
	}
}
