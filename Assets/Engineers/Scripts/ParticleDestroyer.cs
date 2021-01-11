using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        StartCoroutine(ParticleDestroy());
	}
	
	private IEnumerator ParticleDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(this);
         
    }
}
