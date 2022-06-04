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
    //�v���C���[�̃X�e�[�^�X
    [SerializeField] private UnityChanStatus allyStatus;
    //Raund�̃^�C�v�Ǘ�
    public RaundType raundType = RaundType.StandardRaund;

    //RaundObject��Canvas
    [SerializeField] private GameObject RaundCanvasObject;
    //�V���b�v�C���[�W�̐e�̃I�u�W�F�N�g
    [SerializeField] private GameObject ShopImageObject;
    [SerializeField] private GameObject _weaponShopImage;
    //�J�[�h�̃v���n�u�̃��X�g
    [Header("�J�[�h")][SerializeField]private List<GameObject> CardPrefabList = new List<GameObject>();
    [SerializeField] private GameObject CardPosition;
    //�J�[�h�̃A�j���[�V�����J�E���g
    private int CardCount = 0;

    //�J�[�h��PlayableAsset������
    [SerializeField] private List<PlayableAsset> CardAnimationList = new List<PlayableAsset>();
    //�X�|�[���̐e�I�u�W�F�N�g
    [SerializeField] private GameObject spawnGameObject;
    //�q�I�u�W�F�N�g
    private GameObject[] spawnChildren;
    //�]���r�̃v���n�u
    [SerializeField] private GameObject _zombiePrefab;
    //�T���̃v���n�u
    [SerializeField] private GameObject _whalePrefab;

    //���E���h����\������e�L�X�g
    [SerializeField]private TextMeshProUGUI RaundNumText;
    //�x�e���Ԃ�\������e�L�X�g
    [SerializeField] private TextMeshProUGUI BreakTimeText;
    //�x�e���Ԃ�\������I�u�W�F�N�g
    [SerializeField] private GameObject BreakTimeObject;
    //�x�e���Ԃ��n�܂������Ɉ�x����Active�ɂȂ�I�u�W�F�N�g
    [SerializeField] private GameObject OnBreak;
    private bool BreakBool = false;

   

    //���E���h��
    private float WaveCount = 0;
    //Spawn����܂ł̎���
    [SerializeField]private float SpawnTime;
    //SpawnTime�v���p
    private float Count;
    //���E���h�x�e����
    [Tooltip("���E���h�̋x�e����")][SerializeField]private float BreakWaitTime;
    private float BreakWaitCountTime;
    //�E�F�[�u���n�܂������ǂ���
    private bool isWaveStartOne = false;
    //�G���S�ł������ǂ���
    [SerializeField]private bool isdestruction = false;
    //�ǉ��̃]���r
    private int AdditionalZombies = 2;
    //�ǉ������]���r�̃E�F�[�u�{��������������̐���
    private int AddZombies = 0;
    //�ǉ��̃]���r�J�E���g
    private int _additionalZombieCount;
    //�ǉ��̃T��
    private int _additionalWhale = 1;
    //�ǉ������T���̃E�F�[�u�{��������������̐���
    private int _addWhale = 2;
    //�ǉ��̃T���J�E���g
    private int _additionalWhaleCount;
    //�G�̍��v
    private int totalEnemy;
    //�G���ǂꂾ���|���ꂽ��
    public int EnemyDestroyCount;

    [Header("�X�|�[���Ǘ�")]
    //�]���r�̍Œ�X�|�[����
    [SerializeField] private int BasicZombieNum;
    // Start is called before the first frame update
    void Start()
    {
        spawnChildren = new GameObject[spawnGameObject.transform.childCount];
        //�X�|�[�����̎q�I�u�W�F�N�g���Ƃ��Ă���
        for(int i = 0; i < spawnGameObject.transform.childCount; i++)
        {
            spawnChildren[i] = spawnGameObject.transform.GetChild(i).gameObject;
        }
        //�J�E���g�𓯂��l�ɂ���
        BreakWaitCountTime = BreakWaitTime;
   
    }

    // Update is called once per frame
    void Update()
    {
        //if�̒��g��G���S�ł�����ɂ���
        if(!isdestruction)
        {
            WaveStart(WaveCount);
        }

        breakTime();
    }

    //�^�C�����C���ɃA�j���[�V�������Z�b�g����
    void SetTimeLine(GameObject go)
    {
        Animator anim = go.GetComponent<Animator>();
        PlayableDirector pd = go.GetComponent<PlayableDirector>();
        PlayableBinding binding = pd.playableAsset.outputs.First(c => c.streamName == "Animation Track"); // �d�v
        pd.SetGenericBinding(binding.sourceObject, anim);
        pd.Play();
    }

    /// <summary>
    /// ���E���h�Ǘ�
    /// </summary>
    /// <param name="WaveNum"></param>
    private void WaveStart(float WaveNum)
    {

        //RandomZombieNum = Random.Renge(0, 20) + WaveNum;
        Count += Time.deltaTime;
        if (!isWaveStartOne)
        {
            RaundNumText.text = "Round" + WaveNum.ToString();
            //���񂠂���x�̓G�̗N���̈Ⴂ������
            var WaveRandom = UnityEngine.Random.Range(WaveNum, WaveNum + 0.9f);
            //�ǉ������G�̐����v�Z
            AddZombies = (int)Mathf.Floor(WaveRandom) * AdditionalZombies;
            //�ǉ������T��
            _addWhale = (int)Mathf.Floor(WaveRandom) * _additionalWhale;
            //���v�̓G�̐�
            totalEnemy = BasicZombieNum + AddZombies + _addWhale;
            

            //���E���h�̍Œ�X�|�[���B
            for (int j = 0; j < BasicZombieNum; j++)
            {
                
                //�����_���ȏꏊ�ɃX�|�[��������
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);

                GameObject zombie = Instantiate(_zombiePrefab, spawnChildren[RandomPlace].transform.position, spawnChildren[RandomPlace].transform.rotation);
            }

            isWaveStartOne = true;
        }
        
        //������x�X�|�[���̎��ԊԊu��ݒ肷��B��ɃJ�E���g�Ƀ����_���Ȑ��������Ă���������
        if (SpawnTime < Count)
        {
            //�ǉ��Ń]���r���X�|�[��������
            if (AddZombies > _additionalZombieCount)
            {
                //�����_���ȏꏊ�ɃX�|�[��������
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);
                
                GameObject zombie = Instantiate(_zombiePrefab, spawnChildren[RandomPlace].transform.position,spawnChildren[RandomPlace].transform.rotation);
                _additionalZombieCount++;

            }
            if (_addWhale > _additionalWhaleCount) 
            {
                Debug.Log("�Ă΂ꂽ");
                var RandomPlace = UnityEngine.Random.Range(0, spawnChildren.Length);
                GameObject whale = Instantiate(_whalePrefab, spawnChildren[RandomPlace].transform.position, spawnChildren[RandomPlace].transform.rotation);
                _additionalWhaleCount++;
            }
            else if (totalEnemy == EnemyDestroyCount)
            {
                //�G���S�ł������ǂ���
                isdestruction = true;
                isWaveStartOne = false;
                WaveCount++;
                AddZombies = 0;
                _additionalZombieCount = 0;
                _additionalWhaleCount = 0;
                EnemyDestroyCount = 0;
                totalEnemy = 0;
                Debug.Log("�E�F�[�u���I���܂���");
            }
           
            
        }

    }

    //CardShopScript��OnClick�C�x���g����Ăяo��
    public void CardShopOpen() 
    {
        //Shuffle�֐����g���ă����_���ɃJ�[�h�����o��
        foreach (GameObject Card in Shuffle(CardPrefabList))
        {
            //�J�[�h�𐶐����Ă���ɑΉ�����TimeLineAnimation������
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


    //�����_���Ƀ��X�g����v�f�����o��
    public static IEnumerable<T> Shuffle<T>(IEnumerable<T> list)
    {
        var tempList = new List<T>(list); // ���͂����X�g�ɃR�s�[����

        var r = new System.Random(); // �l�����o���Ƃ��ɗ������g�p����

        while (tempList.Count != 0)
        {
            int index = r.Next(0, tempList.Count);

            T value = tempList[index];
            tempList.RemoveAt(index);

            yield return value;
        }
    }

    /// <summary>
    /// �x�e����
    /// </summary>
    private void breakTime() 
    {
        //�]���r��S�ł�������
        if (isdestruction)
        {

            //BreakBool = false;  
            //�x�e���Ԃ̌v��
            BreakTimeText.text = BreakWaitTime.ToString("f1");
            if (!BreakBool && BreakWaitTime == BreakWaitCountTime && isdestruction)
            {
                BreakTimeObject.SetActive(true);
                OnBreak.SetActive(true);
                BreakBool = true;

            }

            BreakWaitTime -= Time.deltaTime;


            //buy�V�X�e��
            if (Input.GetKeyDown(KeyCode.B) && raundType == RaundType.StandardRaund)
            {
                ShopImageObject.SetActive(true);
                _weaponShopImage.SetActive(true);
                raundType = RaundType.ShopSelect;
            }

            //�x�e���Ԃ��I�������
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
