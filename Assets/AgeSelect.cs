using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgeSelect : TouchInputBehaviour {
    [SerializeField] int _difficulty;

    InfoScript _infoScript;
	// Use this for initialization
	void Start ()
    {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        _infoScript = GameObject.FindGameObjectWithTag("InfoScript").GetComponent<InfoScript>();
        //if (_infoScript.Playing)
        //    transform.parent.gameObject.SetActive(false);
    }


    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
        if (Input.touchCount == 1)
        {
            if (pHit.point != null && pHit.transform.name == name)
            {
                //_infoScript.SetDifficulty(_difficulty);
                transform.parent.gameObject.SetActive(false);
            }

        }
    }


    public void SetDifficulty()
    {
        _infoScript.SetDifficulty(_difficulty);
        transform.parent.gameObject.SetActive(false);
    }
}
