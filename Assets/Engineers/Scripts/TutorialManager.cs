using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour {
    public Animator _animator;
    public int _currentState = 0;
    bool _onePassed = false;
    public GameObject _freeGround;
    public GameObject[] _positions;
    GeneratorCounter _generatorCounter;

    // Use this for initialization
    void Start () {
        _generatorCounter = TouchScript.Instance.ObjectsCache.GeneratorCounter;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SwitchState(int state)
    {
        //Debug.Log("TutMng: Called State " + state);
        _currentState = state;
        if (_currentState == 1 && _onePassed)
        {
            _currentState = 9;
        }

        if (_currentState == 1 && !_onePassed)
        {
            _animator.SetTrigger("TappedOnBuild");
            _onePassed = true; 
        }
        
        if (_currentState == 2)
        {
            _animator.SetTrigger("IGotTheBuildMode");
        }
        if (_currentState == 3)
        {
            _animator.SetTrigger("NowPlaceItHere");
            _freeGround.transform.position = _positions[0].transform.position;
        }
        if (_currentState == 4)
        {
            _animator.SetTrigger("GoodJob");
            
        }
        if (_currentState == 5)
        {
            _animator.SetTrigger("MoveThePanelFam");
            _freeGround.transform.position = _positions[1].transform.position;
        }
        if (_currentState == 6)
        {
            _animator.SetTrigger("ClickOnDaBish");
            //disable the build buttons
            //TouchScript.Instance.ToggleBuildMode();
        }
        if (_currentState == 7)
        {
            _animator.SetTrigger("ThereIsWind");
            _generatorCounter.IncreaseGeneratorCounter(1);
        }
        if (_currentState == 8)
        {
            _animator.SetTrigger("BuildAgain");
        }
        if (_currentState == 9)
        {
            _animator.SetTrigger("ClickWind"); //the real state 9
            _freeGround.transform.position = _positions[2].transform.position;
        }
        if (_currentState == 10)
        {
            _animator.SetTrigger("PlaceTurbineFam");
        }
        if (_currentState == 11)
        {
            _animator.SetTrigger("GetInfoAboutWind");
        }
        if (_currentState == 12)
        {
            _animator.SetTrigger("ThisIsEnergyFam");
        }
        if (_currentState == 13)
        {
            _animator.SetTrigger("OilRunsOut");
        }
        if (_currentState == 14)
        {
            _animator.SetTrigger("HappinessFam");
        }
        if (_currentState == 15)
        {
            _animator.SetTrigger("TapCapsules");
        }
        if (_currentState == 16)
        {
            _animator.SetTrigger("UReady");
        }
        if (_currentState == 69)
        {
            TouchScript.Instance.ObjectsCache.WinScreen.SetActive(true);
        }
        
        Debug.Log("TutMng: State " + _currentState);
    }
}
