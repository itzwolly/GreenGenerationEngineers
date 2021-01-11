using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : TouchInputBehaviour {

    [SerializeField] private ParticleSystem _particle;

    private void Start() {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
    }


    public override void Began(RaycastHit pHit, Touch pTouch) {
        if (pHit.transform != null) {
            if (Input.touchCount == 1) {
                if (pHit.transform.tag == tag) {
                    Instantiate(_particle, new Vector3(pHit.point.x, pHit.point.y+0.15f, pHit.point.z), Quaternion.Euler(-90,0,0));
                }
            }
        }
    }
}
