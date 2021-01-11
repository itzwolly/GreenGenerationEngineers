using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBlink : MonoBehaviour {

    [SerializeField] float _speedAppear;
    [SerializeField] float _speedDissaper;
    [SerializeField] float _downTime;
    [SerializeField] float _maxError;

    Text _text;
    bool _isFlashing;
    bool _appearing;
    bool _disapearing;
    public bool IsFlashing
    {
        get { return _isFlashing; }
    }

    private void Start()
    {
        //Debug.Log("text blink activated");
        _isFlashing = false;
        _appearing = false;
        _disapearing = false;
        _text = GetComponent<Text>();
        StartFlashing();
    }

    // Use this for initialization
    void OnActivate()
    {
        //Debug.Log("text blink activated");
        _isFlashing = false;
        _appearing = false;
        _disapearing = false;
        _text = GetComponent<Text>();
        StartFlashing();
    }

    public void StartFlashing()
    {
        if (!_isFlashing && _text != null)
        {
            _isFlashing = true;
            _appearing = true;
            if (_text == null)
                _text = GetComponent<Text>();
            Color color = _text.color;
            color.a = 0;
            _text.color = color;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_isFlashing)
        {
            Color color = _text.color;
            if (_appearing)
            {
                color.a = Mathf.Lerp(color.a, 1, _speedAppear);
                if (color.a > 1 - _maxError)
                {
                    _appearing = false;
                    _disapearing = true;
                }
            }
            if (_disapearing)
            {
                color.a = Mathf.Lerp(color.a, 0, _speedDissaper);
                if (color.a < 0 + _maxError)
                {
                    _disapearing = false;
                    StartCoroutine(CanFlashAgain());
                }
            }
            _text.color = color;
            //Debug.Log(color.a);
        }

    }

    IEnumerator CanFlashAgain()
    {
        yield return new WaitForSeconds(_downTime);

        _isFlashing = true;
        _appearing = true;
    }
}
