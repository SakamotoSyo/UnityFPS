using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemPanelButtonScipt : MonoBehaviour, ISelectHandler
{

    [SerializeField] private RigidbodyUnityChan _playerCs;
    [SerializeField] private Shooting _shootingCs;
    [SerializeField] private UnityChanStatus _statusCs;

    [Header("�C���t�H���[�V�����̉摜")]
    [SerializeField] private Image _informationImageObject;
    [Header("���̃A�C�e���̃C���[�W�摜")]
    [SerializeField] private Sprite _informationSprite;
    [Header("�l�i�̐ݒ�")]
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

    //�C���t�H���[�V�����̉摜�������ւ���
    private void InformationRotate() 
    {
      _informationImageObject.sprite = _informationSprite;  
    }

    public void BuyItem(string itemName) 
    {
        if (itemName == "Grenade")
        {
            _playerCs.GrenadeNum++;
            _statusCs.SetMoney(_statusCs.GetMoney() - _GrenadPrice);
        }
        else if (itemName == "Amo") 
        {
            _shootingCs.shotCount += 30;
            _statusCs.SetMoney(_statusCs.GetMoney() - _assaultAmoPrice);
        }
    }

}
