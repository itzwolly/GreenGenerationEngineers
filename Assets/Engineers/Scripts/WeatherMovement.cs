using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherMovement : MonoBehaviour {
    float _minDistance;
    Vector3 _min;
    Vector3 _max;
    float _speed;
    Vector3 _nextLocation;
    float _initialHeight;

    private void Start()
    {
        _initialHeight = transform.position.y;
    }

    public void SetMinMax(Vector3 min, Vector3 max)
    {
        _initialHeight = transform.position.y;
        _minDistance = _minDistance * _minDistance;
        _min = min;
        _min.y += _initialHeight;
        _max = max;
        _max.y += _initialHeight;
    }

    public void SetSpeedAndDistance(float speed,float minDist)
    {
        _speed = speed;
        _minDistance = minDist;
    }

	public void ChooseMovement()
    {
        _nextLocation = new Vector3(Random.Range(_min.x,_max.x), Random.Range(_min.y, _max.y), Random.Range(_min.z, _max.z));
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _nextLocation, _speed);
        if((transform.position-_nextLocation).sqrMagnitude<_minDistance)
        {
            ChooseMovement();
        }
    }
}
