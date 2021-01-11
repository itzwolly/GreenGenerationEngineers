using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class SceneObjectsCache : MonoBehaviour {
    public GameObject SolarPanelPrefab;
    public GameObject WindTurbinePrefab;

    public GameObject BuildModeToggle;
    public GameObject BuildModeButtons;
    public GameObject WindturbineButton;
    public GameObject SolarPanelButton;

    public Text WorldPositionText;
    public Text RaycastHitText;
    public EventSystem EventSystem;
    public LayerMask LayerMask;
    public GeneratorCounter GeneratorCounter;
    public GameObject TipText;
    public Image EnergyOutlineMeter;

    public GameObject WinScreen;
    public GameObject TapHere;
    public CinemachineVirtualCamera CameraToZoom;
    public Camera PreprocessCamera;
    public GameObject GeneratorsLeft;
    public GameObject EditMalone;
    public InactiveScript WaitCanvas;
}
