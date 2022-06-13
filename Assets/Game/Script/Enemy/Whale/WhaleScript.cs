using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleScript : MonoBehaviour
{
    private ZombieScript zombie;
    private GameObject m_UnityChan;
    
    private SphereCollider m_Collider;

    private void Start()
    {
        zombie = GetComponent<ZombieScript>();
        m_UnityChan = GameObject.Find("UnityChan");
        m_Collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player") && Vector3.Distance(this.gameObject.transform.position, m_UnityChan.transform.position) >= 1.5f )
        {
            zombie?.WhaleTriggerChack("Spit");
        }
        else if(other.gameObject.CompareTag("Player") && Vector3.Distance(this.gameObject.transform.position, m_UnityChan.transform.position) < 1.5f)
        {
            zombie?.WhaleTriggerChack("Tail");
        }
    }

    //�U���J�n���A�j���[�V��������Ă�
    private void TailActiveCollider() 
    {
        m_Collider.enabled = true;
    }

    //�U���I�����A�j���[�V��������Ă�
    private void InactiveCollider() 
    {
        m_Collider.enabled = false;
    }
}
