using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnvirolmentalControl : TouchInputBehaviour
{
    [SerializeField] GameObject _polutionPanel;
    [SerializeField] float _polutionIncrease;
    [SerializeField] float _fossilFuelDecrease;
    [SerializeField] EnergyIndicatorScript _energy;
    [SerializeField] OilIndicatorScript _fossilFuel;
    [SerializeField] HappinessIndicatorScript _happiness;

    [SerializeField] float _greenToFossilMultiplier;
    [SerializeField] Vector3 _minPoint;
    [SerializeField] Vector3 _maxPoint;
    [Range(0,0.008f)]
    [SerializeField] float _cloudSpeed;
    [SerializeField] float _cloudHeight;
    [Range(0, 0.008f)]
    [SerializeField] float _windSpeed;
    [SerializeField] float _windHeight;
    [SerializeField] bool _windRotates;
    [SerializeField] float _weatherErrorDist;

    [SerializeField] float _delay;
    bool _started;
    bool _unPaused;

    List<GameObject> _winds;
    List<GameObject> _clouds;
    int _numberOfConnections;
    // Use this for initialization
    float _greenCreated;
    float _fossilFuelLeft;
    [SerializeField] InfoScript _infoScript;

    void Start ()
    {
        _unPaused = true;
        _started = false;
        StartCoroutine(DelayStart());
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        _winds = new List<GameObject>();
        _clouds = new List<GameObject>();

        _minPoint += transform.position;
        _maxPoint += transform.position;
        _greenCreated = 0;
        _fossilFuelLeft = 100;
        foreach(Transform t in transform)
        {
            if(t.tag=="Cloud")
            {
                _clouds.Add(t.gameObject);
                t.gameObject.AddComponent<WeatherMovement>();
                WeatherMovement weather = t.GetComponent<WeatherMovement>();
                weather.SetMinMax(_minPoint, _maxPoint);
                weather.SetSpeedAndDistance(_cloudSpeed, _weatherErrorDist);
                weather.ChooseMovement();
            }
            else if(t.tag=="WindArea")
            {
                _winds.Add(t.gameObject);
                t.gameObject.AddComponent<WeatherMovement>();
                WeatherMovement weather = t.GetComponent<WeatherMovement>();
                weather.SetMinMax(_minPoint, _maxPoint);
                weather.SetSpeedAndDistance(_windSpeed, _weatherErrorDist);
                weather.ChooseMovement();
            }
        }
        _infoScript = InfoScript.Instance.GetComponent<InfoScript>();
        _infoScript.NextLevel(SceneManager.GetActiveScene().name);
    }

    public void Pause()
    {
        _unPaused = false;
    }

    public void Unpause()
    {
        _unPaused = true;
    }

    public float GetFossilsLeft()
    {
        return _fossilFuelLeft;
    }

    public void ForceStart()
    {
        _started = true;
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(_delay);
        _started = true;
    }

    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        if (pHit.transform.tag == "EnergyDrop")
        {
            //Debug.Log("pressed energy drop");
            pHit.transform.gameObject.GetComponent<EnergyDrop>().Pressed();
        }
    }

    public void ConnectedToCity(int nr)
    {
        _numberOfConnections += nr;
    }

    public void GreenCreated(float val)
    {
        //Debug.Log("receiving green");
        _greenCreated += val;
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (_started && _unPaused)
        {
            //Debug.Log(_greenCreated * _greenToFossilMultiplier);
            
            if (_greenCreated < 100)
            {
                //Debug.Log("EC Individual: " + _greenCreated + " | " + _greenToFossilMultiplier + " | " + _polutionIncrease);
                //Debug.Log("EC Formula: " + (_greenCreated * _greenToFossilMultiplier - _polutionIncrease));
                _greenCreated += _greenCreated * _greenToFossilMultiplier - _polutionIncrease;

                //Debug.Log("EC GreenCreated" + _greenCreated);

                _energy.AddEnergy(_greenCreated);
            }

            //Debug.Log(_greenCreated);

            if (_fossilFuelDecrease > 0)
            {
                //Debug.Log("Individual: " + _fossilFuelDecrease + " | " + _greenCreated + " | " + _greenToFossilMultiplier);
                //Debug.Log("Combined: " + (_fossilFuelDecrease - _greenCreated * _greenToFossilMultiplier));

                _fossilFuelLeft -= Mathf.Max(_fossilFuelDecrease - _greenCreated * _greenToFossilMultiplier, 0);
                _fossilFuel.SetOilPercentage(_fossilFuelLeft);
            }
            if(_fossilFuelLeft<= _weatherErrorDist)
            {
                try
                {
                    _infoScript.ResolutionScreen.SetActive(true);
                }
                catch
                {
                    Debug.Log("no resolution");
                }
            }
            _greenCreated = 0;
        }
    }
}
