using UnityEngine;
using System.Collections;

public class FreerunScript : MonoBehaviour {

    public bool IsHanging;

    public Rigidbody2D rigidBody2D;

    GameObject currentEdge;
    public GameObject hand;

    public float MaxVelocityYOnWall = -5;

    public bool isFacingRight = true;

    public Vector2 hangOffset = new Vector2(0.5f, 1);

    public Vector2 WallJumpForceUp = new Vector2(10, 30);
    public float WallJumpSideX = 35;
    public float WallJumpSideY = 25;
    public int dir = 1;

    public bool isWithinWalljump;

    PlayerScript plyScript;

    void Awake()
    {
    }

    void Start () {

        rigidBody2D = GetComponent<Rigidbody2D>();
        plyScript = GetComponent<PlayerScript>();

        Flip();
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        dir *= -1;
        //transform.RotateAround(transform.position, transform.up, 180f);
    }
    
    void Update () {

        if (plyScript.isOnGround && haveWallJumped)
        {
            haveWallJumped = false;
        }

        if (IsHanging)
        {
            var edge = currentEdge.transform.position;
            //var daHand = hand.transform.localPosition;
            var posToGo = new Vector2(edge.x - (hangOffset.x * dir), 
                edge.y - hangOffset.y);
            transform.position = Vector2.Lerp(transform.position, posToGo, Time.deltaTime * 10);
        }
        else
        {
            currentEdge = null;
        }




        if (isWithinWalljump && currentCol != null && canWallJump)
        {
            bool isRightWall = currentCol.name.StartsWith("R_");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                print("You jumped");
                if (isRightWall && dir < 0 && !haveWallJumped)
                {
                    plyScript.Jump(new Vector2(WallJumpSideX, WallJumpSideY));
                    haveWallJumped = true;
                }
                else if (!isRightWall && dir > 0 && !haveWallJumped)
                {
                    plyScript.Jump(new Vector2(-WallJumpSideX, WallJumpSideY));
                    haveWallJumped = true;
                }
                else if (!haveWallJumpedUpwards)
                {
                    plyScript.Jump(new Vector2(WallJumpForceUp.x * -dir, WallJumpForceUp.y));
                    haveWallJumpedUpwards = true;
                }
            }
        }
    }

    bool haveWallJumped = false;
    bool haveWallJumpedUpwards = false;

    float wallSlideTimer = 0;

    bool canWallJump = false;

    void OnTriggerExit2D(Collider2D col)
    {
        //if (col.tag == "Edge")
        //    plyScript.ignoreEdge = false;
        currentCol = null;

        if (col.tag == "Wall")
        {
            wallSlideTimer = 0;
            haveWallJumped = false;
            haveWallJumpedUpwards = false;
            isWithinWalljump = false;
            //print(isWithinWalljump);
        }
    }


    Collider2D currentCol;
    void OnTriggerEnter2D(Collider2D col)
    {
        currentCol = col;
        if (col.tag == "Wall")
        {
            isWithinWalljump = true;

            //print(isWithinWalljump);
        }
    }

    public GameObject jumpIndicator;

    void OnTriggerStay2D(Collider2D col)
    {

        if (rigidBody2D.velocity.y < 0 && !Input.GetKey(KeyCode.S) && col.tag == "Edge" && !plyScript.disableInput)
        {
            IsHanging = true;
            currentEdge = col.gameObject;
            //print("GRAB");
        }
        else if (rigidBody2D.velocity.y >= (MaxVelocityYOnWall - 2) && col.tag == "Wall" && !plyScript.isOnGround && !IsHanging)
        {
            jumpIndicator.SetActive(true);
            canWallJump = true;

        }
        else
        {
            canWallJump = false;
            jumpIndicator.SetActive(false);
        }

        if (col.tag == "Wall" && rigidBody2D.velocity.y < MaxVelocityYOnWall && rigidBody2D.velocity.y > -10)
        {
            if (wallSlideTimer > 0.5f)
                return;
            wallSlideTimer += Time.deltaTime;
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, MaxVelocityYOnWall);
        }

    }
}
