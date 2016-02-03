using UnityEngine;
using System.Collections;

public class LeiScript : MonoBehaviour {
    public float moveSize = 3;
    GameObject director;
	GameManager gameManager;
    float x; 
	float sogliaAvversario=76;
	float sogliaPlayer=12;

	// Use this for initialization
	void Start () {
        x = transform.localScale.x; 
        director = GameObject.Find("DirectorObj"); 
		gameManager = GameObject.Find ("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (director.GetComponent<Director>().sequence > director.GetComponent<Director>().sogliaBonus1)
        {

            transform.position += new Vector3(-moveSize * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(-x, transform.localScale.y, transform.localScale.z);           
        }
        if (director.GetComponent<Director>().sequence<=director.GetComponent<Director>().sogliaBonus1)
        {
            transform.position -= new Vector3(-moveSize * Time.deltaTime, 0, 0);
            transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);           
        }
		//se lei è arrivata nelle braccia dell'avversario hai perso 
		if (transform.position.x >= sogliaAvversario)
			gameManager.handleLose ();
		if (transform.position.x <= sogliaPlayer)
			gameManager.handleWin ();			
	}
}
