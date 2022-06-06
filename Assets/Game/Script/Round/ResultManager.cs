using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class ResultManager : MonoBehaviour
{
    [Header("�e�L�X�g�̃Q�[���I�u�W�F�N�g")]
    [SerializeField] private TextMeshProUGUI _zonbieNumText;
    [SerializeField] private TextMeshProUGUI _whaleNumText;
    [SerializeField] private TextMeshProUGUI _moneyScoreText;
    [SerializeField] private TextMeshProUGUI _scoreResult;

    [Header("���U���g�e�L�X�g�̃^�C�����C��")]
    [SerializeField] private PlayableDirector[] _textPlayableDirector;

    [Header("�R���[�`���̎��ԊԊu")]
    [SerializeField] private int _coroutineTime;

    [Header("�X�R�A")]
    [SerializeField]public float ZonbieNum = 0;
    [SerializeField] public float WhaleNum = 0;
    [SerializeField] public float MoneyNum = 0;

    private float TotalScore;

    void Start()
    {
        StartCoroutine("ScoreTotalling");
    }

    void Update()
    {
       
        
    }

    //�ԂɃA�j���[�V�����̃X�^�[�g����Ă�������
    private IEnumerator ScoreTotalling() 
    {

        _zonbieNumText.text = ZonbieNum.ToString();
        _textPlayableDirector[0].Play();
        yield return new WaitForSeconds(_coroutineTime);

        _whaleNumText.text = WhaleNum.ToString();
        _textPlayableDirector[1].Play();
        yield return new WaitForSeconds (_coroutineTime);
        
        _moneyScoreText.text = MoneyNum.ToString();
        _textPlayableDirector[2].Play();
        yield return new WaitForSeconds (_coroutineTime);

        TotalScore = ZonbieNum + (WhaleNum * 10) + (MoneyNum * 0.2f);

        _textPlayableDirector[3].Play();

        if (TotalScore < 2000)
        {
            Debug.Log("asdawf");
            _scoreResult.text = "C";
        }
        else if (TotalScore < 4000)
        {
            _scoreResult.text = "B";
        }
        else if (TotalScore < 6000)
        {
            _scoreResult.text = "A";
        }
        else if (TotalScore < 10000)
        {
            _scoreResult.text = "S";
        }
        else if (TotalScore < 20000) 
        {
            _scoreResult.text = "SS";
        }

        yield return new WaitForSeconds(_coroutineTime);

        //Reset����

    }
}
