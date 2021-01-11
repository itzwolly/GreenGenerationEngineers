using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSCounter : MonoBehaviour {

    float deltaTime = 0.0f;

    void Update() {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        float ms = deltaTime * 1000.0f;
        float fps = 1.0f / deltaTime;
        string text = string.Format("FPS: {0:0.0} ({1:0.} MS)", fps, ms);

        GetComponent<Text>().text = text;
    }
}
