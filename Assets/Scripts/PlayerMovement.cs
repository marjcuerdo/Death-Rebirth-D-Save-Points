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

	SpriteRenderer sr; 
    Color srOrigColor; 

	public float runSpeed = 300;

	float horizontalMove = 0f;
	bool jump = false;
	bool crouch = false;
	bool gotHurt = false;
    public bool isDead = false;
    bool gotHealth = false;
    public bool advanceLevel = false;
    public bool isNewGame = true;
    public bool lvlSavePointExists = false;

    public Vector3 levelRespawn;


    SpriteRenderer[] sprites;

    void Awake() {
        sgObj = GameObject.Find("God").GetComponent<SaveGame>();
        rObj = GameObject.Find("God").GetComponent<ResetLevel>();

        // if game is new then use original respawn point when player dies
        if (isNewGame) {
            levelRespawn = this.transform.position;
            sgObj.spawnPoint1 = this.transform.position;
            isNewGame = false;
        } else if (lvlSavePointExists) {

            // only if save exists for level
            // if a save exists, put player in last saved position
                this.transform.position = sgObj.spawnPoint1;
        } else {
            levelRespawn = this.transform.position;
            //sgObj.spawnPoint1 = this.transform.position;
            isNewGame = false;
        }
    }

	void Start() {       
        sgObj = GameObject.Find("God").GetComponent<SaveGame>();
        rObj = GameObject.Find("God").GetComponent<ResetLevel>();
        sObj = GetComponent<Score>();
		hObj = GetComponent<Health>();
        tObj = GetComponent<Timer>();
        lObj = GameObject.Find("Chest").GetComponent<NextLevel>();

		sr = GetComponent<SpriteRenderer>();
        srOrigColor = sr.color;
        sprites = GetComponentsInChildren<SpriteRenderer>(); // get colors for sprites

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
            // if hurt, flash
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color(1,1,1,0.5f);
            }
            
            //sr.color = new Color(1,1,1,0.5f);
        	/*for (int i=0; i < spriteRenderers.Count; ++i) {
            
                renderer.material.color = colors[i];
            }*/

            // fadeback to original color
            StartCoroutine("FadeBack");
        }

        // change color when health pack
        if (gotHealth) {
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color (254/255f, 215/255f, 0f, 1f);
            }
            StartCoroutine("FadeBack");
        }

        // Respawn to SAVE POINT if death conditions are met
        if (isDead) {
            
            if (lvlSavePointExists) {
                this.transform.position = sgObj.spawnPoint1; 
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            rObj.Reset();

            isDead = false;
        }

        // if chest is reached, load next level
        if (advanceLevel) {
            // reset level respawn point
            rObj.restartedLevel = false;
            // remember that savepoint doesn't exist for the next level yet
            lvlSavePointExists = false;

            lObj.LoadNextScene();
        }

    }

    // briefly change player color
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
            gotHurt = false;
        }
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
        }
    }

    // Player triggers with colliders
    void OnTriggerEnter2D(Collider2D col) {

    	if(col.gameObject.tag == "Coins") {
			//Debug.Log("got coin");

            // Add points to player score
            sObj.AddPoints(5);
            col.gameObject.SetActive(false); // make coin inactive
		} 


		// When player gets hurt
		else if (col.gameObject.tag == "Enemy") {

			// Health decrease by 1
			hObj.TakeDamage(1);

			// Change player's color
			gotHurt = true;
		}

        // When player gets health
        else if (col.gameObject.tag == "Health") {

            // Health decrease by 1
            hObj.AddHealth();
            //Destroy(col.gameObject);
            col.gameObject.SetActive(false);
            gotHealth = true;
        }

        // When player dies
        else if (col.gameObject.tag == "DeathZone") {

            // continue timer when player dies
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            isDead = true; // triggers last saved point
        }

        // When player reaches end of level
        else if (col.gameObject.tag == "Finish") {

            // continue timer
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining);
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);

            // save score and health
            PlayerPrefs.SetInt("Player Score", sObj.score);
            PlayerPrefs.SetInt("Player Health", hObj.health);

            isNewGame = false;
            advanceLevel = true;
        }
    }

    // Player collides with other colliders
    void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            // Health decrease by 1
            hObj.TakeDamage(1);

            // Change player's color
            gotHurt = true;
        }
    }

}
