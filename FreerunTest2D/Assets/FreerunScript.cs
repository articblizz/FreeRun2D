using UnityEngine;
using System.Collections;

public class FreerunScript : MonoBehaviour {

    public bool IsHanging;

    public Rigidbody2D rigidBody2D;

    GameObject currentEdge;

    public GameObject arm;

    void Awake()
    {
    }

	void Start () {

	
	}
	
	void Update () {

        var direction = (transform.position - arm.transform.position).normalized;
        var rotation = Quaternion.LookRotation(direction);
        arm.transform.rotation = Quaternion.Slerp(arm.transform.rotation, rotation, Time.deltaTime * 5);

        if (IsHanging)
        {
            transform.position = Vector2.Lerp(transform.position, currentEdge.transform.position, Time.deltaTime * 8);
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
            print("GRAB");
        }
    }
}
