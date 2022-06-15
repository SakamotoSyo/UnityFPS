using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.IO;

public class RankingManager : SingletonBehaviour<RankingManager>
{
    [Header("���U���g�̃I�u�W�F�N�g")]
    [SerializeField] private GameObject _resultCanvas;
    [SerializeField] private GameObject _rankingObject;
    [Header("�X�R�A�ƃv���C���[�̃e�L�X�g")]
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
    �@ //�X�R�A�����[�h����
       StartLoadScore();
    }


    /// <summary>
    /// �X�R�A���~���Ń\�[�g���ďo�͂���
    /// </summary>
    /// <param name="name">player�̖��O</param>
    /// <param name="score">player�̃X�R�A</param>
    private void ScoreSort(string name, float score)
    {
        ScoreData scoredata = new ScoreData();
        //���łɖ��O���������ꍇ�X�R�A���㏑������
        if (_scoreDic.ContainsKey(name))
        {
            _scoreDic[name] = score;
        }
        else
        {
            _scoreDic.Add(name, score);

        }

        //�~���Ń\�[�g����
        foreach (KeyValuePair<string, float> i in _scoreDic.OrderByDescending(x => x.Value).Take(10))
        {
            _playNameText[_count].text = i.Key;
            _scoreText[_count].text = i.Value.ToString();
            scoredata._playerNameS.Add(i.Key);
            scoredata._scoreNumS.Add(i.Value);
            _count++;
        }
        string jsonstr = JsonUtility.ToJson(scoredata);
        //file��������
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
    /// ���O�̓��͂̃{�^���̃C�x���g����Ăяo��
    /// </summary>
    public void InputRank()
    {
        var name = _inputField.text;
        //ResultManager����X�R�A������Ă���
        ScoreSort(name, ScoreNum);
        _inputObject.SetActive(false);
    }

    /// <summary>
    /// �Z�[�u�����t�@�C�������[�h����
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
    /// Start���Ƀ��[�h�����t�@�C���̃f�[�^���Z�b�g����
    /// </summary>
    private void StartLoadScore() 
    {
        ScoreData scoreData = LoadScoreData();

        for (var i = 0; i < scoreData._scoreNumS.Count; i++)
        {
            //���łɖ��O���������ꍇ�X�R�A���㏑������
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
