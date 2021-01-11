using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIStarRatingScript : MonoBehaviour {

    [SerializeField] Sprite _0Stars;
    [SerializeField] Sprite _1Stars;
    [SerializeField] Sprite _2Stars;
    [SerializeField] Sprite _3Stars;

    [SerializeField] Text _scoreText;
    [SerializeField] List<string> _goodPointsText;
    [SerializeField] List<string> _mediumPointsText;
    [SerializeField] List<string> _badPointsText;
    [SerializeField] List<string> _veryBadPointsText;

    [Range(0, 50)]
    [SerializeField] float _1StarScore;
    [Range(0, 50)]
    [SerializeField] float _2StarScore;
    [Range(0, 50)]
    [SerializeField] float _3StarScore;

    Image _stars;
    int _nr;
    private void Start()
    {
        _stars=GetComponent<Image>();
        _nr = 0;
    }

    public int GetStars()
    {
        return _nr;
    }

    public void SetStars(float score)
    {
        Debug.Log(name + " | " +score);
        if (_stars == null)
        {
            _stars = GetComponent<Image>();
        }

        if (score < _1StarScore)
        {
            _stars.sprite = _0Stars;
            if (_scoreText != null)
                _scoreText.text = _veryBadPointsText[Random.Range(0, _veryBadPointsText.Count - 1)];
        }
        else if (score < _2StarScore)
        {
            _nr = 1;
            _stars.sprite = _1Stars;
            if (_scoreText != null)
                _scoreText.text = _badPointsText[Random.Range(0, _badPointsText.Count - 1)];
        }
        else if (score < _3StarScore)
        {
            _nr = 2;
            _stars.sprite = _2Stars;
            if (_scoreText != null)
                _scoreText.text = _mediumPointsText[Random.Range(0, _mediumPointsText.Count - 1)];
        }
        else
        {
            _nr = 3;
            _stars.sprite = _3Stars;
            if (_scoreText != null)
                _scoreText.text = _goodPointsText[Random.Range(0, _goodPointsText.Count - 1)];
        }
    }
}
