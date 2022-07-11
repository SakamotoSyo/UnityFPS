using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RigidbodyUnityChan : MonoBehaviour
{
    public enum Weapon
    {
        Gun,
        Knife
    }

    public UnityChanStatus allyStatus;
    [SerializeField] private ReloScript reloCs;
    [SerializeField] private EnemySpawnScript _enemySpawnScript;

    float x, z;
    float speed = 0.05f;
    private float RunSpeed = 0.08f;
    private float Sumx, Sumy, CamNumx, CamNumy;
    private bool Cambool = false;

    [Header("GameOverUnityPrefab")]
    [SerializeField] private GameObject _gameOverUnityPrefab;

    [Header("カメラオブジェクト")]
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject subCam;

    [SerializeField] private float wave;
    [SerializeField] private float jumpPower;

    [Header("ガンのオブジェクト")]
    [SerializeField] private GameObject mesh_rot, GunModel, CameraGunModel;

    [Header("銃痕")]
    [SerializeField] private GameObject bulletHolePrefab;

    [Header("弾のスクリプト")]
    [SerializeField] private Shooting shotCs;

    [Header("敵からダメージを受け付ける感覚の長さ")]
    [SerializeField] private float AttackDamegeWaitTime;
    [SerializeField] private Slider slider;

    [Header("ノックバックするスピード")]
    [SerializeField] private float m_KnockBackSpeed;


    [Header("お金を表示するテキスト")]
    [SerializeField] private TextMeshProUGUI MoneyTextNum;


    [Header("グレネードのダメージ")]
    [SerializeField] private float _grenadDamege;

    [Header("グレネードのスクリプト")]
    [SerializeField] private DrawArc _grenadCs;
    [SerializeField] private ShootBullet _shootBulletCs;

    [Header("グレネードを持っている数")]
    public int GrenadeNum = 0;

    [Header("グレネードの数を表示するテキスト")]
    [SerializeField] private TextMeshProUGUI _granedNumText;

    [Header("リザルトのキャンバス")]
    [SerializeField] private GameObject _resultCanvas;

    [Header("クロスヘアを表示するオブジェクト")]
    [SerializeField] private GameObject CrossHair;

    [SerializeField] private ResultManager _resultCs;

    [Header("プレイヤーのオーディオソース")]
    [SerializeField] private AudioSource _audioSource;

    [Header("プレイヤーのボイス")]
    [SerializeField] private AudioClip[] _audioClip;

    [Header("ナイフのゲームオブジェクト")]
    [SerializeField] private GameObject KifeGameObject;



    Quaternion cameraRot, characterRot;
    Quaternion subCameraRot;

    public float ZonbieCardNum;
    float Xsensityvity = 3f, Ysensityvity = 3f;
    private Animator animator;
    private Rigidbody rb;
    private MeshRenderer GunModelMesh;


    public bool EnemyAttack = false;
    private bool _jumpNow = false;
    private bool _isDamege = false;
    public bool IsGrenadeThrow = false;
    //回避しているとき
    private bool _isRolling = false;
    private Weapon _weaponType = Weapon.Gun;


    private float CountTime;
    private float m_WhaleAfterTime;
    private float CurrentHp;

    [SerializeField] private GameObject subCamera;//サブカメラ格納用
    private Camera subCameraSetActive;

    //変数の宣言(角度の制限用)
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
        //ナイフの処理
       // KnifeAttack();

        //武器の切り替え
        //ChangeWeapon();

        //カメラに関する処理
        CameraController();

        //画面のカーソルをロックする
        UpdateCursorLock();

        //キャラクターを動かす
        CharacterMove();

        //回避行動


        //グレネードを投げる処理
        GrenadStart();

        //ADSしたときの処理
        ADSBool();

        Jump();


        animator.SetBool("Damage", _isDamege);
        _granedNumText.text = GrenadeNum.ToString();

        //お金を表示させる
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
        x = 0;
        z = 0;
        //プレイヤーの操作。ダメージを受けているときは操作できない
        if (!Input.GetKey(KeyCode.LeftShift) && !_isDamege)
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
    }

    /// <summary>武器の切替</summary>
    private void ChangeWeapon() 
    {
        //マウスホイールで武器を切り替える
        var Mouse = Input.GetAxisRaw("Mouse ScrollWheel");
        Debug.Log(Mouse);
        if (Mouse != 0)  
        {
            if (Weapon.Gun == _weaponType)
            {
                _weaponType = Weapon.Knife;
                GunModel.SetActive(false);
                KifeGameObject.SetActive(true);
                Debug.Log("呼ばれた");
               
            }
            else if (Weapon.Knife == _weaponType) 
            {
                _weaponType = Weapon.Gun;
                KifeGameObject.SetActive(false);
                GunModel.SetActive(true);
            }

        }

    }

    private void KnifeAttack() 
    {
        if (Input.GetMouseButton(0) && Weapon.Knife == _weaponType) 
        {
            animator.SetBool("KnifeAttack", true);
        }
    }

    private void CharacterMove() 
    {
       

        //キャラクターの移動。ダメージを受けているときは操作できない
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) && !_isDamege)
        {
            animator.SetBool("RunBool", true);
        }
        else
        {
            animator.SetBool("RunBool", false);
        }

        
        if (Input.GetKeyDown(KeyCode.C)) 
        {
            _isRolling = true;
        }
        animator.SetBool("RollingBool", _isRolling);
    }

    //画面のカーソルをロックする
    public void UpdateCursorLock()
    {

        //ショップなどに移った時カーソルのロックを外す
        if (_enemySpawnScript.raundType == EnemySpawnScript.RaundType.StandardRaund && allyStatus.GetHp() > 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (_enemySpawnScript.raundType != EnemySpawnScript.RaundType.StandardRaund || allyStatus.GetHp() <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }


    //角度制限関数の作成
    public Quaternion ClampRotation(Quaternion q)
    {
        //q = x,y,z,w (x,y,zはベクトル（量と向き）：wはスカラー（座標とは無関係の量）)

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
        if (_jumpNow == true)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                _jumpNow = false;
                animator.SetBool("jump Bool", false);
            }

        }
    }


    /// <summary>
    ///ジャンプ処理 
    /// </summary>
    void Jump()
    {

        if (_jumpNow == true) return;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _audioSource.PlayOneShot(_audioClip[0]);
            rb.AddForce(transform.up * jumpPower, ForceMode.Impulse);
            animator.SetBool("jump Bool", true);
            _jumpNow = true;
        }


    }


    private void OnTriggerStay(Collider other)
    {
        CountTime += Time.deltaTime;
        bool EnemyTag = other.gameObject.CompareTag("Zombie") || other.gameObject.CompareTag("Whale") || other.gameObject.CompareTag("Bullet");
        if (EnemyTag && EnemyAttack && AttackDamegeWaitTime < CountTime && !_isRolling)
        {
            var EnemyStatusScript = other.gameObject.GetComponent<EnemyStatus>();
            //ダメージを受けたとき
            _isDamege = true;
            _audioSource.PlayOneShot(_audioClip[1]);
            //ノックバック処理
            var a = (other.gameObject.transform.position - transform.position).normalized;
            rb.AddForce(-1 * a * m_KnockBackSpeed, ForceMode.VelocityChange);
            allyStatus.DamageHp(EnemyStatusScript.GetPower());
            slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();
            //HPを減らす処理
            CurrentHp = allyStatus.GetHp();

            EnemyAttack = false;
            CountTime = 0;
        }

        //サメの攻撃後の水たまりを踏んだ時
        if (other.gameObject.CompareTag("WhaleAttackAfter"))
        {
            m_WhaleAfterTime += Time.deltaTime;
            //画面を赤くする処理
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


    /// <summary> グレネードを投げる処理 </summary>
    private void GrenadStart()
    {

        //グレネードを構える
        if (Input.GetButton("Fire1") && GrenadeNum > 0)
        {
            animator.SetBool("GrenadBool", true);
            GunModelMesh.enabled = false;
            _grenadCs.DrawArcBool = true;

        }
        //ボタンを離した時に呼ばれる
        if (Input.GetButtonUp("Fire1") && GrenadeNum > 0)
        {
            animator.SetBool("GrenadBool", false);
            animator.Play("GrenadeThrow");
            GunModelMesh.enabled = true;
            _grenadCs.DrawArcBool = false;
            if (IsGrenadeThrow) 
            {
                GrenadeNum--;
            }

        }
        
    }

    /// <summary>
    /// グレネードを投げる処理
    /// </summary>
    private void GrenadeThrow() 
    {
        IsGrenadeThrow = true;
        Debug.Log("dawdsd");
    }

    /// <summary>
    /// パーティクルに当たった時の処理
    /// </summary>
    /// <param name="other"></param>
    private void OnParticleCollision(GameObject other)
    {

        if (other.layer == 6)
        {
            allyStatus.SetHp(-_grenadDamege);
            slider.value = (float)CurrentHp / (float)allyStatus.GetMaxHp();
            CurrentHp = allyStatus.GetHp();
        }
    }


    /// <summary> ADSしたときの処理 </summary>
    private void ADSBool() 
    {
        if (Input.GetMouseButton(1) && !_isDamege && Weapon.Gun == _weaponType)
        {
            mesh_rot.SetActive(false);
            GunModel.SetActive(false);
            CameraGunModel.SetActive(true);
            animator.SetBool("Bool", true);
            subCamera.SetActive(true);
            subCameraSetActive.enabled = true;
            CrossHair.SetActive(true);

        }
        else if(!Input.GetMouseButton(1) && !_isDamege && Weapon.Gun == _weaponType)
        {
            mesh_rot.SetActive(true);
            GunModel.SetActive(true);
            CameraGunModel.SetActive(false);
            animator.SetBool("Bool", false);
            subCameraSetActive.enabled = false;
            CrossHair.SetActive(false);
        }

    }


　　/// <summary>カメラに関する処理</summary>
    private void CameraController() 
    {
        float xRot = Input.GetAxis("Mouse X") * Ysensityvity;
        float yRot = Input.GetAxis("Mouse Y") * Xsensityvity;
        #region リコイル
        if (Input.GetMouseButton(0) && shotCs.shotCount > 0 && !reloCs.ReloadBool)
        {
            //リコイルパーターンを作る
            Sumx = Random.Range(-0.1f, 0.1f);
            Sumy = Random.Range(-0.1f, 0);
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
        //カメラの回転
        if (!Input.GetMouseButton(1) || Weapon.Knife == _weaponType)
        {

            cameraRot *= Quaternion.Euler(-yRot, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot, 0);
            //cam.transform.rotation = subCamera.transform.rotation;
        }
        else if(Input.GetMouseButton(1) && Weapon.Gun == _weaponType)
        {

            subCameraRot *= Quaternion.Euler(-yRot + Sumy, 0, 0);
            characterRot *= Quaternion.Euler(0, xRot + Sumx + CamNumx, 0);
            //subCam.transform.rotation = cam.transform.rotation;
        }

        //Updateの中で作成した関数を呼ぶ
        if (!Input.GetMouseButton(1))
        {
            cameraRot = ClampRotation(cameraRot);
            cam.transform.localRotation = cameraRot;
        }
        else if(Input.GetMouseButton(1) && Weapon.Gun == _weaponType)
        {
            subCameraRot = ClampRotation(subCameraRot);
            subCamera.transform.localRotation = subCameraRot;
        }

        transform.localRotation = characterRot;

        CamNumx = 0;
        CamNumy = 0;
    }



    private void RollingFalse() 
    {
        _isRolling = false;
        _isDamege = false;
    }


    private void DamegeFalse() 
    {
       _isDamege = false;
        _isRolling = false;

    }
}
