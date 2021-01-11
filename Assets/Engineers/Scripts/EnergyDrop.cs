using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrop : MonoBehaviour
{
    [SerializeField] Vector3 _minSize;
    [SerializeField] Vector3 _maxSize;
    [SerializeField] float _errorSize;
    [SerializeField] float _changeSpeed;
    [SerializeField] float _addedProcent;
    [SerializeField] int _nrOfFlashes;
    [SerializeField] Vector3 _minFlashSize;
    [SerializeField] float _flashSpeed;

    [SerializeField] int _flashes;
    Vector3 _size;
    [SerializeField] bool _growing;
    Color _matColor;
    MeshRenderer _meshRenderer;

    ConnectionMaker _connectionMaker;

	// Use this for initialization
	void Start () {
        _size = _minSize;
        transform.localScale = (_size);
        _meshRenderer = GetComponent<MeshRenderer>();
        _growing = true;
        _matColor = _meshRenderer.material.color;
        _matColor.a = 1;
        _meshRenderer.material.color = _matColor;
        _flashes = 0;
	}

    public void Pressed()
    {
        _connectionMaker.AddEnergyPick(_addedProcent);
        Destroy(gameObject);
    }

    public void SetConnectionMaker(ConnectionMaker connectionMaker)
    {
        _connectionMaker = connectionMaker;
    }

    // Update is called once per frame
    void FixedUpdate () {
        if (_flashes < _nrOfFlashes)
        {
            if (_growing && _size.x < _maxSize.x)
            {
                if (_flashes == 0)
                {
                    _size = Vector3.Lerp(_size, _maxSize, _changeSpeed);
                }
                else
                {
                    _size = Vector3.Lerp(_size, _maxSize, _flashSpeed);
                }

                if (_maxSize.x - _size.x < _errorSize)
                {
                    _growing = false;
                    _flashes++;
                }
            }
            else if(!_growing&&_size.x>_minFlashSize.x)
            {
                _size = Vector3.Lerp(_size, _minFlashSize, _flashSpeed);
                if (_size.x-_minFlashSize.x < _errorSize)//size-error>min
                {
                    _growing = true;
                    //_flashes++;
                }
            }
            transform.localScale = (_size);
        }
        else if(!_growing)
        {
            _matColor.a = Mathf.Lerp(_matColor.a, 0, _changeSpeed);
            if (_matColor.a < _errorSize)
            {
                Destroy(gameObject);
            }
            _meshRenderer.material.color = _matColor;
        }
        
    }
}
