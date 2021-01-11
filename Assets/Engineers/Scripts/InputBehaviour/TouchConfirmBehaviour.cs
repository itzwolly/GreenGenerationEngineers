using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchConfirmBehaviour : TouchInputBehaviour {

    private EnvirolmentalControl _envirolmentalControl;
    private CityController _cityController;
    private HappinesHandler _happinesHandler;
    private GameObject _accept;
    private GameObject _decline;

    private void Start() {
        _accept = Utils.FindChildWithTag(gameObject, "Accept").gameObject;
        _decline = Utils.FindChildWithTag(gameObject, "Decline").gameObject;

        _envirolmentalControl = GameObject.FindWithTag("EnvironmentalHandler").GetComponent<EnvirolmentalControl>();
        _cityController = GameObject.FindWithTag("CityHandler").GetComponent<CityController>();
        _happinesHandler = GameObject.FindGameObjectWithTag("HappinessHandler").GetComponent<HappinesHandler>();

        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }

    public override void Began(RaycastHit pHit, Touch pTouch) {
        //Debug.Log(pHit.transform.name + " | " + pHit.transform.tag);
        //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
        if (pHit.transform.tag == _accept.tag) {
            if (TouchScript.Instance.ObjectsCache.GeneratorCounter.AmountOfGenerators > 0) {
                ConfirmObjectPlacement(TouchScript.Instance.GetDefaultState(), false);
            } else {
                ConfirmObjectPlacement(TouchScript.TouchState.Build, true);
            }
        } else if (pHit.transform.tag == _decline.tag) {
            DeclineObjectPlacement(TouchScript.TouchState.Build);
        }
        //}
    }

    public void ConfirmObjectPlacement(TouchScript.TouchState pMode, bool pDisableBuildOverlay) {
        if (TouchScript.Instance.Target != null) {
            //Debug.Log("Confirming placement...");
            ObjectState state = TouchScript.Instance.Target.GetComponent<ObjectState>();
            if (state != null) {
                state.CurrentState = ObjectState.State.Placed;
                gameObject.SetActive(false);
                CloseConfirmWindow(TouchScript.Instance.Target);
                CloseEditWindow(TouchScript.Instance.Target, pDisableBuildOverlay);

                Energy energy = TouchScript.Instance.Target.GetComponent<Energy>();
                energy.SetCity(_cityController);
                energy.SetEnvirolment(_envirolmentalControl);
                energy.StartConnect();
                energy.SetData();

                if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.Build) {
                    TouchScript.Instance.ObjectsCache.GeneratorCounter.ReduceGeneratorCounter(1);
                    _happinesHandler.PlacedGenerator(TouchScript.Instance.Target);
                }

                if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.Move) {
                    _happinesHandler.CheckGenerator(TouchScript.Instance.Target);
                    //TouchScript.Instance.ToggleBuildMode();
                }

                if (TouchScript.Instance.ObjectsCache.GeneratorCounter.AmountOfGenerators == 0) {
                    pMode = TouchScript.TouchState.None;
                }

                ChangeLayersRecursively(TouchScript.Instance.Target.transform, "Default");
                TouchScript.Instance.Target = null;
                TouchScript.Instance.CurrentMode = pMode;
            } 
        }
    }

    public void DeclineObjectPlacement(TouchScript.TouchState pMode) {
        if (TouchScript.Instance.Target != null) {
            //Debug.Log("Declining placement...");
            if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.Move) {
                TouchScript.Instance.Target.transform.position = TouchScript.Instance.TargetOriginalPosition;
                ObjectState state = TouchScript.Instance.Target.GetComponent<ObjectState>();
                if (state != null) {
                    state.CurrentState = ObjectState.State.Placed;
                }
                if (TouchScript.Instance.ObjectsCache.GeneratorCounter.AmountOfGenerators > 0) {
                    ConfirmObjectPlacement(TouchScript.Instance.GetDefaultState(), false);
                } else {
                    ConfirmObjectPlacement(TouchScript.Instance.GetDefaultState(), true);
                }
                
                TouchScript.Instance.Target = null;
            } else {
                ChangeLayersRecursively(TouchScript.Instance.Target.transform, "Default");
                Destroy(TouchScript.Instance.Target);
                TouchScript.Instance.Target = null;
            }
            
            TouchScript.Instance.CurrentMode = pMode;
        }
    }
}
