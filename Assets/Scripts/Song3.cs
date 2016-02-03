using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#

public class Song3Old : MonoBehaviour
{
    AudioSource as_bass;
    float counter = 0;
    public float delay = 0.75f;
    public float threshold = 0.5f;
    public float[] beatsthresholds = { };
    public int thresholdIndex = 0;
    public static float score = 0;
    public float tolerance = 0.3f;
    public string buttonName;
    float delayCounter = 0;
    GameObject dir;
    ButtonAlphaControl button;
    bool buttonPressed = false;
    bool started = false;
    int frameCounter;


    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    bool playerIndexSet = false;
    float prevCounter;


    bool alreadyPressed = false; 

    // Use this for initialization
    void Start()
    {
        as_bass = GetComponent<AudioSource>();
        dir = GameObject.Find("DirectorObj");
        button = GetComponentInChildren<ButtonAlphaControl>();
    }

    // Update is called once per frame
    void Update()
    {

        frameCounter++; 
        // Find a PlayerIndex, for a single player game
        // Will find the first controller that is connected ans use it
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);
                if (testState.IsConnected)
                {
                    Debug.Log(string.Format("GamePad found {0}", testPlayerIndex));
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }
        prevState = state;
        state = GamePad.GetState(playerIndex);
        prevCounter = counter;
        if (delayCounter <= delay)
        {
            delayCounter += Time.deltaTime; 
        }
        else
        {
            counter += Time.deltaTime;
            if (counter >= beatsthresholds[thresholdIndex])
            {
                as_bass.Play();
                if (!started)
                    started = true; 
                dir.GetComponent<Director>().Beat();
                button.AlphaMax();
                thresholdIndex++;
                if (thresholdIndex >= beatsthresholds.Length)
                    thresholdIndex = 0;

                counter = 0;
                if (!buttonPressed)
                {
                    
                    dir.GetComponent<Director>().sequence -= 10;
                    if (dir.GetComponent<Director>().sequence < 10)
                        dir.GetComponent<Director>().sequence = 0; 

                }
                else
                {
                    if (counter >= tolerance )
                        buttonPressed = false;
                    if (counter <= beatsthresholds[thresholdIndex] - tolerance)
                        buttonPressed = false;
                }
                 
                return;
            }

            if ( !buttonPressed && !alreadyPressed && 
                ( (state.Buttons.A == ButtonState.Pressed && buttonName.Equals("A")) 
                || (state.Buttons.B == ButtonState.Pressed && buttonName.Equals("B")) 
                || (state.Buttons.X == ButtonState.Pressed && buttonName.Equals("X"))
                || (state.Buttons.Y == ButtonState.Pressed && buttonName.Equals("Y")))) 
            {
                if (0<=counter && counter <= tolerance )
                {
                    alreadyPressed = true;
                    score += 10 - (counter / tolerance) * 10;
                    dir.GetComponent<Director>().BeatMain();
                    dir.GetComponent<Director>().sequence++;
                    //Debug.Log(Song3.score);
                    buttonPressed = true;
                }
                
                else //ERRORE DI TEMPO
                {
                    DecreaseSequence();
                    buttonPressed = true;
                    Debug.Log("ERRORE DI TEMPO");
                }

            }

            if (counter > tolerance)
            {
                alreadyPressed = false;

            }
            Debug.Log(alreadyPressed);
        }
        }

    void DecreaseSequence()
    {
        dir.GetComponent<Director>().sequence -= 10;
        if (dir.GetComponent<Director>().sequence < 10)
            dir.GetComponent<Director>().sequence = 0;
    }

    }


