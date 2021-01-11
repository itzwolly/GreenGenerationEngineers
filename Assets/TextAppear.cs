using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAppear : MonoBehaviour {
    [SerializeField] Text _text;
    [SerializeField] string _message;
    [SerializeField] float _floatDist;
    [SerializeField] float _speed;
    float _distanceTraveled;
    RectTransform _transform;
	// Use this for initialization

    public void SetMessage(string message)
    {
        //Debug.Log(message);
        _message = message;
        _distanceTraveled = 0;
        _text.text = message;
        _transform = GetComponent<RectTransform>();
    }

	void Start ()
    {
        //_message = null;
    }
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(_message); 
        if (_distanceTraveled < _floatDist)
        {
            Vector3 pos = _transform.anchoredPosition;
            //Debug.Log("pos = "+pos);
            pos.y -= _speed;
            pos.x -= _speed;
            _transform.anchoredPosition = pos;

            _distanceTraveled += _speed;
            Color col = _text.color;
            col.a = 1-_distanceTraveled / _floatDist;
            _text.color = col;

            //Vector3 scale = _text.transform.localScale;
            //scale = new Vector3(1 - _distanceTraveled / _floatDist, 1 - _distanceTraveled / _floatDist, 1 - _distanceTraveled / _floatDist);
            //_text.transform.localScale = scale; 
        }
        else if (_distanceTraveled >= _floatDist)
        {
            Destroy(gameObject);
        }
	}
}
