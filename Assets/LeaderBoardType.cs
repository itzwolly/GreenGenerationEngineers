using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardType : MonoBehaviour {
    [SerializeField] GameObject _nextButtons;
    [SerializeField] ActualLeaderBoard _leaderBoard;
    [SerializeField] int _type;

    public void Click()
    {
        //Debug.Log("type clicked");
        _leaderBoard.SetType(_type);
        if (_type != 1)
            _nextButtons.SetActive(true);
        else
            _nextButtons.SetActive(false);
        //Debug.Log("set btuton");
        transform.parent.gameObject.SetActive(false);
    }
}
