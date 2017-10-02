using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FreePlayGameController : MonoBehaviour {

    public GameObject pointsText;

    //Free play variables
    int currentGoalCount = 0;
    int streakCount = 0;

    private bool streakActive;

    private AudioSource thisAudio;

    void OnEnable()
    {
        BallControl.OnGoal += HandleGoalAction;
        BallControl.OnFail += HandleFailedAction;
    }

    void OnDisable()
    {
        BallControl.OnGoal -= HandleGoalAction;
        BallControl.OnFail -= HandleFailedAction;
    }

    // Use this for initialization
    void Start () {
        thisAudio = GetComponent<AudioSource>();
	}

	void HandleGoalAction(float distance, float maxHeight, bool floored, bool clear, bool special)
	{
        streakActive = true;
        currentGoalCount++;
        pointsText.GetComponent<Text>().text = currentGoalCount.ToString();

        if (streakActive)
            streakCount++;

        //Call script that handles visual feedback like
        //Points & Particles
	}

    void HandleFailedAction()
    {
        streakActive = false;
        streakCount = 0;
    }
}
