using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TouchScript : Singleton<TouchScript> {
    // Use this for initialization
    private SceneObjectsCache _objectsCache;

    private List<TouchInputBehaviour> _behaviours;
    private GameObject _target;
    private GameObject _currentPrefab;
    private RaycastHit _hit;
    private Vector3 _targetOriginalPosition;
    private TouchPinchToZoom _touchPinchToZoom;
    private TouchState _mode;
    private bool _hasWon;
    private bool _initialized;
    private HappinesHandler _happinessHandler;

    int _time;
    bool _inactive;

    public GameObject Target {
        get { return _target; }
        set { _target = value; }
    }
    public GameObject CurrentPrefab {
        get { return _currentPrefab; }
        set { _currentPrefab = value; }
    }
    public TouchState CurrentMode {
        get { return _mode; }
        set { _mode = value; }
    }
    public List<TouchInputBehaviour> Behaviours {
        get { return _behaviours; }
    }
    public Vector3 TargetOriginalPosition {
        get { return _targetOriginalPosition; }
        set { _targetOriginalPosition = value; }
    }
    public SceneObjectsCache ObjectsCache {
        get { return _objectsCache; }
    }
    public HappinesHandler Handler {
        get { return _happinessHandler; }
    }
    public TouchPinchToZoom PinchToZoom {
        get { return _touchPinchToZoom; }
    }

    public enum TouchState {
        None,
        Build,
        Edit,
        Move
    }

    private void FixedUpdate()
    {
        if (_objectsCache.WaitCanvas != null)
        {
            _time++;
            if (!_inactive && _time > 1200)//1800)//30 sec
            {
                _objectsCache.WaitCanvas.HaveNotTouched();
                _inactive = true;
            }
        }
    }

    private void Update() {
        //Debug.Log("Look at me I'm in mode: " + _mode);

        for (int i = 0; i < Input.touchCount; i++) {
            Touch touch = Input.GetTouch(i);
            switch (touch.phase) {
                case TouchPhase.Began:
                    _time = 0;
                    _inactive = false;
                    Vector3 pos = touch.position;
                    Ray ray = Camera.main.ScreenPointToRay(pos);

                    //Debug.Log("Behaviour count: " + _behaviours.Count);

                    if (Physics.Raycast(ray, out _hit, Mathf.Infinity, _objectsCache.LayerMask)) {
                        try
                        {
                            _objectsCache.RaycastHitText.text = "Raycast hit: " + _hit.transform.name + " | " + _hit.transform.tag + " | " + TouchScript.Instance.CurrentMode;
                            _objectsCache.WorldPositionText.text = "World Position: " + _hit.point;
                        }
                        catch
                        {
                            Debug.Log("no object cache");
                        }

                        for (int k = _behaviours.Count - 1; k >= 0; k--) {
                            if (_behaviours[k] == null) {
                                _behaviours.Remove(_behaviours[k]);
                                continue;
                            }
                            _behaviours[k].Began(_hit, touch);
                        }
                    } else {
                        try
                        {
                            _objectsCache.RaycastHitText.text = "Raycast hit: N/A" + " | " + TouchScript.Instance.CurrentMode;
                            _objectsCache.WorldPositionText.text = "World Position: N/A";
                        }
                        catch
                        {
                            Debug.Log("no object cache");
                        }
                    }
                    break;
                case TouchPhase.Moved:
                    for (int k = _behaviours.Count - 1; k >= 0; k--) {
                        if (_behaviours[k] == null) {
                            _behaviours.Remove(_behaviours[k]);
                            continue;
                        }
                        _behaviours[k].Moved(_hit, touch);
                    }
                    break;
                case TouchPhase.Stationary:
                    for (int k = _behaviours.Count - 1; k >= 0; k--) {
                        if (_behaviours[k] == null) {
                            _behaviours.Remove(_behaviours[k]);
                            continue;
                        }
                        _behaviours[k].Stationary(_hit, touch);
                    }
                    break;
                case TouchPhase.Ended:
                    for (int k = _behaviours.Count - 1; k >= 0; k--) {
                        if (_behaviours[k] == null) {
                            _behaviours.Remove(_behaviours[k]);
                            continue;
                        }
                        _behaviours[k].Ended(_hit, touch);
                    }
                    break;
                case TouchPhase.Canceled:
                    for (int k = _behaviours.Count - 1; k >= 0; k--) {
                        if (_behaviours[k] == null) {
                            _behaviours.Remove(_behaviours[k]);
                            continue;
                        }
                        _behaviours[k].Canceled(_hit, touch);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public bool EnableBuildMode() {
        //Debug.Log("[INSIDE TOUCHSCRIPT!]" + !_objectsCache.BuildModeButtons.activeSelf + " | " + (_objectsCache.GeneratorCounter.AmountOfGenerators > 0));
        if (!_objectsCache.BuildModeButtons.activeSelf && _objectsCache.GeneratorCounter.AmountOfGenerators > 0) {
            //if (_objectsCache.TapHere != null) {
            //    Destroy(_objectsCache.TapHere);
            //}
            //Debug.Log("WHatever here - WOlly");
            _objectsCache.BuildModeButtons.SetActive(true);
            _objectsCache.GeneratorsLeft.SetActive(true);
            StartCoroutine(_touchPinchToZoom.ResetCamera());
            _objectsCache.EditMalone.SetActive(true);
            if (_happinessHandler != null) {
                _happinessHandler.SetActiveAreas(true);
            }
            _mode = TouchState.Build;
            return true;
        }
        return false;
    }

    private IEnumerator ToggleFadeActive(GameObject pTarget) {
        pTarget.SetActive(true);
        yield return new WaitForSeconds(_objectsCache.GeneratorCounter.SecondsToShow);
        pTarget.SetActive(false);
    }

    public void ToggleBuildMode() {
        //Debug.Log("[INSIDE TOUCHSCRIPT!] TOGGLEBUILDMODE");
        if (!EnableBuildMode()) {
            if (!_objectsCache.BuildModeButtons.activeSelf) {
                StartCoroutine(ToggleFadeActive(_objectsCache.GeneratorsLeft));
            } else {
               _objectsCache.GeneratorsLeft.SetActive(false);
            }
            _objectsCache.BuildModeButtons.SetActive(false);
            _objectsCache.EditMalone.SetActive(false);
            if (_happinessHandler != null) {
                _happinessHandler.SetActiveAreas(false);
            }

            if (_target != null) {
                ObjectState state = _target.GetComponent<ObjectState>();
                TouchConfirmBehaviour behaviour = _target.GetComponentInChildren<TouchConfirmBehaviour>(true);

                if (state.CurrentState == ObjectState.State.Preview) {
                    if (_mode == TouchState.Move) {
                        _target.transform.position = _targetOriginalPosition;
                        behaviour.ConfirmObjectPlacement(TouchState.None, true);
                        _target = null;
                    } else {
                        Destroy(_target);
                        _target = null;
                    }
                } else if (state.CurrentState == ObjectState.State.Placed) {
                    //Debug.Log(this + " | " + _mode);
                    if (_mode == TouchState.Edit) {
                        behaviour.CloseEditWindow(_target, true);
                        _mode = GetDefaultState();
                        _target = null;
                    }
                }
            }
            _mode = TouchState.None;
        }
    }

    public void SelectWindTurbine() {
        if (_target != null) {
            ObjectState state = _target.GetComponent<ObjectState>();
            TouchConfirmBehaviour behaviour = _target.GetComponentInChildren<TouchConfirmBehaviour>(true);
            if (state.CurrentState == ObjectState.State.Preview) {
                if (_mode == TouchState.Move) {
                    _target.transform.position = _targetOriginalPosition;
                    behaviour.ConfirmObjectPlacement(TouchState.Build, false);
                    _target = null;
                } else {
                    Destroy(_target);
                    _target = null;
                }
            } else if (state.CurrentState == ObjectState.State.Placed) {
                if (_mode == TouchState.Edit) {
                    behaviour.CloseEditWindow(_target, true);
                    _mode = GetDefaultState();
                    _target = null;
                }
            }
        }
        _currentPrefab = _objectsCache.WindTurbinePrefab;
    }

    public void SelectSolarPanel() {
        if (_target != null) {
            ObjectState state = _target.GetComponent<ObjectState>();
            TouchConfirmBehaviour behaviour = _target.GetComponentInChildren<TouchConfirmBehaviour>(true);

            if (state.CurrentState == ObjectState.State.Preview) {
                if (_mode == TouchState.Move) {
                    _target.transform.position = _targetOriginalPosition;
                    behaviour.ConfirmObjectPlacement(TouchState.Build, false);
                    //behaviour.CloseEditWindow(_target);
                    _target = null;
                } else {
                    Destroy(_target);
                    _target = null;
                }
            } else if (state.CurrentState == ObjectState.State.Placed) {
                if (_mode == TouchState.Edit) {
                    behaviour.CloseEditWindow(_target, true);
                    _mode = GetDefaultState();
                    _target = null;
                }
            }
        }
        _currentPrefab = _objectsCache.SolarPanelPrefab;
    }

    public TouchState GetDefaultState() {
        if (_objectsCache.BuildModeButtons.activeSelf) {
            return TouchState.Build;
        }
        return TouchState.None;
    }

    public bool HasWon() {
        if (_objectsCache.EnergyOutlineMeter.gameObject.GetComponent<EnergyIndicatorScript>().GetEnergy() >= 100) {
            return true;
        }
        //Debug.Log(_objectsCache.EnergyOutlineMeter.gameObject.GetComponent<EnergyIndicatorScript>().GetEnergy());
        return false;
    }

    public void LoadNextLevel(string pName) {
        StartCoroutine(LoadSceneAsync(pName));
    }

    private IEnumerator LoadSceneAsync(string pName) {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(pName);
        while (!asyncLoad.isDone) {
            //Debug.Log("Loading...");
            yield return null;
        }
    }

    public IEnumerator MoveTowards(Transform pTransform, float pSpeed, Vector3 pOffset) {
        Vector3 newPos = new Vector3(pTransform.transform.position.x - pOffset.x, pTransform.transform.position.y - pOffset.y, pTransform.transform.position.z - pOffset.z);

        while (pTransform.transform.position != newPos) {
            float step = pSpeed * Time.fixedDeltaTime;
            pTransform.transform.position = Vector3.MoveTowards(pTransform.transform.position, newPos, step);
            yield return null;
        }
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        //Debug.Log("Level Loaded");
        Initialize();
    }

    public void Initialize() {
        //Debug.Log("Initialize");
        InitializeVariables();
        HideText();
        DestroyPreviousObjects();

        //Debug.Log(_objectsCache + " |  " + _objectsCache.WindturbineButton +" | " + _objectsCache.WindturbineButton.GetComponent<Button>());
        AddOnClickListeners();
    }

    private void InitializeVariables() {
        _behaviours = new List<TouchInputBehaviour>();
        _target = null;
        _mode = TouchState.None;
        _objectsCache = GameObject.FindGameObjectWithTag("ObjectCache").GetComponent<SceneObjectsCache>();
        _currentPrefab = _objectsCache.WindTurbinePrefab;
        _objectsCache.EventSystem.SetSelectedGameObject(null);
        _touchPinchToZoom = GetComponent<TouchPinchToZoom>();
        
        GameObject handler = GameObject.FindGameObjectWithTag("HappinessHandler");
        if (handler != null) {
            _happinessHandler = handler.GetComponent<HappinesHandler>();
        }
    }

    private void HideText() {
        if (_objectsCache.TipText != null) {
            _objectsCache.TipText.SetActive(false);
        }
        if (_objectsCache.TapHere != null) {
            _objectsCache.TapHere.SetActive(false);
        }
    }

    private void DestroyPreviousObjects()
    {
        //Debug.Log("Destroy prev ovjects ");
        if (transform.childCount > 0) {
            for (int i = 0; i < transform.childCount; i++) {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        //Debug.Log(" After Destroy prev ovjects ");
    }

    private void AddOnClickListeners() {
        Button windturbineButton = _objectsCache.WindturbineButton.GetComponent<Button>();
        Button solarPanelButton = _objectsCache.SolarPanelButton.GetComponent<Button>();
        Button buildModeToggle = _objectsCache.BuildModeToggle.GetComponent<Button>();

        if (_objectsCache.WinScreen != null && _objectsCache.WinScreen.transform.childCount >= 2) {
            Button winScreenButton = _objectsCache.WinScreen.transform.GetChild(2).GetComponent<Button>();
            winScreenButton.onClick.AddListener(delegate { LoadNextLevel("GameplayTesting"); });
            //Debug.Log("CALLING GAMEPLAY NIQQ");
        }

        if (windturbineButton != null && solarPanelButton != null && buildModeToggle != null)
        {
            windturbineButton.onClick.AddListener(SelectWindTurbine);
            solarPanelButton.onClick.AddListener(SelectSolarPanel);
            buildModeToggle.onClick.AddListener(ToggleBuildMode);
            //Debug.Log("Added listenerd to windturbinebutton");
        } else {
            Debug.Log("[ERROR] One of: windturbine, solarpanel and buildmodetoggle buttons are null ");
        }
    }
}


