using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Playables;
using System.Linq;
public class EnemySpawnScript : MonoBehaviour
{
    public enum RaundType 
    {
        StandardRaund,
        ShopSelect,
        ShopCardSelect,
        ShopSelectEnd,
        
    }
    //プレイヤーのステータス
    [SerializeField] private UnityChanStatus allyStatus;
    //Raundのタイプ管理
    public RaundType raundType = RaundType.StandardRaund;

    //RaundObjectのCanvas
    [SerializeField] private GameObject RaundCanvasObject;
    //ショップイメージの親のオブジェクト
    [SerializeField] private GameObject ShopImageObject;
    [SerializeField] private GameObject _weaponShopImage;
    //カードのプレハブのリスト
    [Header("カード")][SerializeField]private List<GameObject> CardPrefabList = new List<GameObject>();
    [SerializeField] private GameObject CardPosition;
    //カードのアニメーションカウント
    private int CardCount = 0;

    //カードのPlayableAssetを入れる
    [SerializeField] private List<PlayableAsset> CardAnimationList = new List<PlayableAsset>();
    //スポーンの親オブジェクト
    [SerializeField] private GameObject spawnGameObject;
    //子オブジェクト
    private GameObject[] spawnChildren;
    //ゾンビのプレハブ
    [SerializeField] private GameObject _zombiePrefab;
    //サメのプレハブ
    [SerializeField] private GameObject _whalePrefab;

    //ラウンド数を表示するテキスト
    [SerializeField]private TextMeshProUGUI RaundNumText;
    //休憩時間を表示するテキスト
    [SerializeField] private TextMeshProUGUI BreakTimeText;
    //休憩時間を表示するオブジェクト
    [SerializeField] private GameObject BreakTimeObject;
    //休憩時間が始まった時に一度だけActiveになるオブジェクト
    [SerializeField] private GameObject OnBreak;
    private bool BreakBool = false;

   

    //ラウンド数
    private float WaveCount = 0;
    //Spawnするまでの時間
    [SerializeField]private float SpawnTime;
    //SpawnTime計測用
    private float Count;
    //ラウンド休憩時間
    [Tooltip("ラウンドの休憩時間")][SerializeField]private float BreakWaitTime;
    private float BreakWaitCountTime;
    //ウェーブが始まったかどうか
    private bool isWaveStartOne = false;
    //敵が全滅したかどうか
    [SerializeField]private bool isdestruction = false;
    //追加のゾンビ
    private int AdditionalZombies = 2;
    //追加したゾンビのウェーブ倍率がかかった後の数字
    private int AddZombies = 0;
    //追加のゾンビカウント
    private int _additionalZombieCount;
    //追加のサメ
    private int _additionalWhale = 1;
    //追加したサメのウェーブ倍率がかかった後の数字
    private int _addWhale = 2;
    //追加のサメカウント
    private int _additionalWhaleCount;
    //敵の合計
    private int totalEnemy;
    //敵がどれだけ倒されたか
    public int EnemyDestroyCount;

    [Header("スポーン管理")]
    //ゾンビの最低スポーン数
    [SerializeField] private int BasicZombieNum;
    // Start is called before the first frame update
    void Start()
    {
        spawnChildren = new GameObject[spawnGameObject.transform.childCount];
        //スポーン元の子オブジェクトをとってくる
        for(int i = 0; i < spawnGameObject.transform.childCount; i++)
        {
            spawnChildren[i] = spawnGameObject.transform.GetChild(i).gameObject;
        }
        //カウントを同じ値にする
        BreakWaitCountTime = BreakWaitTime;
   
    }

    // Update is called once per frame
    void Update()
    {
        //ifの中身を敵が全滅したらにする
        if(!isdestruction)
        {
            WaveStart(WaveCount);
        }

        breakTime();
    }

    //タイムラインにアニメーションをセットする
    void SetTimeLine(GameObject go)
    {
        Animator anim = go.GetComponent<Animator>();
        PlayableDirector pd = go.GetComponent<PlayableDirector>();
        PlayableBinding binding = pd.playableAsset.outputs.First(c => c.streamName == "Animation Track"); // 重要
        pd.SetGenericBinding(binding.sourceObject, anim);
        pd.Play();
    }

    /// <summary>
    /// ラウンド管理
    /// </summary>
    /// <param name="WaveNum"></param>
    private void WaveStart(float WaveNum)
    {

        //RandomZombieNum = Random.Renge(0, 20) + WaveNum;
        Count += Time.deltaTime;
        if (!isWaveStartOne)
        {
            RaundNumText.text = "Round" + WaveNum.ToString();
            //毎回ある程度の敵の湧きの違いをつける
            var WaveRandom = UnityEngine.Random.Range(WaveNum, WaveNum + 0.9f);
            //追加される敵の数を計算
            AddZombies = (int)Mathf.Floor(WaveRandom) * AdditionalZombies;
            //追加されるサメ
            _addWhale = (int)Mathf.Floor(WaveRandom) * _additionalWhale;
            //合計の敵の数
            totalEnemy = BasicZombieNum + AddZombies + _addWhale;
            

            //ラウンドの最低スポーン。
            for (int j = 0; j < BasicZombieNum; j++)
            {
                
                //ランダムな場所にスポーンさせる
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);

                GameObject zombie = Instantiate(_zombiePrefab, spawnChildren[RandomPlace].transform.position, spawnChildren[RandomPlace].transform.rotation);
            }

            isWaveStartOne = true;
        }
        
        //ある程度スポーンの時間間隔を設定する。後にカウントにランダムな数字足してもいいかも
        if (SpawnTime < Count)
        {
            //追加でゾンビをスポーンさせる
            if (AddZombies > _additionalZombieCount)
            {
                //ランダムな場所にスポーンさせる
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);
                
                GameObject zombie = Instantiate(_zombiePrefab, spawnChildren[RandomPlace].transform.position,spawnChildren[RandomPlace].transform.rotation);
                _additionalZombieCount++;

            }
            if (_addWhale > _additionalWhaleCount) 
            {
                Debug.Log("呼ばれた");
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);
                GameObject whale = Instantiate(_whalePrefab, spawnChildren[RandomPlace].transform.position, spawnChildren[RandomPlace].transform.rotation);
                _additionalWhaleCount++;
            }
            else if (totalEnemy == EnemyDestroyCount)
            {
                //敵が全滅したかどうか
                isdestruction = true;
                isWaveStartOne = false;
                WaveCount++;
                AddZombies = 0;
                _additionalZombieCount = 0;
                _additionalWhaleCount = 0;
                EnemyDestroyCount = 0;
                totalEnemy = 0;
                Debug.Log("ウェーブが終わりました");
            }
           
            
        }

    }

    //CardShopScriptのOnClickイベントから呼び出す
    public void CardShopOpen() 
    {
        //Shuffle関数を使ってランダムにカードを取り出す
        foreach (GameObject Card in Shuffle(CardPrefabList))
        {
            //カードを生成してそれに対応したTimeLineAnimationを入れる
            GameObject RamdomCard = Instantiate(Card, CardPosition.transform.position, CardPosition.transform.rotation);
            RamdomCard.transform.SetParent(RaundCanvasObject.transform);
            var timeline = RamdomCard.GetComponent<PlayableDirector>();
            timeline.playableAsset = CardAnimationList[CardCount];
            SetTimeLine(RamdomCard);
            CardCount++;
        }
        CardCount = 0;

        raundType = RaundType.ShopCardSelect;
    }


    //ランダムにリストから要素を取り出す
    public static IEnumerable<T> Shuffle<T>(IEnumerable<T> list)
    {
        var tempList = new List<T>(list); // 入力をリストにコピーする

        var r = new System.Random(); // 値を取り出すときに乱数を使用する

        while (tempList.Count != 0)
        {
            int index = r.Next(0, tempList.Count);

            T value = tempList[index];
            tempList.RemoveAt(index);

            yield return value;
        }
    }

    /// <summary>
    /// 休憩時間
    /// </summary>
    private void breakTime() 
    {
        //ゾンビを全滅させたら
        if (isdestruction)
        {

            //BreakBool = false;  
            //休憩時間の計測
            BreakTimeText.text = BreakWaitTime.ToString("f1");
            if (!BreakBool && BreakWaitTime == BreakWaitCountTime && isdestruction)
            {
                BreakTimeObject.SetActive(true);
                OnBreak.SetActive(true);
                BreakBool = true;

            }

            BreakWaitTime -= Time.deltaTime;


            //buyシステム
            if (Input.GetKeyDown(KeyCode.B) && raundType == RaundType.StandardRaund)
            {
                ShopImageObject.SetActive(true);
                _weaponShopImage.SetActive(true);
                raundType = RaundType.ShopSelect;
            }

            //休憩時間が終わったら
            if (BreakWaitTime <= 0)
            {
                isdestruction = false;
                BreakWaitTime = 25;
                BreakBool = false;
                OnBreak.SetActive(false);


            }
        }
    }
}
