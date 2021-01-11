using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateFeedbackForm : MonoBehaviour {

    [SerializeField] GameObject _dataGathering;

    public void ActivateFeedback()
    {
        if(_dataGathering == null)
        {
            Debug.Log("no feedback selected - doing nothing");
        }
        else
        {
            _dataGathering.SetActive(true);
        }
    }
}
