using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastTest : MonoBehaviour
{
    [SerializeField] Vector3 _direction;
    [SerializeField] Vector3 _offset;
    // Use this for initialization

    // Update is called once per frame
    void Update () {

        if (Input.GetKeyDown(KeyCode.Space))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position+_offset, _direction);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.tag + " - " + hit.transform.name);
            }
        }
    }
}
