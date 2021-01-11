using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTagAllChildren : MonoBehaviour {
    [SerializeField] bool _set;
	// Use this for initialization
	void Start () {
		
	}

    private void OnValidate()
    {
        SetChildrenTag(transform, tag);
        _set = false;
    }

    private void SetChildrenTag(Transform tr, string ta)
    {
        if (tr.childCount == 0)
        {
            tr.tag = ta;
        }
        else
        {
            foreach (Transform t in tr)
            {
                t.tag = ta;
                SetChildrenTag(t, ta);
            }
        }
    }

    // Update is called once per frame
    void Update () {
		
	}
}
