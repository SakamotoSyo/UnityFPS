using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;

public class CardTachScript : MonoBehaviour ,ISelectHandler
{
    [SerializeField] private Shooting shootingCs;
    [SerializeField]private GameObject shootingObject;
    [SerializeField] private RigidbodyUnityChan _rbPlayer;
   

    [SerializeField]private PlayableAsset CardReternAnim;
    private Animator anim;
    private PlayableDirector timeline;
    private GameObject _shopObject;
    private EnemySpawnScript _enemySpawnScript;
    [SerializeField]private TextMeshProUGUI CardBuyNumText;
    [SerializeField]private GameObject CardNumImages;
    private Image[] CardBuyNumImage;
    //�J�[�h�̖��O�Ɖ��񂩂������J�E���g����
    private Dictionary<string, int> CardNameNumImage = new Dictionary<string, int>();

    private CardManager _cardManager;

    private bool CardEffectBool = false;

    // Start is called before the first frame update
    void Start()
    {
        shootingObject = GameObject.Find("UnityChan/SubCamera/SubSciFiGunLightBlue");
        _shopObject = GameObject.Find("RoundCanvas/CardShop");
        _rbPlayer = GameObject.Find("UnityChan").GetComponent<RigidbodyUnityChan>();
        _enemySpawnScript = GameObject.Find("SpawnGameObject").GetComponent<EnemySpawnScript>();
        anim = GetComponent<Animator>();    
        shootingCs = shootingObject.GetComponent<Shooting>();
        timeline = GetComponent<PlayableDirector>();
        _cardManager = GameObject.Find("CardPosition").GetComponent<CardManager>();

        //�J�[�h�����������Ă������̃C���[�W�������Ă���
        CardBuyNumImage = new Image[CardNumImages.transform.childCount];
        for (int i = 0; i < CardNumImages.transform.childCount; i++) 
        {
            CardBuyNumImage[i] = CardNumImages.transform.GetChild(i).gameObject.GetComponent<Image>();
        }

        //�J�[�h�̗v�f��ǉ�
        CardNameNumImage.Add("Money", 0);
        CardNameNumImage.Add("Gun", 0);
        CardNameNumImage.Add("ZombieCard", 0);

        for (int i = 0; i < _cardManager.MoneyCardNum; i++)
        {
            if (this.gameObject.tag == "Money") 
            {
                CardBuyNumImage[i].color = new Color(77, 255, 8, 255);
                CardBuyNumText.text = _cardManager.MoneyCardNum.ToString();
            }
            
        }

        for (int i = 0; i < _cardManager.GunCardNumn; i++)
        {
            if (this.gameObject.tag == "Gun")
            {
                CardBuyNumImage[i].color = new Color(77, 255, 8, 255);
                CardBuyNumText.text = _cardManager.GunCardNumn.ToString();
            }
        }

        for (int i = 0; i < _cardManager.ZonbieCardNum; i++)
        {
            if (this.gameObject.tag == "ZombieCard")
            {
                CardBuyNumImage[i].color = new Color(77, 255, 8, 255);
                CardBuyNumText.text = _cardManager.ZonbieCardNum.ToString();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (CardEffectBool)
        {
            this.gameObject.AddComponent<Button>();
            CardEffectBool = false;
        }

        if (_enemySpawnScript.raundType == EnemySpawnScript.RaundType.ShopSelectEnd) 
        {
            anim.SetBool("SetBool", true);
        
        }
    }

    //Card�����I�������A�j���[�V�����ŌĂяo��
    private void AddComponent() 
    {
       CardEffectBool = true;
      
    }

    private void CardEffect() 
    {
        
    }

    //CardShop�̃{�^���������ꂽ�Ƃ�
    public void OnSelect(BaseEventData even) 
    {
        anim.SetBool("SetBool", true);
        _enemySpawnScript.raundType = EnemySpawnScript.RaundType.ShopSelectEnd;
        if (this.gameObject.tag == "Money" && CardNameNumImage["Money"] < 5)
        {
            shootingCs.MoneyCardEffectNum += 0.1f;

            CardNameNumImage["Money"] = CardNameNumImage["Money"] + 1;

            _cardManager.MoneyCardNum++;
            
        }
        else if (CardNameNumImage["Money"] <= 5)
        {
            //�e�L�X�g���o��
        }

        if (this.gameObject.tag == "Gun" && CardNameNumImage["Gun"] < 5) 
        {
            shootingCs.m_CardShotPowerEffect += 0.1f;
            CardNameNumImage["Gun"] = CardNameNumImage["Gun"] + 1;
           
            _cardManager.GunCardNumn++;
        }
        else if (CardNameNumImage["Gun"] <= 5)
        {
            //�e�L�X�g���o��
        }

        if (this.gameObject.tag == "ZombieCard" && CardNameNumImage["ZombieCard"] < 5)
        {
            _rbPlayer.ZonbieCardNum += 0.1f;
            CardNameNumImage["ZombieCard"] = CardNameNumImage["ZombieCard"] + 1;

            _cardManager.ZonbieCardNum++;
        }
        else if (CardNameNumImage["ZombieCard"] <= 5) 
        {
            //�e�L�X�g���o��
        }

        Destroy(this.gameObject, 4f);

    }

    private void SelectEndRaundChange() 
    {
        _enemySpawnScript.raundType = EnemySpawnScript.RaundType.StandardRaund;
    }
}
