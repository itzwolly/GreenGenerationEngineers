using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InactiveScript : TouchInputBehaviour {
    [SerializeField] GameObject _canvas;
    [SerializeField] int _maxCount;
    [SerializeField] string _mainMenu;
    TouchScript _touchScript;
    int _count;

    bool _active;
    bool _loadMainMenu;

	// Use this for initialization
	void Start () {
        _touchScript = TouchScript.Instance.GetComponent<TouchScript>();
        TouchScript.Instance.Behaviours.Add(this);
        _canvas.SetActive(false);
        _loadMainMenu = false;
	}

    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        _count = 0;
        _canvas.SetActive(false);
        _active = false;
    }

    public void HaveNotTouched()
    {
        //Debug.Log("setactive");
        _count = 0;
        _canvas.SetActive(true);
        _active = true;
    }

    private void FixedUpdate()
    {
        if (_active) {
            if (!_loadMainMenu) {
                _count++;
                if (_count > _maxCount) {
                    _loadMainMenu = true;
                    InfoScript.Instance.GetComponent<InfoScript>().NotPlaying();
                    TouchScript.Instance.GetComponent<TouchScript>().LoadNextLevel(_mainMenu);
                }
            }
        }
    }


}
