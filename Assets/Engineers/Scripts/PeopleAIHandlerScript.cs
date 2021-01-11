using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeopleAIHandlerScript : MonoBehaviour
{
    [SerializeField] Transform _peopleParent;
    [SerializeField] WaypointManager _waypoints;
    [SerializeField] WaypointManager _buildings;

    List<PersonAIScript> _people;

    // Use this for initialization
    void Start () {
        _people = new List<PersonAIScript>();
        foreach (Transform t in _peopleParent)
        {
            PersonAIScript personAI = t.gameObject.GetComponent<PersonAIScript>();
            if (personAI != null)
            {
                _people.Add(personAI);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
