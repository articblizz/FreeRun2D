using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float Speed = 5;

    public float JumpForce = 30;
    public float HangJumpForce = 30;

    public bool IsImmortal = false;

    public LayerMask whatIsGround;

    public Transform groundCheck;

    public FreerunScript freeScript;

    public bool isOnGround;
    public float LethalVelocity = -20;
    bool shouldDie = false;

    public bool disableInput = false;

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

        float direction = 0;

        if (!disableInput)
        {
            direction = Input.GetAxis("Horizontal");
        }

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
        else if(!disableInput && isOnGround)
        {
            rigidBody2D.velocity = new Vector2(direction * Speed, rigidBody2D.velocity.y);
        }
    }

	
	void Update () {

        if (rigidBody2D.velocity.y < LethalVelocity && !shouldDie && !IsImmortal)
        {
            shouldDie = true;
            rigidBody2D.fixedAngle = false;
            rigidBody2D.AddTorque(10 * freeScript.dir);
        }


        if (!disableInput)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isOnGround)
                {
                    Jump(JumpForce);
                }
                else if (freeScript.IsHanging)
                {
                    freeScript.IsHanging = false;

                    Jump(new Vector2(10 * freeScript.dir, HangJumpForce));
                }
            }

            if (freeScript.IsHanging)
            {
                if (Input.GetKey(KeyCode.S))
                {
                    freeScript.IsHanging = false;
                }
            }
        }

	}

    void OnCollisionEnter2D(Collision2D col)
    {
        if (shouldDie && col.collider.tag == "Roof")
            disableInput = true;
    }

    public void Jump(float forceY, float forceX = 0)
    {
        rigidBody2D.AddForce(new Vector2(forceX, forceY * 10));
    }

    public void Jump(Vector2 force)
    {
        rigidBody2D.AddForce(force * 10);
    }
}
