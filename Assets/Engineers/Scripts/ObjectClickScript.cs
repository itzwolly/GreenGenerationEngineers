using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectClickScript : TouchInputBehaviour
{
    [SerializeField] protected GameObject _feedbackPanel;
    [SerializeField] protected string _tagToCheck;
    [SerializeField] protected float _appearanceSpeed = 1;
    [SerializeField] protected float _lerpError = 0.01f;
    [SerializeField] Vector3 _smallest = new Vector3(0,0,0);
    [SerializeField] Vector3 _biggest = new Vector3(1, 1, 1);
    [SerializeField] protected bool _enlarging;
    [SerializeField] protected bool _diminishing;
    // Use this for initialization
    void Start ()
    {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        _feedbackPanel.SetActive(false);
        if (_tagToCheck == null || _tagToCheck=="")
            _tagToCheck = tag;              
    }

    virtual protected void Activate()
    { /*Debug.Log("activate");*/
        _feedbackPanel.SetActive(true);
        _feedbackPanel.transform.localScale = _smallest;
        _enlarging = true;
    }

    virtual public void Deactivate()
    { /*Debug.Log("deactivate");*/
        _diminishing = true;
    }

    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        //Debug.Log(pHit.transform.name);
        //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
        if (Input.touchCount == 1)
        {
            if (pHit.point != null && pHit.transform.name == name)
            {
                Activate();
            }
            else
            {
                Deactivate();
            }
        }
        else
        {
            Deactivate();
        }
        //}
    }


    // Update is called once per frame
    virtual protected void Update () {
		if(_enlarging)
        {
            _feedbackPanel.transform.localScale = Vector3.Lerp(_feedbackPanel.transform.localScale, _biggest, _appearanceSpeed);
            if (_feedbackPanel.transform.localScale.x>_biggest.x-_lerpError)
            {
                _feedbackPanel.transform.localScale = _biggest;
                _enlarging = false;
            }
        }
        else if(_diminishing)
        {
            _feedbackPanel.transform.localScale = Vector3.Lerp(_feedbackPanel.transform.localScale, _smallest, _appearanceSpeed);
            if (_feedbackPanel.transform.localScale.x < _smallest.x + _lerpError)
            {
                _feedbackPanel.transform.localScale = _smallest;
                _diminishing = false;
                _feedbackPanel.SetActive(false);
            }
        }
	}
}
