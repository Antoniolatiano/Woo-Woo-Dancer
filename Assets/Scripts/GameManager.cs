using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class GameManagerOld : MonoBehaviour {

	// Use this for initialization
	private bool inMainMenu=true;
	private bool traslateCameraToGame = false;
	public bool inPause = false;
	private bool fadeToBlack=false;
	private GameObject cosePausa;
	private GameObject coseMenu;
	private Camera camera;
	Vector3 cameraOriginal;
	SpriteRenderer blackForeground;
	private Text text;
    private Text sequence;

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
        blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, 0);
		cameraOriginal = camera.transform.localPosition;
		camera.transform.Translate(new Vector2(0,90));
		coseMenu.SetActive (true);
		cosePausa.SetActive (false);
        dir = GameObject.Find("DirectorObj").GetComponent<Director>(); 
	}

	// Update is called once per frame
	void Update () {
		if (!inMainMenu) {
			text.text = "score:"+(int)(MockSong.score);
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
		if(!inMainMenu && Input.GetKeyDown(KeyCode.Escape)){
			if (!inPause)
				Time.timeScale = 0;
			else
				Time.timeScale = 1;
			inPause = !inPause;
			cosePausa.SetActive (!cosePausa.activeSelf);				
		}
		if (fadeToBlack) {
			blackForeground.color = new Color (blackForeground.color.r, blackForeground.color.g, blackForeground.color.b, blackForeground.color.a + 0.009f);
            if (blackForeground.color.a >= 1.0f)
            {
                Application.LoadLevel(Application.loadedLevel);
                MockSong.score = 0; 
            }
		}
	}

	public void handleLose(){
		Time.timeScale = 0;
		fadeToBlack = true;
	}

	public void handleWin(){
		Time.timeScale = 0;
		fadeToBlack = true;
	}

    public static long ToUnixTime(DateTime date)
    {
        var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return Convert.ToInt64((date - epoch).TotalMilliseconds);
    }
}
