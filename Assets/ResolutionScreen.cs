using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionScreen : MonoBehaviour {

    [SerializeField] CityFeedback _city;
    [SerializeField] UIStarRatingScript[] _stars;
    // Use this for initialization
    //[SerializeField] HappinesHandler _worldHappinesIndicator;
    //[SerializeField] EnvirolmentalControl _fossilIndicator;
    //[SerializeField] EnergyIndicatorScript _energyIndicator;


    void Start ()
    {
        gameObject.SetActive(false);
        InfoScript.Instance.GetComponent<InfoScript>().SetResolutionScreen(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
    }

    private void OnEnable()
    {
        //_fossilIndicator.Pause();
        InfoScript.Instance.GetComponent<InfoScript>().SetResolutionScreen(gameObject);
        _city.EnvirolmentalControl.Pause();
        _stars[0].SetStars(_city.HappinesHandler.GetTotalHappiness()/2);
        _stars[1].SetStars(TouchScript.Instance.ObjectsCache.EnergyOutlineMeter.gameObject.GetComponent<EnergyIndicatorScript>().GetEnergy()/2);
        _stars[3].SetStars(_city.EnvirolmentalControl.GetFossilsLeft()/2);
        int nr = (_stars[0].GetStars() + _stars[1].GetStars() + _stars[3].GetStars());
        if (nr == 0)
            _stars[2].SetStars(0);
        else if (nr < 5)
            _stars[2].SetStars(13);
        else if (nr < 9)
            _stars[2].SetStars(30);
        else
            _stars[2].SetStars(50);
    }
}
