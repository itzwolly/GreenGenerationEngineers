using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardData : MonoBehaviour {
    [SerializeField] Keyboard _keyboard;
    [SerializeField] InputField _inputField;

    bool _selected;
	// Use this for initialization
	void Start () {
        _inputField.text = "";
        _selected = false;
    }

    public void AddLetter(string _key)
    {
        Debug.Log(_key + " LOL");
        _inputField.text += _key;
        _inputField.textComponent.text += _key;
    }

    public void RemoveLastLetter()
    {
        _inputField.text = _inputField.text.Remove(_inputField.text.Length - 1);
    }

    public void AddSpace()
    {
        _inputField.text += " ";
    }

    // Update is called once per frame
    void Update () {
        if(!_selected && _inputField.isFocused)
        {
            _selected = true;
            SelectWriting();
        }
            
	}

    public void SelectWriting()
    {
        //Debug.Log("start keyboard writting");
        _keyboard.Activate();
    }

    public void DeselectWriting()
    {
        _keyboard.Deactivate();
        _selected = false;
        //Debug.Log("stop keyboard writting");
    }
}
