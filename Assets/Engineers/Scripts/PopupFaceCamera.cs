using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupFaceCamera : MonoBehaviour {

    Camera _main;
    Quaternion _rotation;
    // Use this for initialization


    private void Awake()
    {
        _rotation = transform.rotation;
    }

    void Start () {
        _main = Camera.main;
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(_main.transform);
        transform.Rotate(_rotation.eulerAngles);

    }
}
