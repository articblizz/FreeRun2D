using UnityEngine;
using System.Collections;

public class FreerunScript : MonoBehaviour {

    public bool IsHanging;

    public Rigidbody2D rigidBody2D;

    GameObject currentEdge;
    public GameObject hand;

    public bool isFacingRight = true;

    public Vector2 hangOffset = new Vector2(0.5f, 1);

    public Vector2 WallJumpForceUp = new Vector2(10, 30);

    public int dir = 1;

    PlayerScript plyScript;

    void Awake()
    {
    }

	void Start () {

        rigidBody2D = GetComponent<Rigidbody2D>();
        plyScript = GetComponent<PlayerScript>();
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

        if (plyScript.isOnGround)
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
	}

    bool haveWallJumped = false;
    bool haveWallJumpedUpwards = false;

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Wall")
        {
            haveWallJumped = false;
            haveWallJumpedUpwards = false;
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        print(col.tag);
        if (rigidBody2D.velocity.y < 0 && !Input.GetKey(KeyCode.S) && col.tag == "Edge" && !plyScript.disableInput)
        {
            IsHanging = true;
            currentEdge = col.gameObject;
            //print("GRAB");
        }
        else if(rigidBody2D.velocity.y > -0.3f && col.tag == "Wall" && !plyScript.isOnGround)
        {
            bool isRightWall = col.name.StartsWith("R_");

            if(Input.GetKeyDown(KeyCode.Space))
            {
                var x = 23;
                var y = 15;
                    
                if (isRightWall && dir > 0 && !haveWallJumped)
                {
                    plyScript.Jump(new Vector2(x, y));
                    haveWallJumped = true;
                }
                else if (!isRightWall && dir < 0 && !haveWallJumped)
                {
                    plyScript.Jump(new Vector2(-x, y));
                    haveWallJumped = true;
                }
                else if(!haveWallJumpedUpwards)
                {
                    plyScript.Jump(new Vector2(WallJumpForceUp.x * dir, WallJumpForceUp.y));
                    haveWallJumpedUpwards = true;
                }
            }
        }
    }
}
