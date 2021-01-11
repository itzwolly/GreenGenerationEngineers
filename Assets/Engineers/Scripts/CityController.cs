using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityController : MonoBehaviour {
    [SerializeField] Vector3 _centerOffset;
    [SerializeField] float _cityRadius;
	[SerializeField] GameObject _debugPrefab;

    public Vector3 GetCenter()
    {
        return transform.position+_centerOffset;
    }

    public Vector3 GetConnectionPoint(Vector3 fromPos)
    {
        Vector3 pos = GetCenter();
        //Instantiate(_debugPrefab, pos, Quaternion.Euler(0, 0, 0), transform);
        Vector3 dir = (fromPos - pos).normalized;
        dir.y = 0;

        //Instantiate(_debugPrefab, pos + dir * (_cityRadius), Quaternion.Euler(0, 0, 0), transform);
        return pos + dir*(_cityRadius);
    }
}
