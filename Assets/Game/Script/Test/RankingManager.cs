using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class RankingManager : SingletonBehaviour<RankingManager>
{
    [Header("リザルトのオブジェクト")]
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private GameObject _rankingObject;
    [Header("スコアとプレイヤーのテキスト")]
    [SerializeField] private Text[] _scoreText = new Text[10];
    [SerializeField] private Text[] _playNameText = new Text[10];

    [SerializeField] private InputField _inputField;
    [SerializeField] private GameObject _inputObject;

    public float ScoreNum;
    private int _count;
    private bool _panelActive = false;
    private static Dictionary<string, float> _scoreDic = new Dictionary<string, float>();

    private void Start()
    {
    　 //スコアをロードする
       StartLoadScore();
    }


    /// <summary>
    /// スコアを降順でソートして出力する
    /// </summary>
    /// <param name="name">playerの名前</param>
    /// <param name="score">playerのスコア</param>
    private void ScoreSort(string name, float score)
    {
        ScoreData scoredata = new ScoreData();
        //すでに名前があった場合スコアを上書きする
        if (_scoreDic.ContainsKey(name))
        {
            _scoreDic[name] = score;
        }
        else
        {
            _scoreDic.Add(name, score);

        }

        //降順でソートする
        foreach (KeyValuePair<string, float> i in _scoreDic.OrderByDescending(x => x.Value).Take(10))
        {
            _playNameText[_count].text = i.Key;
            _scoreText[_count].text = i.Value.ToString();
            scoredata._playerNameS.Add(i.Key);
            scoredata._scoreNumS.Add(i.Value);
            _count++;
        }
        string jsonstr = JsonUtility.ToJson(scoredata);
        //file書きこみ
        var writer = new StreamWriter(Application.dataPath + "/savedata.json", false);
        writer.Write(jsonstr);
        writer.Flush();
        writer.Close();

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

    /// <summary>
    /// 名前の入力のボタンのイベントから呼び出す
    /// </summary>
    public void InputRank()
    {
        var name = _inputField.text;
        //ResultManagerからスコアを取ってくる
        ScoreSort(name, ScoreNum);
        _inputObject.SetActive(false);
    }

    /// <summary>
    /// セーブしたファイルをロードする
    /// </summary>
    /// <returns></returns>
    public ScoreData LoadScoreData() 
    {
        string datastr = "";
        StreamReader reader = new StreamReader(Application.dataPath + "/savedata.json");
        datastr = reader.ReadToEnd();
        reader.Close();

        return JsonUtility.FromJson<ScoreData>(datastr);
    }

    /// <summary>
    /// Start時にロードしたファイルのデータをセットする
    /// </summary>
    private void StartLoadScore() 
    {
        ScoreData scoreData = LoadScoreData();

        for (var i = 0; i < scoreData._scoreNumS.Count; i++)
        {
            //すでに名前があった場合スコアを上書きする
            if (_scoreDic.ContainsKey(scoreData._playerNameS[i]))
            {
                _scoreDic[name] = scoreData._scoreNumS[i];
            }
            else
            {
                _scoreDic.Add(scoreData._playerNameS[i], scoreData._scoreNumS[i]);

            }
        }
    }
}
