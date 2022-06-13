using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class ResultManager : MonoBehaviour
{
    [Header("Player�̃X�e�[�^�X")]
    [SerializeField]private UnityChanStatus _allyStatus;
    [Header("�e�L�X�g�̃Q�[���I�u�W�F�N�g")]
    [SerializeField] private TextMeshProUGUI _zonbieNumText;
    [SerializeField] private TextMeshProUGUI _whaleNumText;
    [SerializeField] private TextMeshProUGUI _moneyScoreText;
    [SerializeField] private TextMeshProUGUI _scoreResult;

    [Header("���U���g�e�L�X�g�̃^�C�����C��")]
    [SerializeField] private PlayableDirector[] _textPlayableDirector;

    [Header("�R���[�`���̎��ԊԊu")]
    [SerializeField] private int _coroutineTime;

    [Header("Shooting�̃X�N���v�g")]
    [SerializeField] private Shooting _shootingCs; 

    private float TotalScore;
    private RankingManager _rankingManager;

    void Start()
    {
        _rankingManager = RankingManager.Instance;
        StartCoroutine("ScoreTotalling");
    }

    void Update()
    {
       
        
    }

    //�ԂɃA�j���[�V�����̃X�^�[�g����Ă�������
    private IEnumerator ScoreTotalling() 
    {

        _zonbieNumText.text = _shootingCs.ZombieNum.ToString();
        _textPlayableDirector[0].Play();
        yield return new WaitForSeconds(_coroutineTime);

        _whaleNumText.text = _shootingCs.WhaleNum.ToString();
        _textPlayableDirector[1].Play();
        yield return new WaitForSeconds (_coroutineTime);
        
        _moneyScoreText.text = _allyStatus.GetMoney().ToString();
        _textPlayableDirector[2].Play();
        yield return new WaitForSeconds (_coroutineTime);

        TotalScore = _shootingCs.ZombieNum + (_shootingCs.WhaleNum * 10) + (_allyStatus.GetMoney() * 0.2f);

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
        else if (TotalScore > 10000) 
        {
            _scoreResult.text = "SS";
        }

        yield return new WaitForSeconds(_coroutineTime);

        _rankingManager.ScoreNum = TotalScore;
        // _rankingManager.PanelActive();
        //_rankingManager.InputRank();
        _rankingManager.NameDecision();
        this.gameObject.SetActive(false);    

    }
}
