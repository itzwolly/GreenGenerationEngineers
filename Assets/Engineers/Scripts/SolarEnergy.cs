using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SolarEnergy : Energy
{
    [SerializeField] Color _on;
    [SerializeField] Color _off;
    [SerializeField] float _checkOffset;
    [Range(0,99)]
    [SerializeField] int maxIncrements;
    Vector3 _lightDir;

    Dictionary<int, Ray> _leftRays;
    Dictionary<int, Ray> _rightRays;
    Dictionary<int, Ray> _forwardRays;
    Dictionary<int, Ray> _backwardRays;

    Ray _forwardLeft,_forwardRight,_backLeft,_backRight;
    [SerializeField] private LayerMask _layerMask;

    [SerializeField] GameObject _sparkParticles;

    int _hitLeft;
    int _hitRight;
    int _hitForward;
    int _hitBack;

    float _maxProcent;
    //List<GameObject> temp;
    //GameObject debug;
    //GameObject _debug;
    // Use this for initialization
    void Start()
    {
        _maxProcent =( maxIncrements-0) * (maxIncrements-0);
        //debug = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //debug.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        //temp = new List<GameObject>();
        //gameObject.GetComponent<MeshRenderer>().material.color = _off;

        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            _maxGreenCreated = _tutorialMaxGreenCreated;
        }
    }

    public override void SetData()
    {
        /*if (temp == null) 
            temp=new List<GameObject>();
        int size = temp.Count;
        for(int i=0;i<size;i++)
        {
            GameObject obj = temp[0];
            Destroy(obj);
            temp.RemoveAt(0);
        }*/
        base.SetData();

        Vector3 tempPos;
        float nr;
        //Debug.Log("set rays");
        _lightDir = Light.GetLights(LightType.Directional, 0)[0].transform.forward;

        _backLeft = new Ray(transform.position - transform.forward - transform.right - _lightDir, -_lightDir);
        //Instantiate(debug,_backLeft.origin, Quaternion.Euler(0, 0, 0));
        _backRight = new Ray(transform.position - transform.forward + transform.right - _lightDir, -_lightDir);
        //Instantiate(debug, _backRight.origin, Quaternion.Euler(0, 0, 0));
        _forwardLeft = new Ray(transform.position + transform.forward - transform.right - _lightDir, -_lightDir);
        //Instantiate(debug, _forwardLeft.origin, Quaternion.Euler(0, 0, 0));
        _forwardRight = new Ray(transform.position + transform.forward + transform.right - _lightDir, -_lightDir);
        //Instantiate(debug, _forwardRight.origin, Quaternion.Euler(0, 0, 0));

        _forwardRays = new Dictionary<int, Ray>();
        _backwardRays = new Dictionary<int, Ray>();
        //+ lightDir * _checkOffset
        for (float i = 0; i < maxIncrements; i++)
        {
            nr = 2*(i + 1) / maxIncrements-1;
            //Debug.Log(nr);
            tempPos = transform.position + transform.forward + transform.right * nr /*+ _lightDir * _checkOffset*/;
            //temp.Add(Instantiate(debug, tempPos, Quaternion.Euler(0, 0, 0),transform));
            _forwardRays.Add((int)i, new Ray(tempPos, -_lightDir));
            tempPos = transform.position - transform.forward + transform.right * nr /*+ _lightDir * _checkOffset*/;
            //temp.Add(Instantiate(debug, tempPos, Quaternion.Euler(0, 0, 0), transform));
            _backwardRays.Add((int)i, new Ray(tempPos, -_lightDir));
        }

        _rightRays = new Dictionary<int, Ray>();
        _leftRays = new Dictionary<int, Ray>();
        for (float i = 0; i < maxIncrements; i++)
        {
            nr = 2*(i + 1) / maxIncrements-1;
            //Debug.Log(nr);
            tempPos = transform.position + transform.right + transform.forward * nr /*+ _lightDir * _checkOffset*/;
           // temp.Add(Instantiate(debug, tempPos, Quaternion.Euler(0, 0, 0), transform));
            _rightRays.Add((int)i, new Ray(tempPos, -_lightDir));
            tempPos = transform.position - transform.right + transform.forward * nr /*+ _lightDir * _checkOffset*/;
            //temp.Add(Instantiate(debug, tempPos, Quaternion.Euler(0, 0, 0), transform));
            _leftRays.Add((int)i, new Ray(tempPos, -_lightDir));
        }
        //_debug = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube),transform.position+transform.forward,Quaternion.Euler(0,0,0));

    }

    public override void PickUp()
    {
        base.PickUp();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _lightDir = Light.GetLights(LightType.Directional, 0)[0].transform.forward;
        CompleteCheck();
        //QuickCheck();
    }
    
    bool QuickCheck()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, -_lightDir);
        
        if (Physics.Raycast(ray, out hit, Mathf.Infinity,_layerMask.value))
        {
            //Debug.Log("hit" + hit.transform.tag);
            if (hit.transform.tag == "Cloud")
            {
                //gameObject.GetComponent<MeshRenderer>().material.color = _off;
                //_connected = false;
                _active = false;
                _sparkParticles.SetActive(_active);
                base.CorrectParticles();
                return false;
            }

            // Do something with the object that was hit by the raycast.
        }
        
        //gameObject.GetComponent<MeshRenderer>().material.color = _on;
        //_connected = true;
        _active = true;
        _sparkParticles.SetActive(_active);
        base.CorrectParticles();
        return true;
        
    }

    void CompleteCheck()
    {
        _hitLeft = 0;
        _hitRight = 0;
        _hitBack = 0;
        _hitForward = 0;

        _procent = 0;
        //if (/*QuickCheck() && */_placed)
        {
            if (CheckCorners())
            {
                //Debug.Log("in shadow");
                if (_hitBack + _hitForward + _hitLeft + _hitRight == 8)//2 for each corner
                {
                    //gameObject.GetComponent<MeshRenderer>().material.color = _off;
                    //_connected = false;
                    _active = false;
                    _sparkParticles.SetActive(_active);
                    base.CorrectParticles();
                }
                else
                {
                    int _tLeft = _hitLeft;
                    int _tRight = _hitRight;
                    int _tForwards = _hitForward;
                    int _tBack = _hitBack;
                    if(_tLeft > 1 || maxIncrements==0)
                    {
                        _hitLeft = 1 + maxIncrements;
                    }
                    else if (_tLeft > 0)
                    {
                        _hitLeft += RaysHitClouds(_leftRays, _tForwards != 0);
                    }
                    //else
                    //{
                    //    _hitLeft = -10;
                    //}
                    
                    if(_tRight> 1 || maxIncrements == 0)
                    {
                        _hitRight = 1 + maxIncrements;
                    }
                    else if (_tRight > 0)
                    {
                        _hitRight += RaysHitClouds(_rightRays, _tForwards!=0);
                    }
                    //else
                    //{
                    //    _hitRight = -10;
                    //}

                    if (_tBack > 1 || maxIncrements == 0)
                    {
                        _hitBack = 1 + maxIncrements;
                    }
                    else if (_tBack > 0)
                    {
                        _hitBack += RaysHitClouds(_backwardRays, _tRight != 0);
                    }
                    //else
                    //{
                    //    _hitBack = -10;
                    //}

                    if (_tForwards > 1 || maxIncrements == 0)
                    {
                        _hitForward = 1 + maxIncrements;
                    }
                    else if (_tForwards > 0)
                    {
                        _hitForward += RaysHitClouds(_forwardRays, _tRight!=0);
                    }
                    //else
                    //{
                    //    _hitForward = -10;
                    //}

                    //gameObject.GetComponent<MeshRenderer>().material.color = _on;
                    //_connected = true;
                    _active = true;
                    _sparkParticles.SetActive(_active);
                    base.CorrectParticles();
                    float total = 0;
                    float LB = (_hitLeft - 2) * (_hitBack - 2);
                    float RF = (_hitRight - 2) * (_hitForward - 2);

                    total = LB + RF;
                    if(LB==0)
                    {
                        total+= (_hitRight - 2) * (_hitBack - 2);
                    }
                    if (RF == 0)
                    {
                        total += (_hitLeft - 2) * (_hitForward - 2);
                    }

                    _procent = 1 - total / _maxProcent;
                    //float horiz = _hitLeft + _hitRight;
                    //float vert = _hitBack + _hitForward;
                    //if (horiz/2 < vert)
                    //{
                    //    _procent = (horiz / vert) * ((horiz + vert) / 44);
                    //}
                    //else
                    //{
                    //    _procent = (vert / horiz) * ((horiz + vert) / 44);
                    //}
                }
                //return false;
            }
            else
            {
                //Debug.Log("not in shadow");
                //gameObject.GetComponent<MeshRenderer>().material.color = _on;
                //_connected = true;
                _active = true;
                _sparkParticles.SetActive(_active);
                base.CorrectParticles();
                _procent = 1;
                //not in shadow at all
            }

        }
        //else
        //{
        //    //Debug.Log("Does not hit corners");
        //    //QuickCheck();
        //}
        //return true;
    }

    public bool CheckCorners()
    {

        RaycastHit hit;
        Debug.DrawRay(_forwardLeft.origin, _forwardLeft.direction, Color.red);
        Debug.DrawRay(_forwardRight.origin, _forwardRight.direction, Color.red);
        Debug.DrawRay(_backLeft.origin, _backLeft.direction, Color.red);
        Debug.DrawRay(_backRight.origin, _backRight.direction, Color.red);

        if (Physics.Raycast(_backLeft, out hit, Mathf.Infinity, _layerMask))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Cloud")
            {
                _hitBack++;
                _hitLeft++;
            }
        }
        //Debug.Log(hit.transform.tag);
        if (Physics.Raycast(_backRight, out hit, Mathf.Infinity, _layerMask))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Cloud")
            {
                _hitBack++;
                _hitRight++;
            }
        }
        //Debug.Log(hit.transform.tag);
        if (Physics.Raycast(_forwardLeft, out hit, Mathf.Infinity, _layerMask))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Cloud")
            {
                _hitForward++;
                _hitLeft++;
            }
        }
        //Debug.Log(hit.transform.tag);
        if (Physics.Raycast(_forwardRight, out hit, Mathf.Infinity, _layerMask))
        {
            //Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Cloud")
            {
                _hitForward++;
                _hitRight++;
            }
        }
        //Debug.Log(hit.transform.tag);

        if(_hitBack+_hitForward+_hitLeft+_hitRight>0)
        {
            //gameObject.GetComponent<MeshRenderer>().material.color = _off;
            ////_connected = false;
            //_active = false;
            //Debug.Log("Hitting cloud");
            return true;
        }
        else
        {
            //gameObject.GetComponent<MeshRenderer>().material.color = _on;
            ////_connected = true;
            //_active = true;
            //Debug.Log("NOT Hitting cloud");
            return false;
        }
    }

    int RaysHitClouds(Dictionary<int, Ray> dic, bool dir)
    {
        RaycastHit hit = new RaycastHit();

        int high = dic.Count-1;
        //Debug.Log(high);
        int low = 0;
        bool first = false;
        int mid;
        int oldMid=0;
        int steps = 0;
        //int count = 0;
        //foreach (KeyValuePair<int,Ray> ray in dic)
        //{
        //    if(Physics.Raycast(ray.Value, out hit))
        //    {
        //        if (hit.transform.tag == "Cloud")
        //        {
        //            count++;
        //        }
        //    }
        //}
        //Debug.Log("actual hits are: "+count);

        //Debug.Log(dic[mid]);
        for (mid = ((high + low + Convert.ToInt32(dir)) / 2); high > low && mid != oldMid; mid = ((high + low + Convert.ToInt32(dir)) / 2))
        {
            if (Physics.Raycast(dic[mid], out hit, Mathf.Infinity, _layerMask.value) && hit.transform != null)
            {
                if (hit.transform.tag == "Cloud")
                {
                    first = true;
                }
                else
                {
                    first = false;
                }
                //Debug.Log(hit.transform.tag);
            }
            else
            {
                first = false;
            }
            if (first)
            {
                if (dir)
                {
                    high = mid;// - 1;
                }
                else
                {
                    //Debug.Log("after first "+steps);
                    low = mid;// + 1;
                }
            }
            else
            {
                if (dir)
                {
                    low = mid;// + 1;
                }
                else
                {
                    //Debug.Log("before first "+steps);
                    high = mid;// - 1;
                }
            }

            //if (high <= low || mid == oldMid)
            //{
            //    Debug.Log("exited");
            //    break;
            //}

            oldMid = mid;
            steps++;
        }
        //Debug.Log(/*low + " " + high + " - " + mid + " hasHit: " + first + */" isNotReversed: " + dir +" | " + Convert.ToInt32(dir) + "||| stepsTaken: " + steps);
        if (first == true)
        {
            if (dir)
            {
                return maxIncrements + 1 - mid;
            }
            else
            {
                return mid + 1;
            }
        }

        return 0;
    }



}
