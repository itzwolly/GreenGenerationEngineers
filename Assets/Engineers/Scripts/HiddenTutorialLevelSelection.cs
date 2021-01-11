using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HiddenTutorialLevelSelection : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("LOL KEK");
            SceneManager.LoadSceneAsync("GameplayTesting");
           
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("LOL KEK 2");
            SceneManager.LoadSceneAsync("GameplayTesting");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SceneManager.LoadSceneAsync("GameplayTesting");
        }
    }
}
