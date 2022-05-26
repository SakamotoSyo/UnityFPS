using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleScript : MonoBehaviour
{
    [SerializeField] private ZombieScript zombie;
    [SerializeField] private GameObject m_UnityChan;
    
    [SerializeField] private SphereCollider m_Collider;

    private void OnTriggerStay(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player") && Vector3.Distance(this.gameObject.transform.position, m_UnityChan.transform.position) >= 1.5f)
        {
            zombie.WhaleTriggerChack("Spit");
        }
        else
        {
            zombie.WhaleTriggerChack("Tail");
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
