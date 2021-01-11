using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRotate : FingerRotate {

    EarthMenu _earthMenu;
    // Use this for initialization
    void Start ()
    {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        _earthMenu = GetComponent<EarthMenu>();

    }

    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        if (_earthMenu != null && !_earthMenu.Rotating)
            base.Began(pHit, pTouch);
    }
}
