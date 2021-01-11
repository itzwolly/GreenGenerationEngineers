using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyboard : MonoBehaviour {
    [SerializeField] float _minY;
    [SerializeField] float _maxY;
    [SerializeField] float _speed;
    [SerializeField] float _error;

    bool _raise;
    bool _drop;
    RectTransform _transform;
	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
        _transform = gameObject.GetComponent<RectTransform>();
        _transform.offsetMax = new Vector2(0, -_minY);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
    }
	
	// Update is called once per frame
	void Update () {
        if(_raise)
        {
            _transform.offsetMax = new Vector2(0, Mathf.Lerp(_transform.offsetMax.y, -_maxY,_speed));

            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale,new Vector3(1, 1, 1),_speed);
            if (_transform.offsetMax.y>-_maxY-_error)
            {
                _raise = false;
                gameObject.transform.localScale = new Vector3(1,1,1);
            }
            //Debug.Log("raising "+_transform.offsetMax.y + " | " + -_maxY );
        }

		if(_drop)
        {
            _transform.offsetMax = new Vector2(0, Mathf.Lerp(_transform.offsetMax.y, -_minY, _speed));
            gameObject.transform.localScale = Vector3.Lerp(gameObject.transform.localScale, new Vector3(0,0,0), _speed);
            if (_transform.offsetMax.y < -_minY + _error)
            {
                _drop = false;
                gameObject.SetActive(false);
                _transform.offsetMax = new Vector2(0, -_minY);
                gameObject.transform.localScale = new Vector3(0,0,0);
            }
            //Debug.Log("dropping " + _transform.offsetMax.y + " | " + -_minY);
        }

	}

    public void Activate()
    {
        gameObject.SetActive(true);
        gameObject.transform.localScale = new Vector3(0, 0, 0);
        _raise = true;
        _drop = false;
        _transform = gameObject.GetComponent<RectTransform>();
    }

    public void Deactivate()
    {
        _raise = false;
        _drop = true;
        _transform = gameObject.GetComponent<RectTransform>();
        gameObject.transform.localScale = new Vector3(1, 1, 1);
    }

}
