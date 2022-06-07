using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RigidbodyUnityChan : MonoBehaviour
{
    public UnityChanStatus allyStatus;
    [SerializeField] private ReloScript reloCs;
    [SerializeField] private EnemySpawnScript _enemySpawnScript;

    float x, z;
    float speed = 0.05f;
    private float RunSpeed = 0.1f;
    private float Sumx, Sumy, CamNumx, CamNumy;
    private bool Cambool = false;

    [Header("GameOverUnityPrefab")]
    [SerializeField] private GameObject _gameOverUnityPrefab;

    [Header("�J�����I�u�W�F�N�g")]
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject subCam;

    [SerializeField] private float wave;
    [SerializeField] private float jumpPower;

    [Header("�K���̃I�u�W�F�N�g")]
    [SerializeField] private GameObject mesh_rot, GunModel, CameraGunModel;

    [Header("�e��")]
    [SerializeField] private GameObject bulletHolePrefab;

    [Header("�e�̃X�N���v�g")]
    [SerializeField] private Shooting shotCs;

    [Header("�G����_���[�W���󂯕t���銴�o�̒���")]
    [SerializeField] private float AttackDamegeWaitTime;
    [SerializeField] private Slider slider;

    [Header("�m�b�N�o�b�N����X�s�[�h")]
    [SerializeField] private float m_KnockBackSpeed;


    [Header("������\������e�L�X�g")]
    [SerializeField] private TextMeshProUGUI MoneyTextNum;


    [Header("�O���l�[�h�̃_���[�W")]
    [SerializeField] private float _grenadDamege;

    [Header("�O���l�[�h�̃X�N���v�g")]
    [SerializeField] private DrawArc _grenadCs;
    [SerializeField] private ShootBullet _shootBulletCs;

    [Header("�O���l�[�h�������Ă��鐔")]
    public int GrenadeNum = 0;

    [Header("�O���l�[�h�̐���\������e�L�X�g")]
    [SerializeField] private TextMeshProUGUI _granedNumText;

    [Header("���U���g�̃L�����o�X")]
    [SerializeField] private GameObject _resultCanvas;

    [Header("�N���X�w�A��\������I�u�W�F�N�g")]
    [SerializeField] private GameObject CrossHair;

    [SerializeField] private ResultManager _resultCs;

    [Header("�v���C���[�̃I�[�f�B�I�\�[�X")]
    [SerializeField] private AudioSource _audioSource;

    [Header("�v���C���[�̃{�C�X")]
    [SerializeField] private AudioClip[] _audioClip;



    Quaternion cameraRot, characterRot;
    Quaternion subCameraRot;

    public float ZonbieCardNum;
    float Xsensityvity = 3f, Ysensityvity = 3f;
    private Animator animator;
    private Rigidbody rb;
    private MeshRenderer GunModelMesh;

    public bool EnemyAttack = false;
    private bool jumpNow = false;
    private bool isDamege = false;


    private float CountTime;
    private float m_WhaleAfterTime;
    private float CurrentHp;

    [SerializeField] private GameObject subCamera;//�T�u�J�����i�[�p
    private Camera subCameraSetActive;

    //�ϐ��̐錾(�p�x�̐����p)
    float minX = -80f, maxX = 80f;

    // Start is called before the first frame update
    void Start()
    {
        slider.value = 1;
        cameraRot = cam.transform.localRotation;
        subCameraRot = subCam.transform.localRotation;
        characterRot = transform.localRotation;
        GunModelMesh = GunModel.GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        subCameraSetActive = subCamera.GetComponent<Camera>();
        rb = GetComponent<Rigidbody>();
        _resultCs = GameObject.Find("ResultManager").GetComponent<ResultManager>();
        

        CurrentHp = allyStatus.GetHp();
        slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();

        _granedNumText.text = GrenadeNum.ToString();
    }

    void Update()
    {

        //�J�����Ɋւ��鏈��
        CameraController();

        //��ʂ̃J�[�\�������b�N����
        UpdateCursorLock();

        //�L�����N�^�[�𓮂���
        CharacterMove();


        //�O���l�[�h�𓊂��鏈��
        GrenadStart();

        //ADS�����Ƃ��̏���
        ADSBool();
       
        Jump();

       
        animator.SetBool("Damage", isDamege);
        _granedNumText.text = GrenadeNum.ToString();

        //������\��������
        MoneyTextNum.text = allyStatus.GetMoney().ToString();

        if (allyStatus.GetHp() <= 0)
        {
            _resultCanvas.SetActive(true); 
            _resultCs.enabled = true;
            var a = Instantiate(_gameOverUnityPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
            
            this.gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
    }

    private void CharacterMove() 
    {
        x = 0;
        z = 0;
        //�v���C���[�̑���B�_���[�W���󂯂Ă���Ƃ��͑���ł��Ȃ�
        if (!Input.GetKey(KeyCode.LeftShift) && !isDamege)
        {
            x = Input.GetAxisRaw("Horizontal") * speed;
            z = Input.GetAxisRaw("Vertical") * speed;
        }
        else
        {
            x = Input.GetAxisRaw("Horizontal") * RunSpeed;
            z = Input.GetAxisRaw("Vertical") * RunSpeed;
        }

        //transform.position += new Vector3(x,0,z);

        transform.position += cam.transform.forward * z + cam.transform.right * x;

        //�L�����N�^�[�̈ړ��B�_���[�W���󂯂Ă���Ƃ��͑���ł��Ȃ�
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !isDamege)
        {
            animator.SetBool("RunBool", true);
        }
        else
        {
            animator.SetBool("RunBool", false);
        }
    }

    //��ʂ̃J�[�\�������b�N����
    public void UpdateCursorLock()
    {

        //�V���b�v�ȂǂɈڂ������J�[�\���̃��b�N���O��
        if (_enemySpawnScript.raundType == EnemySpawnScript.RaundType.StandardRaund)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (_enemySpawnScript.raundType != EnemySpawnScript.RaundType.StandardRaund)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    //�p�x�����֐��̍쐬
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,z�̓x�N�g���i�ʂƌ����j�Fw�̓X�J���[�i���W�Ƃ͖��֌W�̗ʁj)

        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1f;

        float angleX = Mathf.Atan(q.x) * Mathf.Rad2Deg * 2f;

        angleX = Mathf.Clamp(angleX, minX, maxX);

        q.x = Mathf.Tan(angleX * Mathf.Deg2Rad * 0.5f);

        return q;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (jumpNow == true)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                jumpNow = false;
                animator.SetBool("jump Bool", false);
            }

        }
    }


    /// <summary>
    ///�W�����v���� 
    /// </summary>
    void Jump()
    {

        if (jumpNow == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audioSource.PlayOneShot(_audioClip[0]);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            animator.SetBool("jump Bool", true);
            jumpNow = true;
        }


    }


    private void OnTriggerStay(Collider other)
    {
        CountTime += Time.deltaTime;
        bool EnemyTag = other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Whale") || other.gameObject.CompareTag("Bullet");
        if (EnemyTag && EnemyAttack && AttackDamegeWaitTime < CountTime)
        {
            var EnemyStatusScript = other.gameObject.GetComponent<EnemyStatus>();
            //�_���[�W���󂯂��Ƃ�
            isDamege = true;
            _audioSource.PlayOneShot(_audioClip[1]);
            //�m�b�N�o�b�N����
            rb.AddForce(-transform.forward * m_KnockBackSpeed, ForceMode.VelocityChange);
            allyStatus.DamageHp(EnemyStatusScript.GetPower());
            slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();
            //HP�����炷����
            CurrentHp = allyStatus.GetHp();

            EnemyAttack = false;
            CountTime = 0;
        }

        //�T���̍U����̐����܂�𓥂񂾎�
        if (other.gameObject.CompareTag("WhaleAttackAfter"))
        {
            m_WhaleAfterTime += Time.deltaTime;
            //��ʂ�Ԃ����鏈��
            //
            if (m_WhaleAfterTime > 2)
            {
                Debug.Log(allyStatus.GetHp() - 210f);
                allyStatus.SetHp(allyStatus.GetHp() - 510f);
                slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();
                CurrentHp = allyStatus.GetHp();
                m_WhaleAfterTime = 0;
            }
        }
    }


    /// <summary> �O���l�[�h�𓊂��鏈�� </summary>
    private void GrenadStart()
    {

        //�O���l�[�h���\����
        if (Input.GetButton("Fire1") && GrenadeNum > 0)
        {
            animator.SetBool("GrenadBool", true);
            GunModelMesh.enabled = false;
            _grenadCs.DrawArcBool = true;

        }
        //�{�^���𗣂������ɌĂ΂��
        if (Input.GetButtonUp("Fire1") && GrenadeNum > 0)
        {
            animator.SetBool("GrenadBool", false);
            animator.Play("GrenadeThrow");
            GrenadeNum--;
            GunModelMesh.enabled = true;
            _grenadCs.DrawArcBool = false;

        }
        
    }


    private void OnParticleCollision(GameObject other)
    {

        if (other.layer == 6)
        {
            allyStatus.SetHp(-_grenadDamege);
            slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();
            CurrentHp = allyStatus.GetHp();
            Debug.Log("ddd");
        }
    }


    /// <summary> ADS�����Ƃ��̏��� </summary>
    private void ADSBool() 
    {
        if (Input.GetMouseButton(1) && !isDamege)
        {
            mesh_rot.SetActive(false);
            GunModel.SetActive(false);
            CameraGunModel.SetActive(true);
            animator.SetBool("Bool", true);
            subCamera.SetActive(true);
            subCameraSetActive.enabled = true;
            CrossHair.SetActive(true);

        }
        else
        {
            mesh_rot.SetActive(true);
            GunModel.SetActive(true);
            CameraGunModel.SetActive(false);
            animator.SetBool("Bool", false);
            subCameraSetActive.enabled = false;
            CrossHair.SetActive(false);
        }

    }


�@�@/// <summary>�J�����Ɋւ��鏈��</summary>
    private void CameraController() 
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;
        #region
        if (Input.GetMouseButton(0) && shotCs.shotCount > 0 && !reloCs.ReloadBool)
        {
            //���R�C���p�[�^�[�������
            Sumx = Random.Range(-0.3f, 0.3f);
            Sumy = Random.Range(-0.3f, 0);
        }
        if (!Input.GetMouseButton(0))
        {
            Sumx = 0;
            Sumy = 0;
        }

        if (!Input.GetMouseButton(1) && !Cambool)
        {
            //CamNumx = 0;
            //CamNumy = 0;
            CamNumx = subCam.transform.localEulerAngles.x;
            CamNumy = subCam.transform.localEulerAngles.y;
            Cambool = true;
        }
        else if (Input.GetMouseButton(1) && Cambool)
        {
            //CamNumx = 0;
            //CamNumy = 0;
            CamNumx = cam.transform.localEulerAngles.x;
            CamNumy = cam.transform.localEulerAngles.y;
            Cambool = false;
        }

        #endregion
        //�J�����̉�]
        if (!Input.GetMouseButton(1))
        {

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot, 0);
            //cam.transform.rotation = subCamera.transform.rotation;
        }
        else
        {

            subCameraRot *= Quaternion.Euler(-yRot + Sumy, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot + Sumx + CamNumx, 0);
            //subCam.transform.rotation = cam.transform.rotation;
        }

        //Update�̒��ō쐬�����֐����Ă�
        if (!Input.GetMouseButton(1))
        {
            cameraRot = ClampRotation(cameraRot);
            cam.transform.localRotation = cameraRot;
        }
        else
        {
            subCameraRot = ClampRotation(subCameraRot);
            subCamera.transform.localRotation = subCameraRot;
        }

        transform.localRotation = characterRot;

        CamNumx = 0;
        CamNumy = 0;
    }



    private void DamegeFalse() => isDamege = false;
}
