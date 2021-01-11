using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

public class ActualLeaderBoard : MonoBehaviour {
    [SerializeField] GameObject _itemPrefab;
    [SerializeField] Transform _list;

    string _level = "GameplayTesting";
    string _difficulty="18";
    int _type;//1 is yearly,2 is average
    [SerializeField] GameObject _noData;

	// Use this for initialization
	void Start () {
		
	}

    public void SetLevel(string lvl)
    {
        _level = lvl;
        ShowData(GetData());
    }

    public void SetType(int t)
    {
        _type = t;
        _difficulty = InfoScript.Instance.GetComponent<InfoScript>().GetDifficulty().ToString();
        //Debug.Log("set type");
        if (_type == 1)
            ShowData(GetData());
    }

    private List<KeyValuePair<string, float>> GetData()
    {
        List<KeyValuePair<string, float>> data = new List<KeyValuePair<string, float>>();
        string date = DateTime.Now.ToString();
        string destination;
        string[] strings;
        //Debug.Log("getdata");
        try
        {
            if (_type == 1)
            {
                //Debug.Log("yearly");
                destination = "Data/" + _difficulty + "-" + GatherInfo.GetYear(date) + ".csv";//yearly
                StreamReader sr = File.OpenText(destination);
                string line = (sr.ReadLine());
                //Debug.Log(line);
                line = (sr.ReadLine());
                //Debug.Log(line);
                line = (sr.ReadLine());
                while (line != null || line == "")
                {
                    strings = line.Split(' ');
                    //Debug.Log(line);
                    if (strings[0] != ",")
                    {
                        data.Add(new KeyValuePair<string, float>(strings[0].Remove(strings[0].Length - 1), (float)Convert.ToSingle(strings[1].Remove(strings[1].Length - 1))));
                    }
                    line = (sr.ReadLine());
                }
                sr.Close();
            }
            else if (_type == 2)
            {
                Debug.Log("average");
                destination = "Data/" + _difficulty + "-" + _level + ".csv";//average
                StreamReader sr = File.OpenText(destination);
                string line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                while (line != null)
                {
                    strings = line.Split(' ');
                    //Debug.Log(strings[0]);
                    if (strings[0] != ",")
                    {
                        data.Add(new KeyValuePair<string, float>(strings[0].Remove(strings[0].Length - 1), (float)Convert.ToSingle(strings[1].Remove(strings[1].Length - 1))));
                    }
                    line = (sr.ReadLine());
                }
                sr.Close();
            }
            else
            {
                //Debug.Log("daily");
                destination = "Data/Daily/" + _difficulty + "-" + _level + "-" + GatherInfo.RemoveTime(date) + ".csv";//daily
                StreamReader sr = File.OpenText(destination);
                string line = (sr.ReadLine());
                line = (sr.ReadLine());
                line = (sr.ReadLine());
                while (line != null)
                {
                    strings = line.Split(' ');
                    //Debug.Log(strings[0]);
                    if (strings[0] != ",")
                    {
                        data.Add(new KeyValuePair<string, float>(strings[0].Remove(strings[0].Length - 1), (float)Convert.ToSingle(strings[1].Remove(strings[1].Length - 1))));
                    }
                    line = (sr.ReadLine());
                }
                sr.Close();
            }
        }
        catch (Exception e)
        {
            //Debug.Log(e.Message);
            _noData.SetActive(true);
        }



        return data;
    }

    private void ShowData(List<KeyValuePair<string,float>> data)
    {
        //Debug.Log("show data");
        data.Sort(delegate (KeyValuePair<string, float> a, KeyValuePair<string, float> b) {
            return (-a.Value).CompareTo(-b.Value);//using negative values in order to revert the order
        });
        foreach (KeyValuePair<string,float> val in data)
        {
            //Debug.Log(val.Key);
            GameObject item = Instantiate(_itemPrefab,_list.transform);
            item.GetComponent<LeaderBoardData>().SetData(val);
        }
    }

}
