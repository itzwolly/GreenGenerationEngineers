using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Energy : MonoBehaviour
{
    [SerializeField] GameObject _connectionMakerPrefab;
    [SerializeField] ParticleHandler _connectionParticlesEnergy;
    [SerializeField] ParticleHandler _connectionParticlesNoEnergy;
    [SerializeField] protected float _maxGreenCreated;
    [SerializeField] protected float _tutorialMaxGreenCreated;
    [SerializeField] BlinkScript _connectionBlink;
    [SerializeField] BlinkScript _lowEnergyBlink;

    protected float _procent;
    protected bool _active;


    private CityController _city;
    protected EnvirolmentalControl _envirolment;
    private GameObject _connection;
    private ConnectionMaker _connectionMaker;
    protected bool _placed;
    //bool _sent;
    //protected bool _connected;

    public void NoConnectionStartBlinking()
    {
        _connectionBlink.StartFlashing();
        //_connectionParticles.StopParticles();
    }

    public void LowEnergyStartBlinking()
    {
        _lowEnergyBlink.StartFlashing();
    }

    public virtual void SetData()
    {
        _placed = true;
        //Debug.Log("base set Data");
    }

    public bool IsPlaced()
    {
        return _placed;
    }

    public virtual void PickUp()
    {
        _placed = false;
        //Debug.Log("base PickUp");
    }

    public Vector3 GetGeneratorPosition()
    {
        return transform.position;
    }

    public ConnectionMaker GetConnectionMaker()
    {
        return _connectionMaker;
    }

    public bool ProducesEnergy
    {
        get { return _active; }
    }

    public float GetEnergyOutput()
    {
        if (_connectionMaker == null)
            return 0;
        if (!_active && _connectionMaker.IsConnected()) {
            return 0;
        }
        return _maxGreenCreated * _procent;
    }

    public float GetPossibleEnergyOutput()
    {
        //Debug.Log(_maxGreenCreated + " | " + _procent);
        if(_procent<0.33f)
        {
            LowEnergyStartBlinking();
        }
        return _maxGreenCreated * _procent;
    }

    public float GetMaxPower()
    {
        return _maxGreenCreated;
    }

    public GameObject GetCity()
    {
        return _city.gameObject;
    }

    public void SetCity(CityController city)
    {
        _city = city;
    }

    public void SetEnvirolment(EnvirolmentalControl envirolment)
    {
        _envirolment = envirolment;
    }

    protected void CorrectParticles()
    {
        //if (_connectionMaker != null)
        //{
        //    if (_active)
        //    {
        //        _connectionMaker.SetParticles(_connectionParticlesEnergy);
        //    }
        //    else
        //    {
        //        _connectionMaker.SetParticles(_connectionParticlesNoEnergy);
        //    }
        //}
        //else
        //{
        //    Debug.Log("NO CONNECTION MAKER");
        //}
    }

    public void StartConnect()
    {
        if (_connection == null)
        {
            _connection = Instantiate(_connectionMakerPrefab, transform);
            _connectionMaker = _connection.GetComponent<ConnectionMaker>();
            _connectionMaker.SetEnvirolment(_envirolment, this);
        }
        if (_active)
            _connectionMaker.SetParticles(_connectionParticlesEnergy);
        else
            _connectionMaker.SetParticles(_connectionParticlesNoEnergy);
        _connectionMaker.MakeConnectionTo(_city.GetConnectionPoint(transform.position));
        //Debug.Log("Making Connection!");
    }
}
