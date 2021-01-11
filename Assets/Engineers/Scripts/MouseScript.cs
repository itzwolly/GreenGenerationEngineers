using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class MouseScript : MonoBehaviour {

    // Use this for initialization
    [SerializeField] GameObject _solarPanelPrefab;
    [SerializeField] GameObject _windTurbinePrefab;
    [SerializeField] GameObject _batteryPrefab;
    [SerializeField] int _maxAllowedGenerators;
    [SerializeField] Transform _structures;
    [SerializeField] float _moveSpeed = 0.1f;
    [SerializeField] EnvirolmentalControl _envirolmentalControl;
    [SerializeField] CityController _cityController;
    [SerializeField] HappinesHandler _happinesHandler;
    [SerializeField] Text _generatorCounter;

    GameObject _toPlaceObj;
    [SerializeField] bool _moving;
    Vector3 _offset;
    Vector3 _initialPos;
    int _placedGenerators;
    void Start () {
        _toPlaceObj = null;
        _placedGenerators = 0;
        _generatorCounter.text = (_maxAllowedGenerators - _placedGenerators).ToString();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_toPlaceObj != null)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Ground")
                {
                    Vector3 pos = _toPlaceObj.transform.position;
                    // _toPlaceObj.GetComponent<Rigidbody>().AddForce(pos*1000);
                    _toPlaceObj.transform.position = Vector3.Lerp(pos, hit.point + _offset, _moveSpeed);
                }
                // Do something with the object that was hit by the raycast.
            }

            if (hit.transform!=null && Input.GetMouseButtonDown(0))
            {
                if (hit.transform.tag == "Ground")
                {
                    //Debug.Log("placing");
                    PlaceSelected();
                }

            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.tag == "Moveable")
                {
                    if (!_moving)
                    {
                        _moving = true;
                        ConnectionMaker connect = hit.transform.gameObject.GetComponent<Energy>().GetConnectionMaker();
                        connect.DisconnectConnection();
                    }
                    else
                    {
                        _toPlaceObj.transform.position = _initialPos;
                        
                        ConnectionMaker connection = _toPlaceObj.GetComponent<Energy>().GetConnectionMaker();
                        connection.InstantDisconnectConnection();
                        connection.MakeInstantConnection(connection.GetConnectLocation());
                        Debug.Log("Made instant connection");
                    }
                    _initialPos = hit.transform.position;
                    _toPlaceObj = hit.transform.gameObject;
                    //_happinesHandler.RemoveGenerator(_toPlaceObj);
                    PreviewObjectPosition();
                    _toPlaceObj.GetComponent<Energy>().PickUp();
                }
            }
        }
    }

    public void SelectSolarPanel()
    {
        if (_placedGenerators < _maxAllowedGenerators)
        {
            if (_toPlaceObj == null)
            {
                _placedGenerators++;
                _generatorCounter.text = (_maxAllowedGenerators - _placedGenerators).ToString();
            }
            if (!_moving)
            {
                Destroy(_toPlaceObj);
                _toPlaceObj = Instantiate(_solarPanelPrefab);
                _toPlaceObj.transform.SetParent(transform);
                _moving = true;
                PreviewObjectPosition();
            }
        }
    }

    public void SelectWindTurbine()
    {
        if (_placedGenerators < _maxAllowedGenerators)
        {
            if (_toPlaceObj == null)
            {
                _placedGenerators++;
                _generatorCounter.text = (_maxAllowedGenerators - _placedGenerators).ToString();
            }
            if (!_moving)
            {
                Destroy(_toPlaceObj);
                _toPlaceObj = Instantiate(_windTurbinePrefab);
                _toPlaceObj.transform.SetParent(transform);
                _moving = true;
                PreviewObjectPosition();
            }
        }
    }

    public void SelectBattery()
    {
        if (!_moving)
        {
            Destroy(_toPlaceObj);
            _toPlaceObj = Instantiate(_batteryPrefab);
            _toPlaceObj.transform.SetParent(transform);
            _moving = true;
            PreviewObjectPosition();
        }
    }

    void PlaceSelected()
    {
        _happinesHandler.PlacedGenerator(_toPlaceObj);
        _toPlaceObj.layer = 0;
        Color mat = _toPlaceObj.GetComponent<MeshRenderer>().material.color;
        mat.a = 255f;
        _toPlaceObj.GetComponent<MeshRenderer>().material.color = mat;
        _toPlaceObj.GetComponent<Energy>().SetCity(_cityController);
        _toPlaceObj.GetComponent<Energy>().SetEnvirolment(_envirolmentalControl);
        _toPlaceObj.GetComponent<Energy>().StartConnect();
        _toPlaceObj.GetComponent<Energy>().SetData();
        _toPlaceObj = null;
        _moving = false;
    }

    void PreviewObjectPosition()
    {
        _offset = _toPlaceObj.transform.position;
        _offset.x = 0;
        _offset.z = 0;
        Color mat = _toPlaceObj.GetComponent<MeshRenderer>().material.color;
        mat.a = 100f;
        _toPlaceObj.GetComponent<MeshRenderer>().material.color = mat;
        _toPlaceObj.layer = 2;
    }
}
