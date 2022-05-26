using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieScript : MonoBehaviour
{
    [SerializeField][Tooltip("追いかける対象")]private GameObject player;
    [SerializeField] private RigidbodyUnityChan rbUnity;
    public EnemyStatus enemyStatus;
    //WhaleEnemy
    [SerializeField]private GameObject WhaleAttackPrefab;
    [SerializeField]private GameObject WhaleShotingObject;

    private NavMeshAgent navMeshAgent;
    private Animator anim;

    private bool isDamage = false;
    private bool isAttack = false;
    private bool m_TailAttack = false;
    //プレイヤーを攻撃したかどうか
    //private bool playerAttack = false;
    private bool DamageAnim;
    //ダメージモーションが始まったかどうか
    private bool DamegeFrag = false;
    //アタックモーションが始まったかどうか
    private bool AttackFrag = false;

    private float waitTime = 3f;
    private float countTime = 0;

    private void Start()
    {
        player = GameObject.Find("UnityChan");
        rbUnity = player.GetComponent<RigidbodyUnityChan>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        navMeshAgent.speed = 2f;
        
    }

    private void Update()
    {
        if (!DamageAnim)
        {
            //navMeshAgent.destination = player.transform.position;
            navMeshAgent.SetDestination(player.transform.position);
        }
        anim.SetFloat("Speed", navMeshAgent.desiredVelocity.magnitude);
        anim.SetBool("Damege", isDamage);
        anim.SetBool("Attack", isAttack);
        anim.SetBool("TailAttack", m_TailAttack);

        
        countTime += Time.deltaTime;
    }

    /// <summary>
    /// shotingScriptから呼び出す。銃弾を受けた時確率でダメージアニメーションを再生
    /// </summary>
    public void BulletHit()
    {
        var RandomCount = Random.Range(0, 100);
       
            if(RandomCount < 5)
            {
                isDamage = true;
                DamegeFrag = true;
            }
    }

    private void OnTriggerStay(Collider other)
    {

       

            if (other.gameObject.CompareTag("Player") && !AttackFrag && waitTime < countTime && this.gameObject.tag == "Zombie")
            {
                isAttack = true;
                AttackFrag = true;
                countTime = 0;
               
            }
        
        
    }

    /// <summary>
    /// アニメーションイベントから呼び出す。銃弾や攻撃のアニメーションが再生し終わった時の処理
    /// </summary>
    private void Damagefalse()
    {
        if (DamegeFrag)
        {
            isDamage = false;
            navMeshAgent.speed = 2f;
            DamegeFrag = false;
        }

        if (AttackFrag)
        {
            isAttack = false;
            m_TailAttack = false;
            navMeshAgent.speed = 2f;
            AttackFrag = false;        
        }

        rbUnity.EnemyAttack = false;
           
    }

    /// <summary>
    /// アニメーションイベントから呼び出す。銃弾を受けたときアニメーションの間動きを止める
    /// </summary>
    private void StartDamege()
    {
        if(DamegeFrag)
        {
            navMeshAgent.speed = 0f;
        }
        else if (AttackFrag)
        {
            navMeshAgent.speed = 0f;
        }
    }

    /// <summary>
    /// Attackのアニメーションイベントから呼び出す。playerへの攻撃処理
    /// </summary>
    private void Hit()
    {
        //var player_cs = player.GetComponent<RigidbodyUnityChan>();
        rbUnity.EnemyAttack = true;
        
    }

    //サメ専用の攻撃モーション
    public void SpitoutAttack()
    {
        GameObject attack = Instantiate(WhaleAttackPrefab, WhaleShotingObject.gameObject.transform.position, WhaleShotingObject.gameObject.transform.rotation);
        navMeshAgent.speed = 0f;
    }

    //WhaleScriptから呼び出す
    public void WhaleTriggerChack(string name)
    {
        if (!AttackFrag && waitTime < countTime && name == "Spit")
        {
            isAttack = true;
            AttackFrag = true;
            countTime = 0;

        }
        else if (!AttackFrag && waitTime < countTime && name == "Tail") 
        {
            m_TailAttack = true;
            AttackFrag = true;
            countTime = 0;
        }
    }
}
