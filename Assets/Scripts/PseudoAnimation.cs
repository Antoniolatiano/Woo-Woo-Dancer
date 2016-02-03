using UnityEngine;
using System.Collections;

public class PseudoAnimation : MonoBehaviour {
    public Sprite[] sprites;
    SpriteRenderer sprenderer;
    int currentPos; 
	// Use this for initialization
	void Start () {
        sprenderer = GetComponent<SpriteRenderer>(); 
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    void Animate()
    {
        sprenderer.sprite = sprites[currentPos];
        currentPos++;
        if (currentPos >= sprites.Length)
            currentPos = 0; 
    }
}
