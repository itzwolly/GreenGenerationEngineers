using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityFeedback : ObjectClickScript
{
    //[SerializeField] Slider _fossilSlider;
    //[SerializeField] Slider _energySlider;
    //[SerializeField] HappinessIndicatorScript _happinesIndicator;
    [SerializeField] UIStarRatingScript _stars;

    [SerializeField] float _importanceHapiness;
    [SerializeField] float _importanceFossilLeft;
    [SerializeField] float _importanceEnergytotal;

    [SerializeField] HappinesHandler _worldHappinesIndicator;
    [SerializeField] EnvirolmentalControl _fossilIndicator;
    [SerializeField] EnergyIndicatorScript _energyIndicator;
    float _score;

    public EnvirolmentalControl EnvirolmentalControl
    {
        get { return _fossilIndicator; }
    }

    public HappinesHandler HappinesHandler
    {
        get { return _worldHappinesIndicator; }
    }

    public EnergyIndicatorScript EnergyIndicatorScript
    {
        get { return _energyIndicator; }
    }

    public float GetScore()
    {
        return _score;
    }

    // Use this for initialization
    void Start ()
    {
        _feedbackPanel.SetActive(false);
        //_worldHappinesIndicator.GetTotalHappiness();
        //_fossilIndicator.GetFossilsLeft();
        //_energyIndicator.GetEnergy();
    }

    // Update is called once per frame
    protected override void Update () {
        //base.Update();//if we want the pannel to appear slowly
        float energy = TouchScript.Instance.ObjectsCache.EnergyOutlineMeter.GetComponent<EnergyIndicatorScript>().GetEnergy();
        float fossilLeft = _fossilIndicator.GetFossilsLeft();
        float happiness= _worldHappinesIndicator.GetTotalHappiness();
        //_happinesIndicator.SetHappinessPercent(happiness);
        //_fossilSlider.value = fossilLeft / 100;
        //_energySlider.value = energy /100;
        _score = (energy * _importanceEnergytotal + fossilLeft * _importanceFossilLeft + happiness * _importanceHapiness) / 6;
        //Debug.Log(score);
        _stars.SetStars(_score);
	}
}
