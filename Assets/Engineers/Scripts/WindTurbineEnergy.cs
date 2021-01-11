using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(ObjectState))]
public class WindTurbineEnergy : Energy
{
    [SerializeField] GameObject _area;
    [SerializeField] Vector3 _checkOffset;
    [SerializeField] private LayerMask _layerMask;

    private ObjectState _objectState;
    private Animator _animator;

    private void Start() {
        _objectState = GetComponent<ObjectState>();

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            _maxGreenCreated = _tutorialMaxGreenCreated;
        }
        _animator = GetComponent<Animator>();
    }

    public override void SetData()
    {
        base.SetData();
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        //if (_objectState.CurrentState == ObjectState.State.Placed) {
        RaycastHit hit;
        Ray ray = new Ray(_area.transform.position + _checkOffset, new Vector3(0, 1, 0));

        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
            
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)) {
            if (hit.transform.tag == "WindArea") {
                //Debug.Log("Hitting that WindArea!");
                if (_active != true) {
                    _active = true;
                    base.CorrectParticles();
                    _procent = 1;
                    _animator.StopPlayback();
                }
            } else {
                //Debug.Log("Hitting something other than WindArea!");
                if (_active != false) {
                    _active = false;
                    base.CorrectParticles();
                    _procent = 0;
                    _animator.StartPlayback();
                }
            }
        } else {
            //Debug.Log("Not hitting anything!");
            if (_active != false) {
                _active = false;
                base.CorrectParticles();
                _procent = 0;
                _animator.StartPlayback();
            }
        }
        //}
    }
}
