using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] private GameObject _weaponShop;
    [SerializeField] private EnemySpawnScript _enemySpawnScript;
 

    public int MoneyCardNum;
    public int GunCardNumn;
    public int ZonbieCardNum;
    // Start is called before the first frame update
    void Start()
    {
        //Imegeï™îzóÒÇçÏÇÈ
      
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && _enemySpawnScript.raundType == EnemySpawnScript.RaundType.ShopCardSelect) 
        {
            Debug.Log("uiheiknvl");
            _enemySpawnScript.raundType = EnemySpawnScript.RaundType.StandardRaund;
            _weaponShop.SetActive(false);
           
        
        }
    }

    
}
