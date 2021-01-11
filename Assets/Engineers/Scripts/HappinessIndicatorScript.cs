using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HappinessIndicatorScript : MonoBehaviour {
    [SerializeField] Image _face;

    [SerializeField] Color _happy;
    [SerializeField] Color _unhappyStart;
    [SerializeField] Color _unhappyEnd;

    [SerializeField] Sprite _happyFace;
    [SerializeField] Sprite _neutralFace;
    [SerializeField] Sprite _sadFace;
    [Range(0,100)]
    [SerializeField] float _neutralLevel;
    [Range(0, 100)]
    [SerializeField] float _sadLevel;

    float _happiness;

	// Use this for initialization
	void Start () {
        _happiness = 100;
    }

    public void SetHappinessPercent(float lvl)
    {
        if(lvl==100)
        {
            GetComponent<Image>().color = _happy;
            _face.sprite = _happyFace;
        }
        else if (lvl < 100 && lvl >= 0)
        {
            GetComponent<Image>().color = (Color.Lerp(_unhappyEnd, _unhappyStart, lvl / 100));
            if(lvl>_neutralLevel)
            {
                _face.sprite = _happyFace;
            }
            else if (lvl>_sadLevel)
            {
                _face.sprite = _neutralFace;
            }
            else
            {
                _face.sprite = _sadFace;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate () {
        //SetHappinessPercent(_happiness);
        //_happiness -= 0.1f;
	}
}
