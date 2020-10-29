using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{

	public CharacterController2D controller;

	public Health hObj;
    public Score sObj;
    public Timer tObj;
    public Menu mObj;

    public NextLevel lObj;
    public SaveGame sgObj;
    public ResetLevel rObj;

	SpriteRenderer sr; //
    Color srOrigColor; //

	public float runSpeed = 300;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool gotHurt = false;
    public bool isDead = false;
    bool gotHealth = false;
    public bool advanceLevel = false;
    public bool isNewGame = true;

    public Vector3 levelRespawn;


    SpriteRenderer[] sprites;

    void Awake() {
        //mObj = GameObject.Find("MenuPause").GetComponent<Menu>();
        sgObj = GameObject.Find("God").GetComponent<SaveGame>();
        rObj = GameObject.Find("God").GetComponent<ResetLevel>();
        if (isNewGame) {
            levelRespawn = this.transform.position;
            sgObj.spawnPoint1 = this.transform.position;
            isNewGame = false;
        } else {
            this.transform.position = sgObj.spawnPoint1;
        }
    }

	void Start() {
        
        sObj = GetComponent<Score>();
		hObj = GetComponent<Health>();
        tObj = GetComponent<Timer>();
        //mObj = GameObject.Find("MenuPause").GetComponent<Menu>();
        //rObj = GetComponent<RespawnObjects>();
        lObj = GameObject.Find("Chest").GetComponent<NextLevel>();

		sr = GetComponent<SpriteRenderer>();

        srOrigColor = sr.color;

        sprites = GetComponentsInChildren<SpriteRenderer>();

        /*for (int i=0; i < spriteRenderers.Count; ++i) {
            colors.Add(spriteRenderers.material.color);
            renderer.material.color = new Color(1,1,1,0.5f);
        }*/
	}

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;

        /*if(Input.GetKey(KeyCode.Escape)) {
            //Debug.Log("saving game");
            mObj.OpenMenu();
        }*/

        if (Input.GetButtonDown("Jump")) 
        {
        	jump = true;
        	//Debug.Log("jumping");
        }

        if (Input.GetButtonDown("Crouch")) 
        {
        	crouch = true;
        	//Debug.Log("crouch down");
        } else if (Input.GetButtonUp("Crouch")) {
        	crouch = false;
        	//Debug.Log("crouch up");
        }

        if (gotHurt) {

            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color(1,1,1,0.5f);
            }
            
            //sr.color = new Color(1,1,1,0.5f);
        	/*for (int i=0; i < spriteRenderers.Count; ++i) {
            
                renderer.material.color = colors[i];
            }*/

            StartCoroutine("FadeBack");
            //Debug.Log("Start Coroutine");
             // coroutine
        	//gotHurt = false;
        }

        if (gotHealth) {
            //sr.color = new Color(0,1,0);
            //sr.color = Color.white;
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color (254/255f, 215/255f, 0f, 1f);
            }
            //sr.color = new Color (254/255f, 215/255f, 0f, 1f);
            //sr.color = new Color(1f, 0.92f, 0.016f, 1f);
            StartCoroutine("FadeBack");
            //Debug.Log("Start Coroutine");
            
        }

        // Respawn to SAVE POINT if death conditions are met
        if (isDead) {
            

            this.transform.position = sgObj.spawnPoint1;

            rObj.Reset();

            
            /*if (saved) {
                this.transform.position = spawnPoint1;
            } else {
                this.transform.position = levelRespawn; // move player back to last save point
            }*/
            //Application.LoadLevel(Application.loadedLevel);
            //rObj.Respawn();
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            isDead = false;
        }

        if (advanceLevel) {
            lObj.LoadNextScene();
        }

    }

    IEnumerator FadeBack() {
        if (gotHealth) {
            yield return new WaitForSeconds(0.5f);
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = srOrigColor;
            }
            gotHealth = false;
        }

        if (gotHurt) {
            yield return new WaitForSeconds(0.5f);
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = srOrigColor;
            }
            //sr.color = srOrigColor;
            gotHurt = false;
        }
        //sr.color = new Color(0,1,0);
        
        //Debug.Log("Finish Coroutine");
    }

    void FixedUpdate () 
    {
    	// Move our character
    	controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
    	jump = false;

        // Respawn to beginning of level when health is 0
        if (hObj.health == 0) {

            // continue timer when player's health runs out
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);

            isDead = true;

            // for CHECKPOINTS (possibly)
            //this.transform.position = spawnPoint1.transform.position;
            //hObj.health = 5; // reset health
            //reset level

        }
    	
    }

    // Player triggers with colliders

    void OnTriggerEnter2D(Collider2D col) {

    	if(col.gameObject.tag == "Coins") {
			//Debug.Log("got coin");

            // Add points to player score
            sObj.AddPoints(5);
            col.gameObject.SetActive(false);
			//Destroy(col.gameObject); // destroy coin
		} 


		// When player gets hurt

		else if (col.gameObject.tag == "Enemy") {

			// Health decrease by 1
			hObj.TakeDamage(1);

			// Change player's color
			gotHurt = true;


			//Debug.Log("got spike hurt");
		}

        // When player gets health

        else if (col.gameObject.tag == "Health") {

            // Health decrease by 1
            hObj.AddHealth();
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            gotHealth = true;

            //Debug.Log("got healthpack");
        }

        // When player dies
        else if (col.gameObject.tag == "DeathZone") {
            // Respawn player to beg of level

            //Debug.Log("player has died");
            // continue timer when player dies
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            isDead = true;

            //this.transform.position = spawnPoint1.transform.position;
        }

        // When player reaches end of level
        else if (col.gameObject.tag == "Finish") {
           // isNewGame = false;
            //Debug.Log("Setting score");
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining);
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            PlayerPrefs.SetInt("Player Score", sObj.score);
            //Debug.Log("Score: " + sObj.score.ToString());
            //Debug.Log("Setting health");
            PlayerPrefs.SetInt("Player Health", hObj.health);
            //Debug.Log("Health: " + hObj.health.ToString());
            //Debug.Log("finished setting");

            isNewGame = false;
            //Debug.Log("not a new game");
            advanceLevel = true;
           // Debug.Log("next level");
            //Debug.Log("isNewGame 1: " + isNewGame.ToString());

            //lObj.LoadNextScene();

            //advanceLevel = true; 
            
            
            //Debug.Log("End of level");
            
        }
    }

    // Player collides with other colliders
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            // Health decrease by 1
            hObj.TakeDamage(1);

            // Change player's color
            gotHurt = true;


            //Debug.Log("got HIT");
        }
    }

    public void OnApplicationQuit(){
         //spawnPoint1 = levelRespawn;

         //Debug.Log("Reset health");
    }


}
