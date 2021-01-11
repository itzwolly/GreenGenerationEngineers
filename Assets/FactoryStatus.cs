using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryStatus : MonoBehaviour {
    [SerializeField] EnergyIndicatorScript _energyIndicator;
    // Use this for initialization
    int _children;
    float _procent;
    [SerializeField] int nr;
    int i;
    [SerializeField] bool[] _forward, _backward;
	void Start () {
        _children = transform.childCount+1;
        _forward = new bool[_children];
        _backward = new bool[_children];
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        _procent = (_energyIndicator.GetEnergy() / 100);
        if(nr!=(int)(_children*_procent))
        {
            i = 0;
            nr = (int)(_children * _procent);
            foreach (Transform t in transform)
            {
                Animation anim = t.gameObject.GetComponent<Animation>();
                
                if (i < nr)
                {
                    //float lenght;
                    //lenght = anim[anim.name].length;
                    //if(anim[anim.name].time<lenght-1 && !_forward[i])
                    //{
                        if (!anim.isPlaying && !_forward[i])
                        {
                            Debug.Log("start playing forward " + t.name);
                            anim[t.name].speed = 1;
                            anim.Play();
                            _forward[i] = true;
                            _backward[i] = false;
                    }
                    //}
                    //else
                    //{
                    //    Debug.Log("stop playing forward " + t.name);
                    //    anim.Stop();
                    //    _forward[i] = true;
                    //    _backward[i] = false;
                    //}
                }
                else
                {
                    //if (anim[anim.name].time >1 && !_backward[i])
                    //{
                        if (!anim.isPlaying && !_backward[i])
                        {
                            //Debug.Log("start playing back " + t.name);
                            anim[t.name].speed = -1;
                            anim.Play();
                            _backward[i] = true;
                            _forward[i] = false;
                        }
                    //}
                    //else
                    //{
                    //    Debug.Log("stop playing back " + t.name);
                    //    anim.Stop();
                    //    _backward[i] = true;
                    //    _forward[i] = false;
                    //}
                    
                }
                i++;
            }
        }
	}
}
