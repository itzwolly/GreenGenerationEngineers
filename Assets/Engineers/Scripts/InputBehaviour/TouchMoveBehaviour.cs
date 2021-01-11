using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchMoveBehaviour : TouchInputBehaviour {

    private void Start() {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }

    public override void Began(RaycastHit pHit, Touch pTouch) {
        if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.Move) {
            if (pHit.transform.tag == "Ground") {
                ReplaceObject(TouchScript.Instance.Target, pHit.point);
            }
        }
    }
}
