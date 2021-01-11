using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialWind : MonoBehaviour {

    [SerializeField] private float _speed;
    [SerializeField] private GameObject _area;
    [SerializeField] private Vector3 _checkOffset;
    [SerializeField] private LayerMask _layerMask;

    private bool _hasMoved = false;
    private bool _nextStepDisplayed = false;
    private bool _moveDisplayed = false;
    private ObjectState _objectState;

    private Text _tipText;
    private bool _startedLoadingNextLevel = false;
    private RaycastHit _hit;
    private Coroutine _moveWind;
    TutorialManager _tutorialManager;

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            _objectState = GetComponent<ObjectState>();
            _tipText = TouchScript.Instance.ObjectsCache.TipText.transform.GetChild(1).GetComponent<Text>();
            _tutorialManager = GameObject.FindGameObjectWithTag("TutorialManager").GetComponent<TutorialManager>();
        }
        else
        {
            enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if (!TouchScript.Instance.HasWon())
        {
            Ray ray = new Ray(_area.transform.position + _checkOffset, new Vector3(0, 1, 0));

            if (_objectState.CurrentState == ObjectState.State.Placed) //placed the windturbine
            {
                if (Physics.Raycast(ray, out _hit, Mathf.Infinity, _layerMask)) //hit raycast into something
                {
                    if (_hit.transform.tag == "WindArea") //if hit wind
                    {
                        if (_hasMoved && !_nextStepDisplayed)
                        {

                            //_tipText.text = "Het lijkt erop, dat jouw windturbine kleine hoeveelheden energy verliest. Klik op de capsules om het terug te krijgen.";

                        }
                        else if (!_hasMoved) //if wind did not move yet
                        {
                            _tutorialManager.SwitchState(11);
                            //MoveWind(new Vector3(6, 0, 0));
                            //TouchScript.Instance.ObjectsCache.TipText.SetActive(true);
                            //_tipText.text = "De wind beweegt naar het Oosten. Klik op de windturbine en verplaats het naar waar de wind is.";
                            _hasMoved = true;
                            _nextStepDisplayed = true;
                        }
                    }
                }
                else
                {
                    if (!_moveDisplayed)
                    {
                        _moveDisplayed = true;
                        _nextStepDisplayed = false;
                        TouchScript.Instance.ObjectsCache.TipText.SetActive(true);
                        _tipText.text = "Klik op de windturbine en verplaats het naar waar de wind is.";
                    }
                }
            }
        } else {
            //if (TouchScript.Instance.ObjectsCache.WinScreen != null) {
            //    if (!TouchScript.Instance.ObjectsCache.WinScreen.activeSelf) {
            //        TouchScript.Instance.ObjectsCache.WinScreen.SetActive(true);
            //    }
            //}
        }
    }

   private void MoveWind(Vector3 pOffset) {
        _moveWind = StartCoroutine(TouchScript.Instance.MoveTowards(_hit.transform, _speed, pOffset));
    }

   

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        if (_moveWind != null) {
            StopCoroutine(_moveWind);
        }
    }
}
