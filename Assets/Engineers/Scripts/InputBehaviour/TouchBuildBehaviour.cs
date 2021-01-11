using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchBuildBehaviour : TouchInputBehaviour {
    [SerializeField] private TouchPinchToZoom _pinchToZoom;
	
    private void Start() {
        //Debug.Log("Adding to behaviours list..");
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }

    public override void Began(RaycastHit pHit, Touch pTouch) {
       if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.Build) {
            //Debug.Log("Hitting: " + pHit.transform.tag);
            if (pHit.transform != null) {
                //Debug.Log("2 Hitting: " + pHit.transform.tag);
                if (pHit.point != null && pHit.transform.tag == tag && !EventSystem.current.IsPointerOverGameObject(pTouch.fingerId)) {
                    //Debug.Log("3 Hitting: " + pHit.transform.tag);
                    if (TouchScript.Instance.Target == null && TouchScript.Instance.ObjectsCache.GeneratorCounter.HasGenerators()) {
                        //Debug.Log("4 Hitting: " + pHit.transform.tag);
                        TouchScript.Instance.Target = PlaceObject(TouchScript.Instance.CurrentPrefab, pHit.point, TouchScript.Instance.gameObject);
                    } else {
                        ReplaceObject(TouchScript.Instance.Target, pHit.point);
                    }
                }
            }
        }
    }

    public override void Ended(RaycastHit pHit, Touch pTouch) {
        if (TouchScript.Instance.CurrentMode != TouchScript.TouchState.Edit) {
            DisplayConfirmWindow(TouchScript.Instance.Target);
        }
    }

    private GameObject PlaceObject(GameObject pPrefab, Vector3 pPosition, GameObject pParent) {
        GameObject obj = Instantiate(pPrefab, new Vector3(pPosition.x, pPrefab.transform.position.y, pPosition.z), pPrefab.transform.rotation, pParent.transform);
        obj.GetComponent<ObjectState>().CurrentState = ObjectState.State.Preview;
        obj.SetActive(true);
        ChangeLayersRecursively(obj.transform, "PreProcess");
        return obj;
    }
}
