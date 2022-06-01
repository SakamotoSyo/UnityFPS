using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shooting : MonoBehaviour
{
    //���C���J����
    [SerializeField] private Camera _cam;
    //�e�̃v���n�u
    [SerializeField]private GameObject bulletPrefab;
    [SerializeField] private GameObject _bulletMuzzule;
    //�V���b�g�̃}�Y��
    [SerializeField] private GameObject�@shoting;
    //�}�Y���t���b�V�����o���ʒu
    [SerializeField] private GameObject flashPrefab;
    //�T�������񂾂Ƃ��ɏo���v���n�u
    [SerializeField] private GameObject DeadWhale;
    //�e�̃X�s�[�h
    [SerializeField] private float shotSpeed;
    //�e�̐�
    public int shotCount = 30;
    //�ő�̒e�̐�
    public int MaxBulletNum = 4000;
    //�}�l�[�J�[�h�̌���
    public float MoneyCardEffectNum = 1;

    [SerializeField]public float ReloedTime = 30;
    [SerializeField]private float shotInterval;
    [SerializeField] private float ReloadInterval;
    [SerializeField] private GameObject bulletHolePrefab;
    public float shotTime;
    [SerializeField] ParticleSystem muzzuleFlashParticle;
    [SerializeField] GameObject bulletHitEffectPrefab;


    [SerializeField] private float shotPower;
    //�J�[�h�̌��ʂŏe�̈З͂�ς���
    [Tooltip("�J�[�h�ŏe�̈З͂�ς���")]
    [SerializeField] public float m_CardShotPowerEffect = 1;
    [SerializeField] public float CardZombieHelseEffect = 0.9f;

    [SerializeField] private GameObject DeadZombie;

    [SerializeField] private EnemySpawnScript enemySpawnScript;
    [SerializeField] private UnityChanStatus unityStatus;
    [SerializeField] private TextMeshProUGUI BulletNum;
    [SerializeField] private TextMeshProUGUI MaxBulletNumText;

    private bool ReloadBool = false;
    private int DestroyCount = 0;
    // Start is called before the first frame update
    private void Start()
    {
        MaxBulletNumText.text = MaxBulletNum.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        ShotingGun();
        
    }

    private void ShotingGun()
    {
        shotTime += Time.deltaTime;
        ReloedTime += Time.deltaTime;

        if (ReloadInterval < ReloedTime)
        {
           
            if (Input.GetKey(KeyCode.Mouse0) && shotInterval < shotTime)
            {
               

                if (shotCount > 0)
                {

                    shotCount -= 1;
                    BulletNum.text = shotCount.ToString();

                    Vector3 bulletPosition = shoting.transform.position;
                    GameObject bullet = (GameObject)Instantiate(bulletPrefab, bulletPosition, _bulletMuzzule.transform.rotation);
                    //this.gameObject.transform.rotation = Quaternion.AngleAxis(-1.0f, this.gameObject.transform.right) * this.gameObject.transform.rotation;
                    muzzuleFlashParticle.Play();
                    _cam.transform.rotation = Quaternion.AngleAxis(-1.0f, _cam.transform.right) * _cam.transform.rotation;
                    Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
                    bulletRb.AddForce(transform.forward * shotSpeed);
                    //�ˌ�����Ă���e�e�̃I�u�W�F�N�g��j�󂷂�
                    Destroy(bullet, 3.0f);

                    //�e��������
                    Shot();
                  
                }
                shotTime = 0;
            }
            else if (Input.GetKeyDown(KeyCode.R) && !ReloadBool)
            {
                ReloedTime = 0;
                ReloadBool = true;
            }

            //�����[�h�̏���
            Reload();

        }
    }

    /*private IEnumerator Reload()
    {    
       shotCount = 30;
       ReloedTime = 0;
        
       yield return new WaitForSeconds(2);
    }*/

    private void Shot() 
    {
        RaycastHit hit;
        if (Physics.Raycast(shoting.transform.position, shoting.transform.forward, out hit, 100f))
        {
            //var bulletHoleInstance = Instantiate<GameObject>(bulletHolePrefab, hit.point - shoting.transform.forward * 0.001f, Quaternion.FromToRotation(Vector3.up, hit.normal), hit.collider.transform);
            if (hit.collider.tag == "Zombie" || hit.collider.tag == "Whale")
            {
                Debug.Log("cfjioefvlek");
                //�q�b�g�����G�̃X�N���v�g���擾
                var EnemyStatusScript = hit.collider.gameObject.GetComponent<EnemyStatus>();
                EnemyStatusScript.SetHp(EnemyStatusScript.GetHp() * CardZombieHelseEffect);
                var ZombieSc = hit.collider.gameObject.GetComponent<ZombieScript>();

                //�e�������������Ƀ]���r�Ƀ_���[�W��^����i�J�[�h�̌��ʂňЗ͂��ς��j
                EnemyStatusScript.DamageHp(shotPower * m_CardShotPowerEffect);
                //�e�ɓ����������A�m���ł̂����胂�[�V����������
                ZombieSc.BulletHit();

                //�G�̗̑͂��O�ɂȂ�����
                if (EnemyStatusScript.GetHp() < 0)
                {
                    //�G��|�������ɂ������擾
                    unityStatus.SetMoney(EnemyStatusScript.GetMoney() * MoneyCardEffectNum);

                    //Enemy�̃I�u�W�F�N�g��������Prefab���Ăяo��
                    hit.collider.gameObject.SetActive(false);
                    if (hit.collider.tag == "Zombie")
                    {
                        var zombie = Instantiate(DeadZombie, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                    }
                    else if (hit.collider.tag == "Whale")
                    {
                        var shale = Instantiate(DeadWhale, hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                    }

                    Destroy(hit.collider.gameObject, 5f);
                    enemySpawnScript.EnemyDestroyCount++;
                }


            }

        }

    }

    private void Reload() 
    {
        if (ReloadBool && MaxBulletNum != 0)
        {
            if (MaxBulletNum - (30 - shotCount) > 0)
            {
                MaxBulletNum = MaxBulletNum - (30 - shotCount);
                shotCount = 30;
            }
            else
            {
                shotCount = shotCount + MaxBulletNum;
                MaxBulletNum = 0;
            }
            BulletNum.text = shotCount.ToString();
            MaxBulletNumText.text = MaxBulletNum.ToString();
            ReloadBool = false;
        }

    }

    private void Dead() 
    { 

    }

}
