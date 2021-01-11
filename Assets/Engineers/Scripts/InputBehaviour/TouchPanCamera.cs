using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TouchPanCamera : TouchInputBehaviour {

    [SerializeField] private float _panSpeed;
    [SerializeField] private GameObject[] _bounds;

    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    private Plane[] _planes;

    private void Start() {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }

    void Update() {
        _planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
    }

    public override void Began(RaycastHit pHit, Touch pTouch) {
        lastPanPosition = pTouch.position;
        panFingerId = pTouch.fingerId;
    }

    public override void Moved(RaycastHit pHit, Touch pTouch) {
        if (Input.touchCount == 1)
        {
            if (pTouch.fingerId == panFingerId)
            {
                PanCamera(pTouch.position);
            }
        }
    }

    void PanCamera(Vector3 newPanPosition) {
        // Determine how much to move the camera
        Vector3 offset = Camera.main.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(-offset.x * _panSpeed, 0, -offset.y * _panSpeed);

        if (_bounds[0]!=null && !_planes[2].GetSide(_bounds[2].transform.position - move) && !_planes[3].GetSide(_bounds[3].transform.position - move) &&
            !_planes[0].GetSide(_bounds[0].transform.position - move) && !_planes[1].GetSide(_bounds[1].transform.position - move)) {
            transform.Translate(move, Space.World);
            TouchScript.Instance.ObjectsCache.PreprocessCamera.transform.Translate(move, Space.World);
        }

        // Cache the position
        lastPanPosition = newPanPosition;
    }
}
