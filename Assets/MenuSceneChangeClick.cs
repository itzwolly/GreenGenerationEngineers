using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSceneChangeClick : TouchInputBehaviour
{

    [SerializeField] GameObject _loadingScreen;
    [SerializeField] string _sceneToCall;
    Image _image;
    bool _sauced = false;
    Color skeet;
    // Use this for initialization
    private void Start()
    {
        TouchScript.Instance.Behaviours.Add(this);
        _added = true;
        if (_loadingScreen != null)
        {
            _image = _loadingScreen.GetComponent<Image>();
            skeet = _image.color;
            skeet.a = 0;
        }
    }

    public override void Began(RaycastHit pHit, Touch pTouch)
    {
        if (transform.parent.gameObject.activeInHierarchy)
        {
            //if (TouchScript.Instance.CurrentMode == TouchScript.Mode.Build) {
            if (Input.touchCount == 1)
            {
                if (pHit.point != null && pHit.transform.name == name)
                {
                    //Debug.Log(pHit.transform.name);
                    LoadScene(_sceneToCall);
                }

            }
        }
    }

    public string GetSceneName()
    {
        return _sceneToCall;
    }

    public void LoadScene(string pName)
    {
        if (_loadingScreen != null)
        {

            _loadingScreen.SetActive(true);
            _sauced = true;
            StartCoroutine("OneLerpBoi");

        }
        //StartCoroutine(LoadNewScene(_sceneToCall));
        TouchScript.Instance.LoadNextLevel(pName);
    }


    private IEnumerator OneLerpBoi()
    {
        while (skeet.a < 1)
        {
            skeet.a = Mathf.Lerp(skeet.a, 1, Time.unscaledTime/30);
            _image.color = skeet;
            yield return null;
        }
        

    }
}
