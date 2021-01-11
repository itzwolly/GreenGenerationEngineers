using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAIHandlerScript : MonoBehaviour
{
    [SerializeField] WaypointManager _waypointManager;
    [SerializeField] Transform _carsParent;

    List<Transform> _waypoints;
    List<CarAIScript> _cars;

    // Use this for initialization
    void Start () {
        _cars = new List<CarAIScript>();
        foreach(Transform t in _carsParent)
        {
            CarAIScript carAI = t.gameObject.GetComponent<CarAIScript>();
            if(carAI!=null)
            {
                _cars.Add(carAI);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
