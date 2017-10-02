using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrialGameController : MonoBehaviour {

	public GameObject timeText;
	public GameObject gameOverText;
	public GameObject pointsText;

	int points = 0;
	int streakCount = 0;

	//Initial Time for TimeTrial
	float timeLeft = 24;
	float shotClockTime;
	bool gameOver = false;

	void OnEnable()
	{
		BallControl.OnGoal += HandleGoalAction;
		BallControl.OnFail += HandleFailedAction;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!gameOver)
		{
			timeLeft -= Time.deltaTime;
			shotClockTime = timeLeft;
			shotClockTime = Mathf.FloorToInt(timeLeft);
			timeText.GetComponent<Text>().text = shotClockTime.ToString();

			if(timeLeft <= 0)
			{
				timeLeft = 0;
				gameOver = true;
				GameOver();
			}
			else if(timeLeft >= 24)
			{
				timeLeft = 24;
			}

		}
	}

	void GameOver()
	{
		timeLeft = 0;
		gameOverText.SetActive(true);
		timeText.SetActive(false);
		pointsText.SetActive(true);
	}

	void HandleGoalAction(float distance, float maxHeight, bool floored, bool clear, bool special)
	{
		streakCount++;
		points++;
		pointsText.GetComponent<Text>().text = points.ToString();
		timeLeft += 5f;

		//Call script that handles visual feedback like
		//Points & Particles
	}

	void HandleFailedAction()
	{
		streakCount = 0;
	}
}
