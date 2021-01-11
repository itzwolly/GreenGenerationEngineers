using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchEditBehaviour : TouchInputBehaviour {

    private TouchPinchToZoom _pinchToZoom;
    private ObjectState _objectState;
    private Coroutine _cameraZoom;
    
    private void Start() {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        
        _pinchToZoom = GameObject.FindWithTag("TouchScript").GetComponent<TouchPinchToZoom>();
    }

    public override void Began(RaycastHit pHit, Touch pTouch) {
        //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
        if (Input.touchCount == 1) {
            if (pHit.point != null && pHit.transform.tag == tag) {
                if (TouchScript.Instance.Target == null) {
                    _objectState = pHit.transform.GetComponent<ObjectState>();
                    if (_objectState != null) {
                        if (_objectState.CurrentState == ObjectState.State.Placed) {
                            _cameraZoom = StartCoroutine(_pinchToZoom.ResetCamera());
                            DisplayEditWindow(pHit.transform.gameObject);
                            EnableMove();
                            ChangeLayersRecursively(pHit.transform, "PreProcess");
                        }
                    }
                }
            }
        }
        //}
    }

    private void EnableMove() {
        if (TouchScript.Instance.CurrentMode != TouchScript.TouchState.Move) {
            TouchScript.Instance.TargetOriginalPosition = TouchScript.Instance.Target.transform.position;
            TouchScript.Instance.Target.GetComponent<ObjectState>().CurrentState = ObjectState.State.Preview;

            Energy energy = TouchScript.Instance.Target.GetComponent<Energy>();
            ConnectionMaker connect = energy.GetConnectionMaker();

            connect.DisconnectConnection();
            energy.PickUp();

            TouchScript.Instance.CurrentMode = TouchScript.TouchState.Move;
        }
    }
}
