using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float Speed = 5;

    public float JumpForce = 30;

    public LayerMask whatIsGround;

    public Transform groundCheck;

    public FreerunScript freeScript;

    bool isOnGround;

    Rigidbody2D rigidBody2D;

	void Start () {

        rigidBody2D = transform.parent.GetComponent<Rigidbody2D>();
	
	}

    void FixedUpdate()
    {
        float direction = Input.GetAxis("Horizontal");

        if (freeScript.IsHanging)
        {
            rigidBody2D.velocity = Vector2.zero;
        }
        else
        {
            rigidBody2D.velocity = new Vector2(direction * Speed, rigidBody2D.velocity.y); 
        }
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, whatIsGround);
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                Jump();
            }
            else if(freeScript.IsHanging)
            {
                freeScript.IsHanging = false;

                Jump();
            }
        }
	}

    void Jump()
    {
        rigidBody2D.AddForce(new Vector2(0, JumpForce * 10));
    }
}
