using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Patrol : MonoBehaviour {

    public bool facingRight = true;
    public Rigidbody2D rigidBody2D;
    public SpriteRenderer spriteRenderer;

    public Vector2 speed = new Vector2(1, 0);
    public float speedFactor = 0.25F;
    public Vector2 direction = new Vector2(1, 0);

    public void Awake()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if(col.gameObject.tag == "Border") {
            //Debug.Log("LOOP BACK");
            direction = Vector2.Scale(direction, new Vector2(-1, 0));
            if (!facingRight)
            {
                //Debug.Log("Flip1");
                Flip();
            }
            else if (facingRight)
            {
                //Debug.Log("Flip2");
                Flip();
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 movement = new Vector2(speed.x * direction.x * speedFactor, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        GetComponent<SpriteRenderer>().flipX = true;
    }
}