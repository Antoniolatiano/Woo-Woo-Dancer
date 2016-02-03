using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Threading;

public class GameManager : MonoBehaviour {

	// Use this for initialization
	private bool inMainMenu=true;
	private bool traslateCameraToGame = false;
	public bool inPause = false;
	public bool fadeToBlack=false;
	private GameObject cosePausa;
	private GameObject coseMenu;
	private Camera camera;
	Vector3 cameraOriginal;
	SpriteRenderer blackForeground;
	private Text text;
    private Text sequence;
	private Text loseWin;

    Director dir; 
	void Start () {
		inMainMenu = true;
		Time.timeScale=0;
		coseMenu = GameObject.Find ("CoseMenu");
		cosePausa = GameObject.Find ("CosePausa");
		camera = GameObject.Find ("Main Camera").GetComponent<Camera>();
		blackForeground = GameObject.Find ("black").GetComponent<SpriteRenderer>();
		text = GameObject.Find ("score").GetComponent<Text> ();
        sequence = GameObject.Find("sequence").GetComponent<Text>();
		loseWin = GameObject.Find ("lose-win").GetComponent<Text> ();
        blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, 0);
		cameraOriginal = camera.transform.localPosition;
		camera.transform.Translate(new Vector2(0,90));
		coseMenu.SetActive (true);
		cosePausa.SetActive (false);
        GameObject.Find("sequence").SetActive(true);
        dir = GameObject.Find("DirectorObj").GetComponent<Director>(); 
	}

	// Update is called once per frame
	void Update () {
		if (!inMainMenu) {
			text.text = "score:"+(int)(MockSong.score*100);
            sequence.text = "X" + dir.sequence;
		}
		if (inMainMenu && (Input.GetKeyDown (KeyCode.A) || Input.GetButtonDown("Fire1"))) {
			traslateCameraToGame = true;
			coseMenu.SetActive (false);
		}
		if (traslateCameraToGame) {
			camera.transform.Translate(new Vector2(0,-0.5f));
			if (cameraOriginal.y >= camera.transform.localPosition.y) {
				traslateCameraToGame = false;
				inMainMenu = false;
				Time.timeScale = 1;
			}
		}
		if(!inMainMenu && (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))){
			if (!inPause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
			inPause = !inPause;
			cosePausa.SetActive (!cosePausa.activeSelf);				
		}
		if (fadeToBlack) {
			blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, blackForeground.color.a + 0.004f);
			loseWin.color = new Color (loseWin.color.r, loseWin.color.g, loseWin.color.b, loseWin.color.a - 0.004f);
			if(text.enabled)
				text.color=new Color (text.color.r, text.color.g, text.color.b, text.color.a - 0.004f);
			if (loseWin.color.a <= 0f)
            {
                Application.LoadLevel(Application.loadedLevel);
				MockSong.score = 0; 
            }
		}
	}

	public void handleLose(){
		//Time.timeScale = 0.0000001f;
		text.enabled = false;
		blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, 0.4f);//mostro l'immagine black
		loseWin.text = "YOU LOSE!!!!";//mostro la scritta "You Lose!!"
		//riproduzione della melodia di sconfitta
		StartCoroutine("blockAnimations");

	}

	public void handleWin(){
		//Time.timeScale = 0.0000001f;
		Debug.Log("ho vinto");
		blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, 0.4f);//mostro l'immagine black
		loseWin.text = "YOU WIN! score: " + MockSong.score;//mostro la scritta "You Win!!!" in alto al centro
		text.color=loseWin.color;
		text.transform.localPosition-=new Vector3(280,15,0);//il testo dello score si sposta sotto la scritta win
        text.enabled = false; 
		//attendo un tot di tempo, poi mando in fade sia le scritte che lo sfondo
		StartCoroutine("blockAnimations");
	}
		
	public IEnumerator blockAnimations(){
		MockSong[] buttons = GameObject.Find ("DirectorObj").GetComponentsInChildren<MockSong> ();
		foreach (MockSong button in buttons) {
			button.quarter = 1000000000000;
		}
        GameObject.Find("sequence").SetActive(false); 
		LeiScript lei = GameObject.Find ("Lei_0").GetComponent<LeiScript> ();
		lei.moveSize = 0;
		Time.timeScale = 0.1f;
		float pauseEndtime = Time.realtimeSinceStartup + 2;
		while(Time.realtimeSinceStartup < pauseEndtime){
			yield return 0;
		}
		//attendo un tot tempo, poi mando in fade sia la scritta che lo sfondo
		//yield return new WaitForSeconds (3 * Time.timeScale);
		fadeToBlack = true;
		Time.timeScale = 0;
	}

}
