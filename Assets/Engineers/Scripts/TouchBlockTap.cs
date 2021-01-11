using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchBlockTap : MonoBehaviour {
    Animator _animator;
	// Use this for initialization
	void Start () {
        _animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TapMe()
    {
        //Debug.Log("CANT TAP YET FAM");
        _animator.SetTrigger("Popup");

    }
}
