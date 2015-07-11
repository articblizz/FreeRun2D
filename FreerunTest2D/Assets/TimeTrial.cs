using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TimeTrial : MonoBehaviour {


	public int NextCheckPoint = 0;


	float timer;
	bool isLive = false;

	public GameObject[] CheckPoints;

	GameObject goal, start;

	public Vector2 StartPos;

	public GameObject Player;

	public Text uiTimer;

    public Color NextTargetColor;
    Color standardColor;

    public GameObject indicator;

	// Use this for initialization
	void Start () {

		start = CheckPoints[0];

		Player.transform.position = StartPos;

		goal = CheckPoints[CheckPoints.Length - 1];

        standardColor = CheckPoints[1].GetComponent<SpriteRenderer>().color;
        
	}
	
	// Update is called once per frame
	void Update () {

		if (isLive)
		{
			timer += Time.deltaTime;
            indicator.transform.position = Player.transform.position;

            var pos1 = indicator.transform;
            var pos2 = CheckPoints[NextCheckPoint].transform.position;
            Vector3 dir = pos2 - pos1.position;

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

            pos1.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



            indicator.transform.rotation = pos1.rotation;

		}

        uiTimer.text = string.Format("{0}/{1}  {2:0.00} s", NextCheckPoint,CheckPoints.Length,timer);
	}

	public void PlayerEnterCheckpoint(GameObject checkPoint)
	{
        checkPoint.GetComponent<SpriteRenderer>().color = standardColor;
		if (checkPoint == start)
		{
            indicator.SetActive(true);
			isLive = true;
			start.SetActive(false);
			NextCheckPoint++;
            CheckPoints[NextCheckPoint].GetComponent<SpriteRenderer>().color = NextTargetColor;

		}
		else if (checkPoint == goal)
		{
            indicator.SetActive(false);
			isLive = false;
			print(string.Format("You finished at {0:0.00} seconds", timer));
            NextCheckPoint++;
			goal.SetActive(false);
		}
		else if (checkPoint == CheckPoints[NextCheckPoint])
		{
            //checkPoint.GetComponent<SpriteRenderer>().color = standardColor;
			checkPoint.SetActive(false);
			NextCheckPoint++;

            if(CheckPoints[NextCheckPoint] != goal)
                CheckPoints[NextCheckPoint].GetComponent<SpriteRenderer>().color = NextTargetColor;
		}
	}


	void Reset()
	{
		timer = 0;
		isLive = false;
		NextCheckPoint = 0;
		for (int i = 0; i < CheckPoints.Length; i++)
		{
			CheckPoints[i].SetActive(true);
		}
	}

}
