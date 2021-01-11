using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelNameSelector : MonoBehaviour {
    public Text _levelName;
    private GameObject _tipText;

	// Use this for initialization
	void Start () {
        
        if (SceneManager.GetActiveScene().name == "Tutorial") {
            _levelName.text = "Introductie";
        } else if (SceneManager.GetActiveScene().name == "GameplayTesting") {
            _levelName.text = "Mist Eiland";
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
