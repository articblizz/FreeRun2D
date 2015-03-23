using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float Speed = 5;

    public float JumpForce = 30;
    public float HangJumpForce = 30;

    public LayerMask whatIsGround;

    public Transform groundCheck;

    public FreerunScript freeScript;

    bool isOnGround;

    Animator animator;

    Rigidbody2D rigidBody2D;

	void Start () {

        rigidBody2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        freeScript = GetComponent<FreerunScript>();
	}

    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, 0.03f, whatIsGround);

        //animator.SetBool("IsHanging", !isOnGround);
        animator.SetBool("IsHanging", freeScript.IsHanging);

        float direction = Input.GetAxis("Horizontal");

        if (!freeScript.IsHanging)
        {
            if (direction > 0 && !freeScript.isFacingRight)
            {
                freeScript.Flip();
            }
            else if (direction < 0 && freeScript.isFacingRight)
            {
                freeScript.Flip();
            }
        }

        if (freeScript.IsHanging)
        {
            rigidBody2D.velocity = Vector2.zero;
        }
        else
        {
            rigidBody2D.velocity = new Vector2(direction * Speed, rigidBody2D.velocity.y); 
        }
    }
	
	void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isOnGround)
            {
                Jump(JumpForce);
            }
            else if(freeScript.IsHanging)
            {
                freeScript.IsHanging = false;

                Jump(HangJumpForce);
            }
        }

	}

    void Jump(float force)
    {
        rigidBody2D.AddForce(new Vector2(0, force * 10));
    }
}
