using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour {
    [SerializeField] string _key;
    [SerializeField] int _size;
    [SerializeField] KeyboardData _keyboardData;

    //Keyboard _keyboard;

    //public void SetKeyboard(Keyboard keyboard)
    //{
    //    _keyboard = keyboard;
    //}

    public void ClickKey()
    {
        _keyboardData.AddLetter(_key);
    }

    public void BackSpace()
    {
        _keyboardData.RemoveLastLetter();
    }

    private void OnValidate()
    {
        gameObject.GetComponentInChildren<Text>().text = _key;
        gameObject.GetComponentInChildren<Text>().fontSize = _size;
    }
}
