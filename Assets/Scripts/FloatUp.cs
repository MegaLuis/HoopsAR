using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        iTween.FadeFrom(gameObject, 0, .5f);
        iTween.MoveBy(gameObject, iTween.Hash("y", 5, "easeType", "spring", "delay", .3));
	}
}
