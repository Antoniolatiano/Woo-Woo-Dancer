using UnityEngine;
using XInputDotNetPure; // Required in C#
using System.Collections;

public class Director : MonoBehaviour {
    GameObject[] characters;
    GameObject mainCharacter; 
    public float sequence = 0;
    MockSong[] songs;
    public int sogliaBonus1 = 15;
    public float counter = 0;
    public LeiScript lei; 

    public float ourDelay; 
    // Use this for initialization
    void Start () {
        mainCharacter = GameObject.FindGameObjectWithTag("MainCharacters");
        characters = GameObject.FindGameObjectsWithTag("Player"); 
        songs =(MockSong[]) GetComponentsInChildren<MockSong>();
        lei = GameObject.Find("Lei_0").GetComponent<LeiScript>();
        ourDelay = 60.5f * songs[0].quarter;
        Initialize();
    }

    void Initialize()
    {
        songs[0].delay = 0f;
        songs[0].buttonName = "Fire1";
        songs[0].beatsthresholds = new float[] { 4f };  //, 0.4f, 0.9f, 0.1f }


        songs[1].delay = 17.5f;
        songs[1].buttonName = "Fire2";
        songs[1].beatsthresholds = new float[] { 4f };

        songs[2].delay = 35f;
        songs[2].buttonName = "Fire3";
        songs[2].beatsthresholds = new float[] { 4f };

        songs[3].delay = 50f;
        songs[3].buttonName = "Jump";
        songs[3].beatsthresholds = new float[] { 2.5f, 1.5f, 2.5f, 1.5f, 2.5f};
    }
	
	// Update is called once per frame
	void Update () {
        counter += Time.deltaTime;
        
        if (counter > (ourDelay) )
        {
            lei.moveSize += 0.3f; 
            Debug.Log("eccolo");
            foreach (MockSong s in songs)
            {

                if (s.quarter > 0.35)
                    s.quarter *= 0.95f;
                s.counter = 0; 
                s.delayCounter = 0;
                s.thresholdIndex = 0;
            }
            counter = counter - songs[3].delay* songs[0].quarter -8 * songs[0].quarter; 
        }
	}
    public void Beat()
    {
        foreach(GameObject go in characters)
        {
            go.SendMessage("Animate"); 
        }
    }

    public void BeatMain()
    {
        mainCharacter.SendMessage("Animate");
    }
}
