using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Playables;

public class ResultManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _zonbieNumText;
    [SerializeField] private TextMeshProUGUI _whaleNumText;
    [SerializeField] private TextMeshProUGUI _moneyScoreText;
    [SerializeField] private TextMeshProUGUI _scoreResult;

    //リザルトテキストのタイムライン
    [SerializeField] private PlayableDirector[] _textPlayableDirector;

    //コルーチンの時間間隔

    public float ZonbieNum = 0;
    public float WhaleNum = 0;
    public float MoneyNum = 0;

    private float TotalScore;

    void Start()
    {
        StartCoroutine("ScoreTotalling");
    }

    void Update()
    {
       
        
    }

    //間にアニメーションのスタート入れていいかも
    private IEnumerator ScoreTotalling() 
    {

        _zonbieNumText.text = ZonbieNum.ToString();
        _textPlayableDirector[0].Play();
        yield return new WaitForSeconds(3);

        _whaleNumText.text = WhaleNum.ToString();
        _textPlayableDirector[1].Play();
        yield return new WaitForSeconds (3);
        
        _moneyScoreText.text = MoneyNum.ToString();
        _textPlayableDirector[2].Play();
        yield return new WaitForSeconds (3);

        TotalScore = ZonbieNum + (WhaleNum * 10) * (MoneyNum * 0.2f);

        _textPlayableDirector[3].Play();
        if (TotalScore < 100)
        {
            _scoreResult.text = "C";
        }
        else if (TotalScore < 300)
        {
            _scoreResult.text = "B";
        }
        else if (TotalScore < 500)
        {
            _scoreResult.text = "A";
        }
        else if (TotalScore < 1000)
        {
            _scoreResult.text = "S";
        }
        else if (TotalScore < 2000) 
        {
            _scoreResult.text = "SS";
        }

        yield return new WaitForSeconds(3);

        //Reset処理

    }
}
