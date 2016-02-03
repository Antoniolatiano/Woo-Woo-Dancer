using UnityEngine;
using System.Collections;
using System;
using XInputDotNetPure; // Required in C#

public class MockSong : MonoBehaviour
{
    AudioSource as_bass;
    public float counter = 0;
    public float delay = 0.75f;
    public float[] beatsthresholds;
    public int thresholdIndex = 0;
    public static float score = 0;
    public string buttonName = "Jump";
    public float delayCounter = 0;
    GameObject dir;
    ButtonAlphaControl button;
    public bool want = false;
    public string valutazione;
    Director director;
    float availableTime = 0; 
    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    bool playerIndexSet = false;
    public float upperTolerance = 0.1f;
    public float underTolerance = 0.1f;
    public float quarter = 0.437f; 

    // Use this for initialization
    void Start()
    {
        as_bass = GetComponent<AudioSource>();
        dir = GameObject.Find("DirectorObj");
        director = dir.GetComponent<Director>();
        button = GetComponentInChildren<ButtonAlphaControl>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (delayCounter <= delay * quarter)
            delayCounter += Time.deltaTime;
        else
        {
            counter += Time.deltaTime;
            availableTime -= Time.deltaTime;
            float thr = beatsthresholds[thresholdIndex];
            if (counter >= (thr * quarter) - underTolerance && counter < (thr * quarter) && availableTime <= 0)
            {
                want = true;
                availableTime = (thr * quarter) + upperTolerance - counter;
                button.AlphaMax();
            }


            if (counter >= thr * quarter)
            {
                as_bass.Play();
                dir.GetComponent<Director>().Beat();
                //ottengo il tempo in millisecondi riferito all'inizio della riproduzione del suono

                thresholdIndex++;
                if (thresholdIndex >= beatsthresholds.Length)
                    thresholdIndex = 0;
                Debug.Log("dopo " + counter);
                counter -= thr * quarter;
                return;
            }

            if (want)
            {
                if (Input.GetButtonDown(buttonName))
                {
                    Reward(availableTime);
                    want = false;
                }
                else
                {
                    if (availableTime > 0)
                        return;
                    else
                    {
                        DecreaseSequence();
                        want = false;
                    }
                }

            }
            else
                if(Input.GetButtonDown(buttonName))
                DecreaseSequence();
        }

    }

    void DecreaseSequence()
    {
        if (director.sequence < 10)
            director.sequence -= 1;
        if(director.sequence<=20 && director.sequence >= 10) { 
            director.sequence -= 3;
        }
        if (director.sequence > 20)
        {
            director.sequence -= 10; 
        }
        if (director.sequence < 0)
            director.sequence = 0;
    }

    void Reward(float availableTime)
    {
        float increase = 1000 * availableTime;
        //DO STUFF
        if (director.sequence >= 10 && director.sequence<20)
            increase *= 1.5f;
        if (director.sequence >= 20 && director.sequence<50)
            increase *= 3f;
        if (director.sequence >= 50)
            increase *= 7f; 
        score += increase;
        dir.GetComponent<Director>().BeatMain();
        director.sequence++;
    }

}

