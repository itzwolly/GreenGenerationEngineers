using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectState : MonoBehaviour {
    private State _currentState;

    public State CurrentState {
        get { return _currentState; }
        set { _currentState = value; }
    }
    public enum State {
        None,
        Placed,
        Preview,
    }
}
