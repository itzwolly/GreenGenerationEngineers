using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderBoardData : MonoBehaviour {

    [SerializeField] Text _name;
    [SerializeField] Text _score;
    [SerializeField] UIStarRatingScript _stars;

    public void SetData(KeyValuePair<string,float> values)
    {
        _name.text = values.Key;
        _score.text = values.Value.ToString();
        _stars.SetStars(values.Value);
    }
}
