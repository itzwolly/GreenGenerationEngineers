using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EarthMenu : MonoBehaviour {

    [SerializeField] float _idleRotationSpeed;
    [SerializeField] Vector3 _idleRotationAxis;

    [SerializeField] List<MenuCityScript> _levels;

    Collider _collider;
    float _rotationSpeed;

    [SerializeField] float _turnToSpeed = 1;
    [SerializeField] float _minError=50f;
    bool _rotating;
    MenuCityScript _cityPosition;

    public bool Rotating
    {
        get { return _rotating; }
    }

    private void Awake() {
        GameObject.FindGameObjectWithTag("TouchScript").GetComponent<TouchScript>().Initialize();
    }

    // Use this for initialization
    void Start () {
        _rotationSpeed = _idleRotationSpeed;
        _collider = gameObject.GetComponent<SphereCollider>();
    }

    //public float DistanceToLine(Ray ray, Vector3 point)
    //{
    //    return Vector3.Cross(ray.direction, point - ray.origin).magnitude;
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(_idleRotationAxis, _rotationSpeed);

        if(_cityPosition!=null)
        {
            Vector2 midScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            Camera cam = Camera.main;
            Vector3 pos = _cityPosition.transform.position;
            Vector2 posScreen = cam.WorldToScreenPoint(pos);
            float dist = (midScreen - posScreen).magnitude;
            //Debug.Log(posScreen + " | " + midScreen + " | " + dist);
            if (dist<_minError)
            {
                _cityPosition = null;
                _rotating = false;
            }
            else
            {
                GetComponent<EarthRotate>().RotateEarth((posScreen.x - midScreen.x) * _turnToSpeed, (posScreen.y - midScreen.y) * _turnToSpeed);
            }
        }
        else
        {
            //Debug.Log("noi city selecteds");
        }
    }

    public void Selected(MenuCityScript cityPosition)
    {
        _cityPosition = cityPosition;
        //Debug.Log("cirty si selected");
        //gameObject.GetComponent<SphereCollider>().enabled = false;
        _rotationSpeed = 0;
        _rotating = true;
        //StartCoroutine(LoadNewScene(hit.transform.gameObject.GetComponent<MenuCityScript>().GetSceneName()));
    }

    public void Deselected()
    {
        _rotating = false;
        //Debug.Log("cirty si deselected");
        //gameObject.GetComponent<SphereCollider>().enabled = true;
        _rotationSpeed = _idleRotationSpeed;
    }

    //void CheckClick()
    //{
    //    //if (Input.GetMouseButtonDown(0))
    //    //{
    //    //    RaycastHit hit;
    //    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    //    if (Physics.Raycast(ray, out hit))
    //    //    {
    //    //        if (hit.transform.tag=="City")
    //    //        {

    //    //            _loadingScreen.SetActive(true);

    //    //            // ...and start a coroutine that will load the desired scene.
    //    //            StartCoroutine(LoadNewScene(hit.transform.gameObject.GetComponent<MenuCityScript>().GetSceneName()));
    //    //        }
    //    //    }
    //    //    Debug.Log(hit.transform.tag);
    //    //}
    //}

    private void OnValidate()
    {
        if (Application.isEditor)
        {
            int size = _levels.Count;
            List<MenuCityScript>.Enumerator enumerator = _levels.GetEnumerator();
            for (int i = 0; i < size; i++)
            {
                enumerator.MoveNext();
                if (enumerator.Current.IsPlaced())
                {
                    continue;
                }
                GameObject city = enumerator.Current.GetGameObject();

                Vector3 cityPos = city.transform.position;
                Vector3 earthPos = transform.position;



                RaycastHit hit;
                Ray ray = new Ray(cityPos, earthPos - cityPos);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.tag == "Earth")
                    {
                        city.transform.SetParent(transform);
                        //Debug.Log("will be moved");
                        city.transform.position = hit.point;
                        // This ensures that A's down points at B's position
                        Quaternion rotation = Quaternion.FromToRotation(transform.up, hit.normal);
                        // Point straight towards B with secondary emphasis on the root's forward vector
                        // Then, apply offset to align the downward axis instead
                        city.transform.rotation = rotation;
                    }
                }
            }
        
        }
    }

}
