using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GatherInfo : MonoBehaviour {
    [SerializeField] CityFeedback _city;
    [SerializeField] InputField _name;
    [SerializeField] Slider _opinionWind;
    [SerializeField] Slider _opinionSolar;
    [SerializeField] Slider _awarenessWind;
    [SerializeField] Slider _awarenessSolar;

    InfoScript _infoScript;
    // Use this for initialization
    void Start () {
        gameObject.SetActive(false);
        _infoScript = InfoScript.Instance.GetComponent<InfoScript>();
    }

    public void SaveAverageData()
    {

        string date = DateTime.Now.ToString();
        string destination = "Data/" + _infoScript.GetDifficulty() + "-" + SceneManager.GetActiveScene().name+ ".csv";
        //FileStream file;
        // _infoScript.GetDifficulty() + "-" + GetYear(date)
        //StreamWriter writer;
        string information = _name.text + ", " + _city.GetScore() + ", " + RemoveTime(date) + ", " + GetTime(date) + ", " + _infoScript.LevelReached() + ", " + _opinionWind.value + ", " + _opinionSolar.value + ", " + _awarenessWind.value + ", " + _awarenessSolar.value;
        if (!File.Exists(destination))
        {
            var sr = File.CreateText(destination);
            sr.WriteLine("sep=,");
            sr.WriteLine("#ofPlayers, 1");
            sr.WriteLine("AverageWindOpinion, " + _opinionWind.value);
            sr.WriteLine("AverageSolarOpinion, "+ _opinionSolar.value);
            sr.WriteLine("AverageWindAwareness, "+ _awarenessWind.value);
            sr.WriteLine("AverageSolarAwareness, "+ _awarenessSolar.value);
            sr.WriteLine("Name, Score, Date, Time, LevelsPlayed, WindOpinion, SolarOpinion, AwarenessWind, AwarenessSolar");
            sr.WriteLine(information);
            sr.Close();
        }
        else
        {

            StreamReader sr = File.OpenText(destination);

            List<string> lines = new List<string>();
            string temp="";
            int nr = 0;
            string line = (sr.ReadLine());
            lines.Add(line);

            line = (sr.ReadLine());//number of players
            string[] strings = line.Split(' ');
            nr = Convert.ToInt32(strings[1]);
            temp = strings[0] + " " + (nr+1).ToString(); ;
            lines.Add(temp);

            line = sr.ReadLine();//averagewindopinion
            strings = line.Split(' ');
            temp = strings[0];
            temp +=" "+ ((Convert.ToSingle(strings[1])*nr+_opinionWind.value)/(nr+1)) ;
            lines.Add(temp);

            line = sr.ReadLine();//averagesolaropinion
            strings = line.Split(' ');
            temp = strings[0];
            temp += " " + ((Convert.ToSingle(strings[1]) * nr + _opinionSolar.value) / (nr + 1));
            lines.Add(temp);

            line = sr.ReadLine();//averagewindawareness
            strings = line.Split(' ');
            temp = strings[0];
            temp += " " + ((Convert.ToSingle(strings[1]) * nr + _awarenessWind.value) / (nr + 1));
            lines.Add(temp);

            line = sr.ReadLine();//averagesolarawareness
            strings = line.Split(' ');
            temp = strings[0];
            temp += " " + ((Convert.ToSingle(strings[1]) * nr + _awarenessSolar.value) / (nr + 1));
            lines.Add(temp);

            line = sr.ReadLine();
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }

            lines.Add(information);
            sr.Close();
            File.WriteAllLines(destination, lines.ToArray());
        }
    }

    public void SaveYearlyData()
    {
        string date = DateTime.Now.ToString();
        string destination = "Data/" + _infoScript.GetDifficulty() + "-" + GetYear(date) + ".csv";
        string day = RemoveTime(date);
        string time = GetTime(date);

        string information = _name.text + ", " + _city.GetScore() + ", " + day + ", " + time + ", " + _infoScript.LevelReached() + ", " + _opinionWind.value + ", " + _opinionSolar.value + ", " + _awarenessWind.value + ", " + _awarenessSolar.value;
        if (!File.Exists(destination))
        {
            var sr = File.CreateText(destination);
            sr.WriteLine("sep=,");
            sr.WriteLine("Name, Score, Date, Time, LevelsPlayed, WindOpinion, SolarOpinion, AwarenessWind, AwarenessSolar");
            sr.WriteLine(information);
            sr.Close();
        }
        else
        {

            StreamReader sr = File.OpenText(destination);

            List<string> lines = new List<string>();
            string[] strings;
            string line = (sr.ReadLine());
            bool added = false;
            while (line != null)
            {
                strings = line.Split(' ');
                if (strings[0]!="," && strings[0] == _name.text + ",")
                {
                    Debug.Log("same username");
                    if(Convert.ToInt32(strings[4].Remove(strings[4].Length-1))<_infoScript.LevelReached() && strings[2]==(day+","))
                    {
                        added = true;
                        Debug.Log("same day and has not restarted");
                        strings[1] = (Convert.ToSingle(strings[1].Remove(strings[1].Length - 1)) + _city.GetScore()).ToString();
                        strings[3] = time + ",";
                        strings[4] = _infoScript.LevelReached() + ",";
                        strings[5] = (Convert.ToSingle(strings[5].Remove(strings[5].Length - 1)) *(_infoScript.LevelReached()-1) + _opinionWind.value)/_infoScript.LevelReached() + ",";
                        strings[6] = (Convert.ToSingle(strings[6].Remove(strings[6].Length - 1)) * (_infoScript.LevelReached() - 1) + _opinionSolar.value) / _infoScript.LevelReached() + ",";
                        strings[7] = (Convert.ToSingle(strings[7].Remove(strings[7].Length - 1)) * (_infoScript.LevelReached() - 1) + _awarenessWind.value) / _infoScript.LevelReached() + ",";
                        strings[8] = ((Convert.ToSingle(strings[8]) * (_infoScript.LevelReached() - 1) + _awarenessSolar.value) / _infoScript.LevelReached()).ToString();
                        line = "";
                        for(int i=0;i<9;i++)
                        {
                            line += strings[i] + " ";
                        }
                    }
                }
                lines.Add(line);
                line = sr.ReadLine();
            }
            if(!added)
                lines.Add(information);
            sr.Close();
            File.WriteAllLines(destination, lines.ToArray());
        }
    }

    public void SaveDailyData()
    {
        string date = DateTime.Now.ToString();
        string destination = "Data/Daily/" + _infoScript.GetDifficulty() + "-" + SceneManager.GetActiveScene().name + "-" + RemoveTime(date) + ".csv";

        string information = _name.text + ", " + _city.GetScore() + ", " + GetTime(date) + ", " + _opinionWind.value + ", " + _opinionSolar.value + ", " + _awarenessWind.value + ", " + _awarenessSolar.value + ", " + _infoScript.LevelReached();
        if (!File.Exists(destination))
        {
            var sr = File.CreateText(destination);
            sr.WriteLine("sep=,");
            sr.WriteLine("Name, Score, Time, WindOpinion, SolarOpinion, AwarenessWind, AwarenessSolar, LevelsPlayed");
            sr.WriteLine(information);
            sr.Close();
        }
        else
        {

            StreamReader sr = File.OpenText(destination);

            List<string> lines = new List<string>();
            string line = (sr.ReadLine());
            while (line != null)
            {
                lines.Add(line);
                line = sr.ReadLine();
            }

            lines.Add(information);
            sr.Close();
            File.WriteAllLines(destination, lines.ToArray());
        }
    }

    static public string RemoveTime(string str)
    {
        string newString = "";
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                continue;
            }
            else if (str[i] == ' ')
            {
                count++;
                newString += '-';
            }
            else if (str[i] == '/')
            {
                count++;
                newString += '-';
            }
            else
            {
                newString += str[i];
            }
            if (count > 2)
                break;
        }
        return newString;
    }

    static public string GetYear(string str)
    {
        string newString = "";
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                continue;
            }
            else if (str[i] == ' ')
            {
                count++;
            }
            else if (str[i] == '/')
            {
                count++;
            }
            else
            {
                if(count>1)
                newString += str[i];
            }
            if (count > 2)
                break;
        }
        return newString;
    }

    static public string GetTime(string str)
    {
        string newString = "";
        int count = 0;
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                continue;
            }
            else if (str[i] == ' ')
            {
                count++;
            }
            else if (str[i] == '/')
            {
                count++;
            }
            else if (str[i] == ':')
            {
                newString += '-';
            }
            else
            {
                if(count>2)
                newString += str[i];
            }
        }
        return newString;
    }

    static public string ReplaceSpaces(string str)
    {
        string newString = "";
        for (int i = 0; i < str.Length; i++)
        {
            if (str[i] == '\n')
            {
                continue;
            }
            else if (str[i] == ' ')
            {
                newString += '-';
            }
            else if (str[i] == '/')
            {
                newString += '-';
            }
            else if (str[i] == ':')
            {
                newString += '-';
            }
            else
            {
                newString += str[i];
            }
        }
        return newString;
    }
}
