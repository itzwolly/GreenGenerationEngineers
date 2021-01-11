using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    [SerializeField] bool _visualWaypoints;
    List<Transform> _waypoints;
	// Use this for initialization
	void Start () {
        _waypoints = new List<Transform>();
        foreach(Transform t in transform)
        {
            _waypoints.Add(t);
            t.gameObject.GetComponent<MeshRenderer>().enabled = (_visualWaypoints);
        }
		
	}

    private void OnValidate()
    {
        _waypoints = new List<Transform>();
        foreach (Transform t in transform)
        {
            _waypoints.Add(t);
            t.gameObject.GetComponent<MeshRenderer>().enabled = (_visualWaypoints);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
