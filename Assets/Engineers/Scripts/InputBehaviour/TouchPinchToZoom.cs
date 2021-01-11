using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class TouchPinchToZoom : MonoBehaviour {
    public enum ZoomState {
        None,
        ZoomedOut,
        ZoomedIn
    }

    [SerializeField] private float _perspectiveZoomSpeed = 0.5f;        // The rate of change of the field of view in perspective mode.
    [SerializeField] private float _minFov;
    [SerializeField] private float _maxFov;

    private CinemachineVirtualCamera _camera;
    private Camera _preprocessCamera;
    private ZoomState _zoomState;
    private Vector3 _mainCameraCenter;
    private Vector3 _preprocessCameraCenter;

    public ZoomState ZoomingState {
        get { return _zoomState; }
    }

    private void Update() {
        //Debug.Log("Mode equals: " + TouchScript.Instance.CurrentMode);
        if (SceneManager.GetActiveScene().name != "MainMenu" && SceneManager.GetActiveScene().name != "LeaderBoards") {
            if (TouchScript.Instance.CurrentMode == TouchScript.TouchState.None) {
                //Debug.Log("Touch count: " + Input.touchCount);
                if (Input.touchCount == 2) {
                    Touch touchZero = Input.GetTouch(0);
                    Touch touchOne = Input.GetTouch(1);

                    Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                    Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

                    float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
                    float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

                    float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;
                    float result = deltaMagnitudeDiff * _perspectiveZoomSpeed * Time.deltaTime;

                    _camera.m_Lens.FieldOfView += result;
                    _preprocessCamera.fieldOfView += result;

                    if (result > 0) {
                        LerpCameraToOriginalPosition();
                    }
                    _camera.m_Lens.FieldOfView = Mathf.Clamp(_camera.m_Lens.FieldOfView, _minFov, _maxFov);
                    _preprocessCamera.fieldOfView = Mathf.Clamp(_preprocessCamera.fieldOfView, _minFov, _maxFov);
                }
            }
        }
    }

    private void LerpCameraToOriginalPosition() {
        // Lerp camera position back to the center positon when zooming out.
        float mainNormalizedClamped = Mathf.Clamp01((_camera.m_Lens.FieldOfView + 0.01f - _minFov) / (_maxFov - _minFov));
        float preprocessNormalizedClamped = Mathf.Clamp01((_preprocessCamera.fieldOfView + 0.01f - _minFov) / (_maxFov - _minFov));


        _camera.transform.position = Vector3.Lerp(_camera.transform.position, _mainCameraCenter, mainNormalizedClamped);
        _camera.m_Lens.FieldOfView = Mathf.Lerp(_camera.m_Lens.FieldOfView, _maxFov, mainNormalizedClamped);

        _preprocessCamera.transform.position = Vector3.Lerp(_preprocessCamera.transform.position, _preprocessCameraCenter, preprocessNormalizedClamped);
        _preprocessCamera.fieldOfView = Mathf.Lerp(_preprocessCamera.fieldOfView, _maxFov, preprocessNormalizedClamped);
    }

    public IEnumerator ResetCamera() {
        if (_camera != null) {
            while (_camera.transform.position != _mainCameraCenter || _camera.m_Lens.FieldOfView != _maxFov) {
                LerpCameraToOriginalPosition();
                yield return null;
            }
        }
        yield return null;
    }

    private void ChangeState(ZoomState pState) {
        if (_zoomState != pState) {
            _zoomState = pState;
        }
    }

    void OnEnable() {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    void OnDisable() {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode) {
        Debug.Log("Level Loaded");
        _zoomState = ZoomState.None;
        _camera = TouchScript.Instance.ObjectsCache.CameraToZoom;
        Debug.Log("Camera equals to: " + _camera);
        if (_camera != null)
        {
            _mainCameraCenter = _camera.transform.position;
        }
        
        _preprocessCamera = TouchScript.Instance.ObjectsCache.PreprocessCamera;
        if (_preprocessCamera != null)
        {
            _preprocessCameraCenter = _preprocessCamera.transform.position;
        }
        
    }
}
