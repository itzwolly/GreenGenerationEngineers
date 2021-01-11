using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class EnergyIndicatorScript : MonoBehaviour {
    [SerializeField] float _maxLevel;

    [SerializeField] GameObject _polutionParticles;
    [SerializeField] int _maxParticles;
    [SerializeField] GameObject _polutionMalone;
    [SerializeField] float _minYMalone;
    [SerializeField] float _maxYMalone;
    [SerializeField] GameObject _vehicles;
    [SerializeField] InfoScript _infoScript;
    float _energyLevel;
    private Image _image;

    [SerializeField] GameObject _numberPrefab;
    [SerializeField] float _frequency;
    [SerializeField] bool _canCreate;
    [SerializeField] string _positiveMessage;
    [SerializeField] string _negativeMessage;
    [SerializeField] Vector3 _offset;
    float _prevLevel;
    float _dif;


    // Use this for initialization
    void Start () {
        _prevLevel = 0;
        _energyLevel = 0;
        SetEnergyLevel(0);
        _infoScript = InfoScript.Instance.GetComponent<InfoScript>();
        _image = GetComponent<Image>();
		_canCreate = true;
    }

    public float GetFillAmount() {
        return GetComponent<Image>().fillAmount;
    }

    IEnumerator CantCreate(float val)
    {
        _canCreate = false;
        yield return new WaitForSeconds(val);
        _canCreate = true;
    }

    public float GetEnergy()
    {
        return _energyLevel;
    }

    public void AddEnergy(float lvl)
    {
        _energyLevel += lvl;

        _dif = _energyLevel - _prevLevel;
        _prevLevel = _energyLevel;

        //Debug.Log(_prevLevel + " | " + _energyLevel + " | " +_dif);

        if (_canCreate && _dif != 0)
        {
            try
            {
                GameObject indicator = GameObject.Instantiate(_numberPrefab, transform.position + _offset, Quaternion.Euler(0, 0, 180), transform);
                if (_dif < 0)
                    indicator.GetComponent<TextAppear>().SetMessage(_negativeMessage);
                else if (_dif > 0)
                    indicator.GetComponent<TextAppear>().SetMessage(_positiveMessage);
            }
            catch
            {
                Debug.LogError("NO NUMBER INDICATOR ON ENERGY INDICATOR SCRIPT");
            }

            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(CantCreate(_frequency * (1 - Mathf.Abs(_dif))));
            }
        }

        if (_energyLevel < 0) {
            _energyLevel = 0;
        }
        SetEnergyLevel(_energyLevel);
        SetCityStatus(_energyLevel);
    }

    private void SetCityStatus(float lvl)
    {
        if (_vehicles != null)
        {
            int count = _vehicles.transform.childCount;
            int stepoff = (int)(count*(lvl/100));
            IEnumerator _current = _vehicles.transform.GetEnumerator();
            int i = 0;
            //Debug.Log(stepoff +" | "+count +" | "+lvl);
            //for (int i = 0; i < count; i++)
            foreach(Transform t in _vehicles.transform)
            {
                //_current.MoveNext();
                if(i<stepoff)
                {
                    t.gameObject.SetActive(false);
                }
                else
                {
                    t.gameObject.SetActive(true);
                }
                i++;
            }
        }
    }

    public void SetEnergyLevel(float lvl)
    {
        //Debug.Log(lvl);
        if (lvl <= 100 && lvl >= 0)
        {
            float amount = lvl / 100;
            //Debug.Log("Amount of green energy: " + amount);
            GetComponent<Image>().fillAmount = _maxLevel * amount;
            Vector3 pos = _polutionMalone.transform.position;
            pos.y = _maxYMalone - (_maxLevel - _minYMalone) * amount;
            _polutionMalone.transform.position = pos;
            ParticleSystem.MainModule mainMod = (_polutionParticles.GetComponent<ParticleSystem>().main);
            mainMod.maxParticles = (int) (_maxParticles * (1- amount));
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (TouchScript.Instance.HasWon())
        {
            if (_infoScript != null) {
                if (_infoScript.ResolutionScreen != null) {
                    _infoScript.ResolutionScreen.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("No resolution on infoscript");
                }
            }
            else
            {
                Debug.LogError("No info script on energy indicator");
            }
        }
	}
}
