using UnityEngine;
using System.Collections;

public class FreerunScript : MonoBehaviour {

    public bool IsHanging;

    public Rigidbody2D rigidBody2D;

    GameObject currentEdge;

    public GameObject hand;

    public bool isFacingRight = true;    

    public float hangingOffset = 0.9f;

    int d = 1;

    void Awake()
    {
    }

	void Start () {

        rigidBody2D = GetComponent<Rigidbody2D>();
	}

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        d *= -1;
        //transform.RotateAround(transform.position, transform.up, 180f);
    }
	
	void Update () {

        //var direction = (transform.position - arm.transform.position);
        //var rotation = Quaternion.LookRotation(direction);
        //arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, rotation, Time.deltaTime * 5);

        if (IsHanging)
        {
            var edge = currentEdge.transform.position;
            //var daHand = hand.transform.localPosition;
            var posToGo = new Vector2(edge.x - (hangingOffset * d), 
                edge.y - (hangingOffset));
            transform.position = Vector2.Lerp(transform.position, posToGo, Time.deltaTime * 10);
        }
        else
        {
            currentEdge = null;
        }
	}

    void OnTriggerStay2D(Collider2D col)
    {
        if (rigidBody2D.velocity.y < 0)
        {
            IsHanging = true;
            currentEdge = col.gameObject;
            //print("GRAB");
        }
    }
}
