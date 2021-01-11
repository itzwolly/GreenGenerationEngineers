using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavecontroller : MonoBehaviour {
    public float height;
    public float time;

	
void Start () {
        iTween.MoveBy(this.gameObject, iTween.Hash("y", height, "time", time, "looptype", "pingpong", "easetype", iTween.EaseType.easeInOutSine));
	}
	
void Update () {
		
	}
}
