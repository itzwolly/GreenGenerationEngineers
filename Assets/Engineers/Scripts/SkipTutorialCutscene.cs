using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SkipTutorialCutscene : MonoBehaviour {
    [SerializeField] private GameObject _tutorialTimeline;

    private PlayableDirector _director;
    private bool _hasEnabled;

	// Use this for initialization
	void Start () {
        _director = _tutorialTimeline.GetComponent<PlayableDirector>();
    }
	
	// Update is called once per frame
	void Update () {
        if (!_hasEnabled) {
            if (_director.time > _director.duration - 0.02f) {
                if (TouchScript.Instance.ObjectsCache.TapHere != null) {
                    if (!TouchScript.Instance.ObjectsCache.TapHere.activeSelf) {
                        TouchScript.Instance.ObjectsCache.TapHere.SetActive(true);
                    }
                }
                _hasEnabled = true;
            }
        }
    }

    public void StopTimeline()
    {
        //_tutorialTimeline.GetComponent<PlayableDirector>().Stop();
        _director.time = 35.0f;
        if (!TouchScript.Instance.ObjectsCache.TapHere.activeSelf) {
            TouchScript.Instance.ObjectsCache.TapHere.SetActive(true);
        }
    }
}
