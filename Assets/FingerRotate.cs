using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerRotate : TouchInputBehaviour
{

    [SerializeField] float _speed;
    [SerializeField] Vector3 _horizAxis;
    [SerializeField] Vector3 _vertAxis;

    Vector2 _start;
    Vector2 _end;
    int _dirHoriz = 0;
    int _dirVert = 0;
    bool _touch;
	// Use this for initialization
	void Start () {
        _touch = false;
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
	}

    public override void Began(RaycastHit pHit, Touch pTouch)
    {

        if (pHit.transform.tag == tag)
        {
            _start = pTouch.position - pTouch.deltaPosition;
            _end = pTouch.position - pTouch.deltaPosition;
            _touch = true;
        }
    }    
    

    public override void Moved(RaycastHit pHit, Touch pTouch)
    {
        if (pHit.transform != null) {
            if (pHit.transform.tag == tag) {
                _end = pTouch.position - pTouch.deltaPosition;
                _touch = true;
            }
        }
    }

    public override void Ended(RaycastHit pHit, Touch pTouch)
    {
        _touch = false;
        //_dirHoriz = 0; _dirVert = 0;
    }

    public void RotateEarth(float horiz,float vert)
    {
        gameObject.transform.RotateAround(transform.position, _horizAxis, horiz);
        gameObject.transform.RotateAround(transform.position, _vertAxis, vert);
    }

    private void FixedUpdate()
    {
        if(_touch)
        {
            RotateEarth((_start.x - _end.x) * _speed, (_start.y - _end.y) * _speed);
        }
    }
}
