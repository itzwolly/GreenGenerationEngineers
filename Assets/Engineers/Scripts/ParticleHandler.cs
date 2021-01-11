using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleHandler : MonoBehaviour {
    [SerializeField] private GameObject _target;
    [SerializeField] private float _lengthMultiplier;
    [SerializeField] private float _speed;

    private ParticleSystem.MainModule _main;
    private ParticleSystem.CollisionModule _collision;
    private ParticleSystem _pSystem;
    private ConnectionMaker _connectionMaker;
    Transform _parent;
    
    private void Start()
    {
        //transform.name = transform.name + " debug"+count;
        //Debug.Log(transform.parent + " | start");
        _parent = transform.parent;
        _pSystem = GetComponent<ParticleSystem>();
        _main = _pSystem.main;
        _collision = _pSystem.collision;
    }

    public void StartParticles(ConnectionMaker connection, GameObject target) {
        _pSystem.Play();
        _target = target;
        Debug.Log("Target name: " + _target.name);
        _connectionMaker = connection;
        _main.startLifetime = 0;
        _collision.enabled = true;
        _parent.LookAt(_target.transform);
    }

    public void StopParticles()
    {
        //Debug.Log("stop particles");
        _main.startLifetime = 0;
        _pSystem.Stop();
        _connectionMaker = null;
        _collision.enabled = false;
    }
	
	// Update is called once per frame
	private void Update() {
        //Debug.Log((_connectionMaker!=null) +" || " + (_collision.enabled));
        if (_connectionMaker != null && _connectionMaker.IsTransferingEnergy && _collision.enabled) {
            _main.startLifetime = Mathf.Lerp(_main.startLifetime.constant, _connectionMaker.GetConnectionLength() *  _lengthMultiplier,_speed);
        }
        else
        {
            _main.startLifetime = 0;
        }
    }

    private void OnParticleCollision(GameObject other) {
        if (other.layer == LayerMask.NameToLayer("City")) {
            _collision.enabled = false;
        }
    }
}
