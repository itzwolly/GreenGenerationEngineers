using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[RequireComponent(typeof(Text))]
public class GeneratorCounter : MonoBehaviour {
    [SerializeField] private Text _generatorCounter;
    [SerializeField] private string _prefix;
    [SerializeField] private string _postfix;
    [SerializeField] private int _maxGenerators;
    [SerializeField] private float _secondsToShow;
    
    private int _amountOfGenerators;

    public int AmountOfGenerators {
        get { return _amountOfGenerators; }
    }
    public float SecondsToShow {
        get { return _secondsToShow; }
    }

    // Use this for initialization
    private void Start () {
        _amountOfGenerators = _maxGenerators;
        SetText(_prefix + _amountOfGenerators + _postfix);
    }

    private void SetText(string pAmount) {
        _generatorCounter.text = pAmount.ToString();
    }

    public void ReduceGeneratorCounter(int pAmount) {
        if (HasGenerators()) {
            _amountOfGenerators -= pAmount;
            SetText(_prefix + _amountOfGenerators + _postfix);

            if (_amountOfGenerators == 0) {
                TouchScript.Instance.ToggleBuildMode();
            }
        }
    }

    public void IncreaseGeneratorCounter(int pAmount)
    {
        
            _amountOfGenerators += pAmount;
            SetText(_prefix + _amountOfGenerators + _postfix);
       
    }

    public bool HasGenerators() {
        return _amountOfGenerators > 0;
    }
}
