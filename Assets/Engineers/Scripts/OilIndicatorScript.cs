using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OilIndicatorScript : MonoBehaviour {
    [SerializeField] float _maxLevel;
    [SerializeField] float _minLevel;
    [SerializeField] GameObject _level;
    [SerializeField] Text _percentageText;

    float _oilLevel;

	// Use this for initialization
	void Start () {
        _oilLevel = 100;
	}

    //public void ChangeOilLevel(float lvl)
    //{
    //}

    public void SetOilPercentage(float lvl)
    {
        Vector3 pos = _level.transform.localPosition;
        //Debug.Log(this + " level equals to: " + lvl);

        if (lvl < 100 && lvl > 0)
        {
            //Debug.Log("oil percentage" + lvl);
            pos.y = _minLevel + (_maxLevel - _minLevel) / 100 * lvl;
            _level.transform.localPosition = pos;
            _percentageText.text = ((int) lvl).ToString() + "%";
        }
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //SetOilPercentage(_oilLevel);
        //_oilLevel -= 0.1f;
	}
}
