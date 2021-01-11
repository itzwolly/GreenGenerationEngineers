using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialSolar : MonoBehaviour {

    Vector3 _lightDir;
    [SerializeField] private float _speed;
    [SerializeField] private LayerMask _layerMask;
    private ObjectState _objectState;
    private bool _hasMoved = false;
    private bool _nextStepDisplayed = false;
    private bool _moveDisplayed = false;

    private Text _tipText;
    private bool _startedLoadingNextLevel = false;
    private RaycastHit _hit;
    private Coroutine _moveCloud;
    private SolarEnergy _solar;
    [SerializeField] private GameObject _cloud;
    TutorialManager _tutorialManager;
    

    // Use this for initialization
    void Start () {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            _objectState = GetComponent<ObjectState>();
            _tipText = TouchScript.Instance.ObjectsCache.TipText.transform.GetChild(1).GetComponent<Text>();
            _solar = GetComponent<SolarEnergy>();
            _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        }
        else
        {
            enabled = false;
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (!TouchScript.Instance.HasWon())
        {
            if (_objectState.CurrentState == ObjectState.State.Placed) //placed the solar
            {
                Debug.Log("Placed Solar...");
                
                if (!_solar.CheckCorners()) // TRUE MEANS THAT IT IS HITTING
                    //TRUE SELF IS WITHOUT FORM
                {
                    if (_hasMoved && !_nextStepDisplayed)
                    {
                        _tutorialManager.SwitchState(7);
                        //TouchScript.Instance.ToggleBuildMode();


                        //_tipText.text = "Het lijkt erop, dat jouw solar paneel kleine hoeveelheden energy verliest. Klik op de capsules om het terug te krijgen.";
                        _nextStepDisplayed = true;
                    }
                    else if (!_hasMoved) //if cloud did not move yet
                    {
                        _tutorialManager.SwitchState(4);
                        Debug.Log("MUV DA CLOUD");
                        MoveCloud(new Vector3(6, 0, 0));
                        //_tipText.text = "De wolk beweegt naar jouw locatie. Klik op de solar paneel en verplaats het naar waar de zon is.";
                        _hasMoved = true;
                        _nextStepDisplayed = true;
                    }
                }
                else
                {
                    if (!_moveDisplayed)
                    {
                        _moveDisplayed = true;
                        _nextStepDisplayed = false;
                        TouchScript.Instance.ObjectsCache.TipText.SetActive(true);
                        _tipText.text = "Klik op de solar paneel en verplaats het naar waar de zon is.";
                    }
                }
            }
        }
        else {
            //if (!TouchScript.Instance.ObjectsCache.WinScreen.activeSelf)
            //{
            //    TouchScript.Instance.ObjectsCache.WinScreen.SetActive(true);
                
            //}
        }
	}

    bool QuickCheck()
    {
      
        Ray ray = new Ray(transform.position, -_lightDir);
        Debug.DrawRay(ray.origin,ray.direction,Color.cyan);
        if (Physics.Raycast(ray, out _hit, Mathf.Infinity, _layerMask))
        {
            Debug.Log("HITS THE TAG: "+_hit.transform.tag);
            if (_hit.transform.tag == "Cloud")
            {
                Debug.Log("HIT DA CLAUD");
                _cloud = _hit.transform.gameObject;
                return true;
            }

            
        }

       
        return false;

    }

    private void MoveCloud(Vector3 pOffset)
    {
        _moveCloud = StartCoroutine(TouchScript.Instance.MoveTowards(_cloud.transform, _speed, pOffset));
    }
}
