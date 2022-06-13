using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    [SerializeField] private RigidbodyUnityChan _player;
    [SerializeField]private BoxCollider _boxCollider;
    [SerializeField]private Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void KnifeStartEvent() 
    {
        _boxCollider.enabled = true;
    }
    private void KnifeEndEvent()
    {
        _boxCollider.enabled = false;
        _animator.SetBool("KnifeAttack", false);
    }

}
