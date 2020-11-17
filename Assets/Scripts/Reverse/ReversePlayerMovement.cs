using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReversePlayerMovement : MonoBehaviour
{

    public ReverseCharacterController controller;

    public ReverseHealth hObj;
    public ReverseScore sObj;
    public ReverseTimer tObj;
    public Wind wObj;

    SpriteRenderer sr; //
    Color srOrigColor; //

    public GameObject spawnPoint1;

    public float runSpeed = 300;

    float horizontalMove = 0f;
    bool jump = false;
    //bool crouch = false;
    bool gotHurt = false;
    bool isDead = false;
    bool gotHealth = false;
    public bool advanceLevel = false;
    public bool isNewGame = true;
        public bool lvlSavePointExists = false;

    public Vector3 levelRespawn;

    SpriteRenderer[] sprites;

    public NextLevel lObj;

    public ReverseSaveGame sgObj;
    public ReverseResetLevel rObj;

    public AudioSource coinAudio;
    public AudioSource deathAudio;

    public Animator anim;

    public ReverseCharacterController cObj;
    

    void Awake() {
        sgObj = GameObject.Find("God").GetComponent<ReverseSaveGame>();
        rObj = GameObject.Find("God").GetComponent<ReverseResetLevel>();

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
        sgObj = GameObject.Find("God").GetComponent<ReverseSaveGame>();
        rObj = GameObject.Find("God").GetComponent<ReverseResetLevel>();
        cObj = GetComponent<ReverseCharacterController>();
        anim = GetComponent<Animator>();
        sObj = GetComponent<ReverseScore>();
        hObj = GetComponent<ReverseHealth>();
        tObj = GetComponent<ReverseTimer>();
        if (SceneManager.GetActiveScene().name == "Level5") {
            wObj = GetComponent<Wind>();
        }
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

        if (Input.GetButtonDown("Jump")) 
        {
            jump = true;
            //Debug.Log("jumping");

        } 

        if (horizontalMove == 0 && cObj.m_Grounded) {
            anim.SetBool("isRunning", false);
        } else if (horizontalMove > 0 && !cObj.m_Grounded){
            anim.SetBool("isRunning", false);
        } else {
            anim.SetBool("isRunning", true);
        }



        // for crouching
        /*if (Input.GetButtonDown("Crouch")) 
        {
            crouch = true;
            //Debug.Log("crouch down");
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
            //Debug.Log("crouch up");
        }*/

        if (gotHurt) {

            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color(1,1,1,0.5f);
            }

            StartCoroutine("FadeBack");
        }

        if (gotHealth) {
            for (int i=0; i <sprites.Length; i++) {
                sprites[i].color = new Color (254/255f, 215/255f, 0f, 1f);
            }
            StartCoroutine("FadeBack");         
        }

        // Restart level if death conditions are met
        if (isDead) {
            if (lvlSavePointExists) {
                this.transform.position = sgObj.spawnPoint1; 
            } else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }

            rObj.Reset();
            isDead = false;
        }

        // when chest/finish point is reached, load next level
        if (advanceLevel) {
            // reset level respawn point
            rObj.restartedLevel = false;
            // remember that savepoint doesn't exist for the next level yet
            lvlSavePointExists = false;

            lObj.LoadNextScene();
        }

        // if wind is blowing on Level 5
        if (SceneManager.GetActiveScene().name == "Level5") {
            if (wObj.windIsBlowing) {
                for (int i=0; i <sprites.Length; i++) {
                    sprites[i].color = new Color (0f, 0f, 255f/255f, 1f);
                }

                StartCoroutine("FadeBack");   
            }
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

        if (SceneManager.GetActiveScene().name == "Level5") {
            if (wObj.windIsBlowing) {

                yield return new WaitForSeconds(3f);
                for (int i=0; i <sprites.Length; i++) {
                    sprites[i].color = srOrigColor;
                }
                //sr.color = srOrigColor;
                //gotHurt = false;
            }
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
            sObj.AddPoints(10);
            coinAudio.Play();
            col.gameObject.SetActive(false);
            //Destroy(col.gameObject);
            //col.GetComponent<SpriteRenderer>().enabled = false;
        } 


        // When player gets hurt
        else if (col.gameObject.tag == "Enemy") {

            // Health decrease by 1
            hObj.TakeDamage(1);
            Debug.Log("HEALTH: " + hObj.health);
            hObj.tookDamage = true; /////

            // Change player's color
            gotHurt = true;


            //Debug.Log("got spike hurt");
        }

        // When player gets health pack
        else if (col.gameObject.tag == "Health") {


            // Health decrease by 1
            hObj.AddHealth();
            Debug.Log("HEALTH: " + hObj.health);

            // make health pack inactive
            col.gameObject.SetActive(false);

            gotHealth = true;
        }

        // When player dies
        else if (col.gameObject.tag == "DeathZone") {
            // Respawn player to beg of level

            // continue timer when player dies
            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining); 
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            deathAudio.Play();
            isDead = true;
        }

        // When player reaches end of level
        else if (col.gameObject.tag == "Finish") {

            PlayerPrefs.SetFloat("TimeRem", tObj.timeRemaining);
            PlayerPrefs.SetFloat("TimeInc", tObj.timeInc);
            PlayerPrefs.SetInt("Player Score", sObj.score);
            PlayerPrefs.SetInt("Player Health", hObj.health);
            PlayerPrefs.SetInt("Extra Hearts", hObj.currentExtraHearts);
            PlayerPrefs.SetInt("Took Damage", (hObj.tookDamage ? 1 : 0));

            if (SceneManager.GetActiveScene().name == "WinScreen") {
                PlayerPrefs.SetFloat("TimeRem", 300);
                PlayerPrefs.SetFloat("TimeInc", 0);
            }

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


            //Debug.Log("got HIT");
        }
    }


}
