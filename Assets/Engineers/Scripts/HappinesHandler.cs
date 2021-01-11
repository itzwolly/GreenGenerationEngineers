using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappinesHandler : MonoBehaviour {

    [SerializeField] float _happinessDecrease;
    [SerializeField] HappinessIndicatorScript _happinessScript;
    List<HappinesAreaScript> _areas;
    Dictionary<GameObject, float> _generators;
    Dictionary<GameObject, bool> _generatorState;
    List<GameObject> keys;
    float _percentage;
    float procentage;

    // Use this for initialization
    void Start ()
    {
        _areas = new List<HappinesAreaScript>();
        _generators = new Dictionary<GameObject, float>();
        _generatorState = new Dictionary<GameObject, bool>();
        keys = new List<GameObject>();
        foreach (Transform t in transform)
        {
            if(t.tag=="Happiness")
            {
                _areas.Add(t.gameObject.GetComponentInChildren<HappinesAreaScript>());
            }
        }
    }

    public float GetTotalHappiness()
    {
        if(keys==null || keys.Count<1)
        {
            //Debug.Log("no generators");
            return 100;
        }
        return 100 - _percentage / keys.Count;
    }

    public void PlacedGenerator(GameObject obj)
    {
        Vector3 pos = obj.transform.position;
        _generators.Add(obj,0);
        _generatorState.Add(obj, CheckPos(pos));
        keys.Add(obj);
    }

    public void SetActiveAreas(bool pEnable) {
        foreach (HappinesAreaScript area in _areas) {
            if (area.gameObject.activeSelf) {
                ParticleSystem system = area.gameObject.transform.GetComponent<ParticleSystem>();
                ParticleSystemRenderer renderer = system.GetComponent<ParticleSystemRenderer>();
                renderer.enabled = pEnable;
            }
        }
    }

    public bool CheckPos(Vector3 pos)
    {
        foreach (HappinesAreaScript area in _areas)
        {
            if (area.CheckIfIn(pos))
            {
                //ParticleSystem system = area.gameObject.transform.GetComponent<ParticleSystem>();
                //ParticleSystemRenderer renderer = system.GetComponent<ParticleSystemRenderer>();
                //renderer.enabled = true;
                return true;
            }

            //ParticleSystem sys = area.gameObject.transform.GetComponent<ParticleSystem>();
            //ParticleSystemRenderer rend = sys.GetComponent<ParticleSystemRenderer>();
            //rend.enabled = false;
        }
        return false;
    }

    //public void RemoveGenerator(GameObject obj)
    //{
    //    _generators.Remove(obj);
    //    _generatorState.Remove(obj);
    //    keys.Remove(obj);
    //}

    public void CheckGenerator(GameObject generator)
    {
        //Debug.Log("checked");
        _generatorState[generator] = CheckPos(generator.transform.position);
    }

    public void ResetHappiness(GameObject obj,float nr)
    {
        _generators[obj] = nr;

    }

    public float HappinessForGenerator(GameObject generator)
    {
        try
        {
            return _generators[generator];
        }
        catch
        {
            return 0;
        }
    }

    // Update is called once per frame
 
    void Update () {
        //Debug.Log(_generators.Keys.Count);
        _percentage = 0;
        foreach (GameObject obj in keys)
        {
            //Debug.Log(_generatorState[obj]);
            if (obj != null) {
                procentage = 0;
                if (_generatorState != null) {
                    if (_generatorState[obj]) {
                        procentage = 0;
                        //Debug.Log("procentage = " + _generators[obj]);
                        if (_generators.TryGetValue(obj, out procentage)) {
                            if (procentage > 100) {
                                obj.GetComponent<Energy>().GetConnectionMaker().DestroyConnection();
                            } else {
                                _generators[obj] += _happinessDecrease;
                            }
                        }
                    } else {
                        _generators[obj] = 0;
                    }
                }
                _percentage += procentage;
            }
        }
        //Debug.Log(_percentage/keys.Count);
        if (keys.Count > 0)
            _happinessScript.SetHappinessPercent(100 - _percentage / keys.Count);
	}
}
