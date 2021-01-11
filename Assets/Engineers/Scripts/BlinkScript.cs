using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlinkScript : MonoBehaviour {
    
    [SerializeField] float _speedAppear;
    [SerializeField] float _speedDissaper;
    [SerializeField] float _downTime;
    [SerializeField] float _maxError;

    Image _image;
    bool _isFlashing;
    bool _appearing;
    bool _disapearing;
    public bool IsFlashing
    {
        get { return _isFlashing; }
    }
	// Use this for initialization
	void Start () {
        _isFlashing = false;
        _appearing = false;
        _disapearing = false;
        _image = GetComponent<Image>();
        //StartFlashing();
	}

    public void StartFlashing()
    {
        //Debug.Log("start flashing");
        if (!_isFlashing && _image != null)
        {
            _isFlashing = true;
            _appearing = true;
            if (_image == null)
                _image = GetComponent<Image>();
            Color color = _image.color;
            color.a = 0;
            _image.color = color;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(_isFlashing)
        {
            Color color = _image.color;
            if(_appearing)
            {
                color.a = Mathf.Lerp(color.a, 1, _speedAppear);
                if(color.a>1-_maxError)
                {
                    _appearing = false;
                    _disapearing = true;
                }
            }
            if(_disapearing)
            {
                color.a = Mathf.Lerp(color.a, 0, _speedDissaper);
                if (color.a < 0+_maxError)
                {
                    _disapearing = false;
                    StartCoroutine(CanFlashAgain());
                }
            }
            _image.color = color;
            //Debug.Log(color.a);
        }
		
	}

    IEnumerator CanFlashAgain()
    {
        yield return new WaitForSeconds(_downTime);

        _isFlashing = false;
    }
}
