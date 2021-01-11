using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class HappinesAreaScript : MonoBehaviour {
    [Range(0f, 100f)] [SerializeField] private float _radius;

    void OnDrawGizmos() {
#if UNITY_EDITOR
        if (_radius > 0) {
                Handles.color = Color.green;
                Handles.DrawWireDisc(transform.position, transform.forward, _radius);
        }
#endif
    }

    public bool CheckIfIn(Vector3 pos) {
        if (_radius > 0) {
            return (Vector3.Distance(pos, transform.position) < _radius);
        }
        return false;
    }
}
