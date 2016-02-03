using UnityEngine;
using System.Collections;

public class Song2 : MonoBehaviour
{
    AudioSource as_bass;
    float counter = 0;
    public float delay = 0.75f;
    public float threshold = 0.5f;
    public static float score = 0;
    public float tolerance = 0.3f;
    public string buttonName = "Jump";
    float delayCounter = 0;
    GameObject dir;
    bool buttonPressed = false;

    // Use this for initialization
    void Start()
    {
        as_bass = GetComponent<AudioSource>();
        dir = GameObject.Find("DirectorObj"); 
    }

    // Update is called once per frame
    void Update()
    {
        if (delayCounter <= delay)
            delayCounter += Time.deltaTime;
        else
        {
            counter += Time.deltaTime;
            if (counter >= threshold)
            {
                as_bass.Play();
                dir.GetComponent<Director>().Beat();
                counter = 0;
                if (!buttonPressed) {
                    dir.GetComponent<Director>().sequence = 0;
                }
                else
                {
                    if(counter>=tolerance || counter <= threshold - tolerance)
                        buttonPressed = false;
                }
                return;
            }

            if (Input.GetButtonDown(buttonName))
            {
                if (counter <= tolerance)
                {
                    score += 1;
                    Debug.Log(score);
                    dir.GetComponent<Director>().sequence++;
                    buttonPressed = true;
                    dir.GetComponent<Director>().BeatMain(); 
                }
                else
                {
                    dir.GetComponent<Director>().sequence = 0; 
                }
            }

        }
    }
}

