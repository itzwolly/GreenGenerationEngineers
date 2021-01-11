using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancelPopout : TouchInputBehaviour
{
    [SerializeField] ObjectClickScript _objectWithDeactivate;
    // Use this for initialization
    private void Start()
    {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }
    public override void Began(RaycastHit pHit, Touch pTouch)
    {

        //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
        if (Input.touchCount == 1)
        {
            if (pHit.point != null && pHit.transform.name == name)
            {
                //Debug.Log(pHit.transform.name);
                _objectWithDeactivate.Deactivate();
                transform.parent.Translate(0,10000,0);
            }
        }
        else
        {
            _objectWithDeactivate.Deactivate();
            transform.parent.Translate(0, 10000, 0);
        }
        //}
    }
}
