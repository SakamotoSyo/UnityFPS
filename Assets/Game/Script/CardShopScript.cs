using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardShopScript : MonoBehaviour
{
    [SerializeField] private EnemySpawnScript enemySpawnCs;
    [SerializeField] private UnityChanStatus allyStatus;


    [SerializeField] private TextMeshProUGUI m_informationText;
    [SerializeField] private float CardShopPrice = 500;
    //ショップのオブジェクト
    [Header("武器屋さん")]
    [SerializeField] private GameObject _weaponShopObject;
    [SerializeField] private GameObject _cardShopObject;
    // Start is called before the first frame update
    void Start()
    {
        //allyStatus = GameObject.Find("UnityChan").GetComponent<UnityChanStatus>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && enemySpawnCs.raundType == EnemySpawnScript.RaundType.ShopSelect) 
        {
            
           
            _cardShopObject.SetActive(false);
            enemySpawnCs.raundType = EnemySpawnScript.RaundType.StandardRaund;
        }
        else if(Input.GetButtonDown("Cancel") && enemySpawnCs.raundType == EnemySpawnScript.RaundType.ShopCardSelect)
        {
            _cardShopObject.SetActive(false);
            _weaponShopObject.SetActive(false);
        }
    }

    public void ShopOpenActive() 
    {
        //カードを購入したとき
        if (allyStatus.GetMoney() >= CardShopPrice)
        {
            Debug.Log("yobareta");
            //カードをランダムに排出する
            enemySpawnCs.CardShopOpen();
            //お金を減らす処理
            allyStatus.SetMoney(allyStatus.GetMoney() - (allyStatus.GetMoney() + CardShopPrice));
            this.gameObject.SetActive(false);
        }
        else if (allyStatus.GetMoney() < 500f) 
        {
            m_informationText.text = "お金が足りません";
        }
        
    }

    public void WeaponShopOpen() 
    {
        Debug.Log("hiadhwodi");
        _weaponShopObject.SetActive(true);
        _cardShopObject.SetActive(false);
        
       
        enemySpawnCs.raundType = EnemySpawnScript.RaundType.ShopCardSelect;
    }
}
