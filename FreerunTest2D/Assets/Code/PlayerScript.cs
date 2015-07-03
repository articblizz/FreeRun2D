using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {

    public float TargetSpeed = 5;
    float spdRight, spdLeft;
    public float Acceleration = 0.1f;

    public float JumpForce = 30;
    public float HangJumpForce = 30;

    public float groundCheckRadious = 0.2f;

    public bool IsImmortal = false;

    public LayerMask whatIsGround;

    public Transform groundCheck;

    public FreerunScript freeScript;

    public bool isOnGround;
    public float LethalVelocity = -20;
    bool shouldDie = false;

    public bool disableInput = false;

    Animator animator;
    public float MaxVeloY = 5;
    Rigidbody2D rigidBody2D;

    void Start () {

        rigidBody2D = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        freeScript = GetComponent<FreerunScript>();

        
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadious);
    }

    void FixedUpdate()
    {
        isOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadious, whatIsGround);
        //animator.SetBool("IsHanging", !isOnGround);
        //animator.SetBool("IsHanging", freeScript.IsHanging);
        

        float direction = 0;

        if (!disableInput)
        {
            direction = Input.GetAxisRaw("Horizontal");
        }

        animator.SetBool("IsRunning", direction != 0);

        if (!freeScript.IsHanging)
        {
            if (direction < 0 && !freeScript.isFacingRight)
            {
                freeScript.Flip();
            }
            else if (direction > 0 && freeScript.isFacingRight)
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
            if (direction > 0 && TargetSpeed < 0)
                TargetSpeed *= -1;
            else if (direction < 0 && TargetSpeed > 0)
                TargetSpeed *= -1;
            var speed = rigidBody2D.velocity.x;

            if (direction > 0 && speed < TargetSpeed)
            {
                speed += Acceleration;
            }
            else if (direction < 0 && speed > TargetSpeed)
            {
                speed -= Acceleration;
            }
            else if (direction == 0)
            {
                if (speed < 0)
                {
                    speed += Acceleration;
                    if (speed > 0)
                        speed = 0;
                }
                else
                {
                    speed -= Acceleration;
                    if (speed < 0)
                        speed = 0;
                }
            }
            rigidBody2D.velocity = new Vector2(speed, rigidBody2D.velocity.y);
            //rigidBody2D.velocity = new Vector2(direction * TargetSpeed, rigidBody2D.velocity.y);

        }
        else if (!isOnGround && !freeScript.isWithinWalljump)
        {
            var velo = rigidBody2D.velocity;
            velo.x += (direction * (Acceleration / 10));
            rigidBody2D.velocity = velo;
        }

        if (rigidBody2D.velocity.y > MaxVeloY)
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, MaxVeloY);
        animator.SetFloat("ms", Mathf.Abs(rigidBody2D.velocity.x));
        animator.SetBool("groundTouch", isOnGround);
    }

    
    void Update () {

        if (rigidBody2D.velocity.y < LethalVelocity && !shouldDie && !IsImmortal)
        {
            shouldDie = true;
            rigidBody2D.constraints = RigidbodyConstraints2D.None;
            rigidBody2D.AddTorque(10 * -freeScript.dir);
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
                    //animator.SetTrigger("ClimbEdge");
                    Jump(new Vector2((HangJumpForce/10) * (freeScript.dir * -1),HangJumpForce));
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
        if (shouldDie && (col.collider.tag == "Roof" || col.collider.tag == "Ground"))
        {
            rigidBody2D.drag = 3f;
            disableInput = true;
        }
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
