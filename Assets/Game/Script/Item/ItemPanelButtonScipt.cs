using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ItemPanelButtonScipt : MonoBehaviour, ISelectHandler
{

    [SerializeField] private RigidbodyUnityChan _playerCs;
    [SerializeField] private Shooting _shootingCs;
    [SerializeField] private UnityChanStatus _statusCs;

    [SerializeField] private TextMeshProUGUI _bulletNum;
    [Header("インフォメーションの画像")]
    [SerializeField] private Image _informationImageObject;
    [Header("このアイテムのイメージ画像")]
    [SerializeField] private Sprite _informationSprite;
    [Header("値段の設定")]
    [SerializeField] private float _GrenadPrice;
    [SerializeField] private float _assaultAmoPrice;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnSelect(BaseEventData eventData) 
    {
        InformationRotate();
    }

    //インフォメーションの画像を差し替える
    private void InformationRotate() 
    {
      _informationImageObject.sprite = _informationSprite;  
    }

    public void BuyItem(string itemName) 
    {
        if (itemName == "Grenade")
        {
            if (_statusCs.GetMoney() - _GrenadPrice > 0) 
            {
                _playerCs.GrenadeNum++;
                _statusCs.SetMoney(-_GrenadPrice);
            }
            
        }
        else if (itemName == "Amo") 
        {
            if (_statusCs.GetMoney() - _assaultAmoPrice > 0) 
            {
                _shootingCs.shotCount += 30;
                _statusCs.SetMoney(-_assaultAmoPrice);
            }
            
        }
    }

}
