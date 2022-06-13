using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class RankingManager : SingletonBehaviour<RankingManager>
{
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private GameObject _rankingObject;
    [SerializeField] private Text[] _scoreText = new Text[10];
    [SerializeField] private Text[] _playNameText = new Text[10];
    [SerializeField] private InputField _inputField;
    [SerializeField] private GameObject _inputObject;

    public float ScoreNum;

    private int _count;
    private bool _panelActive = false;
  　private static Dictionary<string, float> _scoreDic = new Dictionary<string, float>();

    private void ScoreSort(string name, float score) 
    {
        if (_scoreDic.ContainsKey(name))
        {
            _scoreDic[name] = score;
        }
        else 
        {
            _scoreDic.Add(name, score);
        }

      
        foreach (KeyValuePair<string, float> i in _scoreDic.OrderByDescending(x=> x.Value).Take(10)) 
        {
            _playNameText[_count].text = i.Key;
            _scoreText[_count].text = i.Value.ToString();
            _count++;
        }
        _count = 0; 
    }

    public void PanelActive() 
    {
        _panelActive = !_panelActive;
        _rankingObject.SetActive(_panelActive);
        _resultCanvas.SetActive(!_panelActive);  
    }

    public void NameDecision() 
    {
        _inputObject.SetActive(true);
    }

    public void InputRank() 
    {
        var name = _inputField.text;
        Debug.Log(name);
        //ResultManagerからスコアを取ってくる
        ScoreSort(name, ScoreNum);
        _inputObject.SetActive(false);
    }
}
