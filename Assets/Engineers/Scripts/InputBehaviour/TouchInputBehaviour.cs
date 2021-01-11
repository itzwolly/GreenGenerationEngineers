using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInputBehaviour : MonoBehaviour {
    public virtual void Began(RaycastHit pHit, Touch pTouch) { /* Empty */ }
    public virtual void Moved(RaycastHit pHit, Touch pTouch) { /* Empty */ }
    public virtual void Stationary(RaycastHit pHit, Touch pTouch) { /* Empty */ }
    public virtual void Ended(RaycastHit pHit, Touch pTouch) { /* Empty */ }
    public virtual void Canceled(RaycastHit pHit, Touch pTouch) { /*Empty*/ }
    protected bool _added = false;


    private void OnEnable()
    {
        if(!_added)
        {
            if (TouchScript.Instance.Behaviours != null)
            {
                //Debug.Log("adding to touchscript");
                TouchScript.Instance.Behaviours.Add(this);
                _added = true;
            }
        }
    }

    protected void ReplaceObject(GameObject pTarget, Vector3 pPosition) {
        if (pTarget != null) {
            ObjectState state = pTarget.GetComponent<ObjectState>();
            if (state != null) {
                if (state.CurrentState != ObjectState.State.Preview) {
                    state.CurrentState = ObjectState.State.Preview;
                }
            }

            ChangeLayersRecursively(pTarget.transform, "PreProcess");
            pTarget.transform.position = new Vector3(pPosition.x, pTarget.transform.position.y, pPosition.z);
        }
    }

    protected void DisplayConfirmWindow(GameObject pTarget) {
        if (pTarget != null) {
            GameObject confirmWindow = Utils.FindChildWithTag(pTarget, "Confirm").gameObject;
            if (!confirmWindow.activeSelf) {

                confirmWindow.SetActive(true);
            }
        }
    }

    protected void CloseConfirmWindow(GameObject pTarget) {
        if (pTarget != null) {
            GameObject confirmWindow = Utils.FindChildWithTag(pTarget, "Confirm").gameObject;
            if (confirmWindow.activeSelf) {

                confirmWindow.SetActive(false);
            }
        }
    }

    protected void DisplayEditWindow(GameObject pTarget) {
        if (pTarget != null) {
            GameObject editWindow = Utils.FindChildWithTag(pTarget, "Edit").gameObject;
            if (!editWindow.activeSelf) {
                TouchScript.Instance.EnableBuildMode();
                TouchScript.Instance.CurrentMode = TouchScript.TouchState.Edit;
                TouchScript.Instance.ObjectsCache.EditMalone.SetActive(true);
                if (TouchScript.Instance.Handler != null)
                {
                    TouchScript.Instance.Handler.SetActiveAreas(true);
                }
                editWindow.SetActive(true);
                TouchScript.Instance.Target = pTarget;
            }
        }
    }

    public void CloseEditWindow(GameObject pTarget, bool pDisableBuildOverlay) {
        if (pTarget != null) {
            GameObject editWindow = Utils.FindChildWithTag(pTarget, "Edit").gameObject;
            if (editWindow.activeSelf) {
                if (pDisableBuildOverlay) {
                    TouchScript.Instance.ObjectsCache.EditMalone.SetActive(false);
                    if (TouchScript.Instance.Handler != null)
                    {
                        TouchScript.Instance.Handler.SetActiveAreas(false);
                    }
                }
                editWindow.SetActive(false);
            }
        }
    }

    public void ChangeLayersRecursively(Transform pTransform, string pName) {
        foreach (Transform child in pTransform) {
            child.gameObject.layer = LayerMask.NameToLayer(pName);
            ChangeLayersRecursively(child, pName);
        }
    }
}
