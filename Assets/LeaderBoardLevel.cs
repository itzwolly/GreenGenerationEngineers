using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBoardLevel : MonoBehaviour
{
    [SerializeField] GameObject _nextButtons;
    [SerializeField] ActualLeaderBoard _leaderBoard;
    [SerializeField] string _level;

    public void Click()
    {
        _leaderBoard.SetLevel(_level);
        _nextButtons.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }
}
