using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeedbackChanger : MonoBehaviour {
    [SerializeField] Energy _energy;
    [SerializeField] Slider _energySilder;
    [SerializeField] HappinessIndicatorScript _happinessIndicator;
    [SerializeField] HappinesHandler _happinesHandler;
    // Use this for initialization
    void Start () {
        _happinesHandler = GameObject.FindGameObjectWithTag("HappinessHandler").GetComponent<HappinesHandler>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log(_energy.GetEnergyOutput() / _energy.GetMaxPower());
        _energySilder.value = _energy.GetPossibleEnergyOutput() / _energy.GetMaxPower();
        //Debug.Log(_happinesHandler.HappinessForGenerator(_energy.gameObject) / 100);
        if (_energy.IsPlaced())
        {
            _happinessIndicator.SetHappinessPercent(100 - _happinesHandler.HappinessForGenerator(_energy.gameObject));
        }
        else
        {
            if (_happinesHandler.CheckPos(_energy.gameObject.transform.position))
            {
                _happinessIndicator.SetHappinessPercent(0);
            }
            else
            {
                _happinessIndicator.SetHappinessPercent(100);
            }
        }
	}
}
