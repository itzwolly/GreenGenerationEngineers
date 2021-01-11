using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionMaker : MonoBehaviour {

    [Range(0,1)]
    [SerializeField] float _percentageLossPerConnection;
    [SerializeField] GameObject _dotPrefab;
    [SerializeField] float _dotsDistance;
    [SerializeField] float _timePerDot;
    [SerializeField] float _timePerDotRemoval;


    [SerializeField] Material _normalConnection;
    [SerializeField] Material _highlightedConnection;
    [SerializeField] float _timePerEnergyPulse;
    int _pulsePos;

    [SerializeField] GameObject _dropPrefab;
    [SerializeField] float _cooldownOfDrop;
    int _dropPos;
    [SerializeField] float _pickedUpEnergyMultiplier;
    GameObject _droppedEnergy;
    bool _canDrop;

    bool _makingConnection;
    bool _removingConnection;
    bool _waitingForRemoval;
    bool _recentlyDestroyedConnection;
    bool _energyActive;

    List<GameObject> _conections;
    bool _connected;
    Vector3 _whereTo;
    Vector3 _whereFrom;
    Vector3 _direction;
    Energy _energy;
    EnvirolmentalControl _envirolment;
    int _connectionLenght;
    ParticleHandler _connectionParticles;

    List<GameObject>.Enumerator _energyPulse;
	// Use this for initialization
	void Start () {        
        _conections = new List<GameObject>();
        _makingConnection = false;
        _connectionLenght = 0;
        _pickedUpEnergyMultiplier = 1;
    }

    public void SetParticles(ParticleHandler particles)
    {
        if(_connectionParticles!=null) 
            _connectionParticles.StopParticles();
        _connectionParticles = particles;
    }

    public bool IsTransferingEnergy
    {
        get { return _energy.GetEnergyOutput()>0; }
    }

    public float GetConnectionLength()
    {
        if(_conections.Count==0)
        {
            return 0;
        }
        return Vector3.Distance(_conections[0].transform.position, _conections[_conections.Count-1].transform.position);
    }

    public float GetTotalEnergyLoss()
    {
        return Mathf.Min(_connectionLenght * _percentageLossPerConnection, 1);
    }

    public void DestroyConnection()
    {
        //Debug.Log("instantly destroy connection");
        _energy.NoConnectionStartBlinking();
        _envirolment.ConnectedToCity(-1);
        _connectionLenght = 0;
        _connected = false;
        _recentlyDestroyedConnection = true;
        int size = _conections.Count;
        for(int i =0;i<size;i++)
        {
            GameObject obj = _conections[_conections.Count - 1];
            _conections.RemoveAt(_conections.Count - 1);
            Destroy(obj);
        }
        _connectionParticles.StopParticles();
    }
    public void AddEnergyPick(float val)
    {
        StartCoroutine(MoveEnergy(val));
    }

    private IEnumerator MoveEnergy(float val)
    {
        _pickedUpEnergyMultiplier += val;
        yield return new WaitForSeconds(_timePerEnergyPulse);

        _pickedUpEnergyMultiplier -= val;
    }
    
    public Vector3 GetConnectLocation()
    {
        return _whereTo;
    }

    public void SetEnvirolment(EnvirolmentalControl envirolment, Energy energy)
    {
        _envirolment = envirolment;
        _energy = energy;
    }

    public bool IsConnected()
    {
        return _connected;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_connected && _energy.ProducesEnergy)
        {
            float energyOutput = _energy.GetEnergyOutput() * (1 - GetTotalEnergyLoss());
            //Debug.Log("ConnectionMaker: " + _energy.GetEnergyOutput() + " | " + (1 - GetTotalEnergyLoss()));
            //Debug.Log("energy output = " + energyOutput);
            _envirolment.GreenCreated(energyOutput * _pickedUpEnergyMultiplier);
        }
    }

    public void DisconnectConnection()
    {
        _envirolment.ConnectedToCity(-1);
        _removingConnection = true;
        transform.SetParent(null);
        _connected = false;
        _connectionLenght = 0;
        _recentlyDestroyedConnection = true;
        //_envirolment.GreenCreated(-_energy.GetEnergyOutput());
        StartCoroutine(ContinueDisconnect());
        _connectionParticles.StopParticles();
    }

    public void InstantDisconnectConnection()
    {
        int size = _conections.Count;
        for(int i=0;i<size;i++)
        {
            GameObject obj = _conections[0];
            Destroy(obj);
            _conections.RemoveAt(0);
        }
        //_envirolment.GreenCreated(-_energy.GetEnergyOutput());
        _envirolment.ConnectedToCity(-1);
        _connectionLenght = 0;
        _connected = false;
        _recentlyDestroyedConnection = true;
        _connectionParticles.StopParticles();
    }

    public void MakeConnectionTo(Vector3 to)
    {
        _connectionLenght = 0;
        if (_conections == null)
            _conections = new List<GameObject>();
        transform.SetParent(_energy.gameObject.transform);
        _makingConnection = true;
        _whereTo = to;
        //_conections.Add(Instantiate(_dotPrefab,to,Quaternion.Euler(0,0,0),transform));
        _whereFrom = _energy.GetGeneratorPosition();
        _whereFrom.y = to.y;
        Vector3 dir = to - _whereFrom;
        dir.Normalize();
        _direction = dir;
        StartCoroutine(HandlePlacement());
        _connectionParticles.StartParticles(this, _energy.GetCity());
    }

    public void MakeInstantConnection(Vector3 to)
    {
        _connectionLenght = 0;
        if (_conections == null)
            _conections = new List<GameObject>();
        _whereTo = to;
        //_conections.Add(Instantiate(_dotPrefab,to,Quaternion.Euler(0,0,0),transform));
        Vector3 posi = transform.position;
        posi.y = to.y;
        Vector3 dir = to - posi;
        dir.Normalize();
        while((posi - _whereTo).sqrMagnitude > (_dotsDistance * _dotsDistance))
        {
            _connectionLenght++;
            _conections.Add(Instantiate(_dotPrefab, posi, Quaternion.Euler(0, 0, 0)));
            posi = posi + dir * _dotsDistance;
        }
        StartEnergyTransfer();
        _envirolment.ConnectedToCity(1);
        _connected = true;
        _connectionParticles.StartParticles(this, _energy.GetCity());
        //_envirolment.GreenCreated(_energy.GetEnergyOutput());
    }

    private IEnumerator HandlePlacement()
    {
        if(_waitingForRemoval)
        {
            //Debug.Log("waiting for removal");
        }
        else if (_removingConnection && _makingConnection)
        {
            _waitingForRemoval = true;
            yield return new WaitForSeconds(_timePerDotRemoval*_conections.Count);
            _waitingForRemoval = false;
            //StopAllCoroutines();
            StartCoroutine(HandlePlacement());
        }
        else
        {
            //Debug.Log("Placing");
            yield return new WaitForSeconds(_timePerDot);
            if ((_whereFrom - _whereTo).sqrMagnitude > (_dotsDistance * _dotsDistance))
            {
                //Debug.Log("making connection");
                GameObject con = Instantiate(_dotPrefab, _whereFrom, Quaternion.Euler(0, 0, 0));
                con.transform.parent = transform;
                _conections.Add(con);
                _connectionLenght++;
                _whereFrom += _direction * _dotsDistance;
                StartCoroutine(HandlePlacement());
            }
            else
            {
                _connected = true;
                //Debug.Log("finished connecting");
                _recentlyDestroyedConnection = false;
                _envirolment.ConnectedToCity(1);
                StartEnergyTransfer();
                //_envirolment.GreenCreated(_energy.GetEnergyOutput());
                _makingConnection = false;
            }
        }
    }

    private IEnumerator ContinueDisconnect()
    {
        yield return new WaitForSeconds(_timePerDotRemoval);
        int size = _conections.Count;
        if (size > 0)
        {
            //Debug.Log("removing connection");
            GameObject obj = _conections[0];
            Destroy(obj);
            _conections.RemoveAt(0);
            StartCoroutine(ContinueDisconnect());
        }
        else
        {
            _recentlyDestroyedConnection = true;
            _removingConnection = false;
        }
        
    }

    private void StartEnergyTransfer()
    {
        //_conections.Add(new GameObject());
        _energyPulse = _conections.GetEnumerator();
        _energyPulse.MoveNext();
        _dropPos = Random.Range(0,_conections.Count-1);
        _pulsePos = 0;
        _canDrop = true;
        //Debug.Log("rutine started");
        StartCoroutine(MoveEnergy());
    }

    private IEnumerator MoveEnergy()
    {
        //Debug.Log("waiting for energy");
        yield return new WaitForSeconds(_timePerEnergyPulse);
        int count = _conections.Count;
        if (count>0 && _connected && !_recentlyDestroyedConnection && _energyPulse.Current != null)
        {
            if (_energyPulse.Current == _conections[count - 1])
            {
                _energyPulse.Current.GetComponent<MeshRenderer>().material = _normalConnection;
                _energyPulse = _conections.GetEnumerator();
                _energyPulse.MoveNext();//since get enumerator returns before first item in list
                _dropPos = Random.Range(0, _conections.Count - 1);
                _pulsePos = 0;
            }
            _energyPulse.Current.GetComponent<MeshRenderer>().material = _normalConnection;
            _energyPulse.MoveNext();
            _energyPulse.Current.GetComponent<MeshRenderer>().material = _highlightedConnection;
            _energyActive = true;
            if(_energy.ProducesEnergy&&_canDrop && _pulsePos==_dropPos)
            {
               _droppedEnergy = Instantiate(_dropPrefab, _energyPulse.Current.transform.position, Quaternion.Euler(0, 0, 0), transform);
                _droppedEnergy.GetComponent<EnergyDrop>().SetConnectionMaker(this);
                StartCoroutine(ActivateCooldown());
            }
            _pulsePos++;
            StartCoroutine(MoveEnergy());
        }
        else
        {
            //Debug.Log("rutine stopped");
            //_recentlyDestroyed = false;
            yield return new WaitForSeconds(_timePerDotRemoval * count + 0.1f);
            //StartEnergyTransfer();
        }
    }

    private IEnumerator ActivateCooldown()
    {
        //Debug.Log("can't place");
        _canDrop = false;
        yield return new WaitForSeconds(_cooldownOfDrop);
        //Debug.Log("can place");
        _canDrop = true;
    }
}
