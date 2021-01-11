using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoScript : Singleton<InfoScript>
{
    [SerializeField] int _difficutly;
    [SerializeField] bool _playing;
    [SerializeField] Dictionary<string, int> _levels;
    GameObject _resolutionScreen;
    

	// Use this for initialization
	void Start () {
        Instance.name = "InfoScript";
        _levels = new Dictionary<string, int>();
    }

    public void NextLevel(string lvl)
    {
        try
        {
            _levels[lvl]++;
        }
        catch
        {
            _levels.Add(lvl, 1);
        }
        //Debug.Log(lvl + "-" +_levels[lvl]);
    }

    public int LevelReached()
    {
        return _levels.Keys.Count;
    }

    public GameObject ResolutionScreen
    {
        get { return _resolutionScreen; }
    }

    public void SetResolutionScreen(GameObject resolutionScreen)
    {
        //Debug.Log("set resolution screen");
        _resolutionScreen = resolutionScreen;
    }

    public bool Playing
    {
        get { return _playing; }
    }

    public int GetDifficulty()
    {
        return _difficutly;
    }


    public void SetDifficulty(int lvl)
    {
        _difficutly = lvl;
        _playing = true;
    }

    public void NotPlaying()
    {
        _playing = false;
        _levels = new Dictionary<string, int>();
        _difficutly = 0;
    }

	//// Update is called once per frame
	//void Update () {
		
	//}
}
