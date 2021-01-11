using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCityScript : ObjectClickScript
{

    [SerializeField] Vector3 _offset;
    [SerializeField] EarthMenu _earth;
    Camera _main;
    bool _placed;
    bool _activeButton;

    Animator City;
    private Renderer _renderer;

    public GameObject GetGameObject()
    {
        return gameObject;
    }

    public bool IsPlaced()
    {
        return _placed;
    }


    protected override void Activate()
    {
        base.Activate();
        _earth.Selected(this);
        _activeButton = true;
        //Debug.Log("menucity");
    }

    public override void Deactivate()
    {
        if (_activeButton)
        {
            base.Deactivate();
            _earth.Deselected();
            Material mymat = GetComponent<Renderer>().material;
            mymat.SetColor("_EmissionColor", Color.black);
            _activeButton = false;
        }
        //Debug.Log("menucity");
    }

    // Use this for initialization
    void Start ()
    {
        if (_earth == null)
            _earth = GameObject.FindGameObjectWithTag("Earth").GetComponent<EarthMenu>();
        TouchScript.Instance.Behaviours.Add(this); ;
        _feedbackPanel.SetActive(false);
        _placed = false;
        _main = Camera.main;
        _activeButton = false;
        _renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (_activeButton) {
            Material mymat = _renderer.material;
            mymat.SetColor("_EmissionColor", Color.Lerp(Color.black, Color.blue, Mathf.PingPong(Time.time, 1)));
        }

        //Debug.Log(name);

        _feedbackPanel.transform.position = transform.position + _offset;
        //_canvas.transform.rotation = Quaternion.LookRotation(_main.transform.forward,_main.transform.up);
    }
}
