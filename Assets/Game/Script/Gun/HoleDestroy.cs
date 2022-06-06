using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDestroy : MonoBehaviour
{
    // Start is called before the first frame update
    private float timeCount = 0;
    private float waitTime = 5;
    [SerializeField] private int shotPower; 

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;

        if(timeCount > waitTime)
        {
            Destroy(this.gameObject);
        }
       

    }

    private void OnCollisionEnter(Collision collision)
    {
       
            Destroy(this.gameObject);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*//raycastに処理を移した
        //敵の体力を減らす処理
        if (other.gameObject.CompareTag("Zombie"))
        {
            var EnemyScript = other.gameObject.GetComponent<ZombieScript>();
            EnemyScript.enemyStatus.DamageHp(shotPower);
            if (EnemyScript.enemyStatus.GetHp() < 0)
            {
                Destroy(other.gameObject);
            }
            //死亡用（アニメーション）のプレハブを出す
        }
        */
    }
}
