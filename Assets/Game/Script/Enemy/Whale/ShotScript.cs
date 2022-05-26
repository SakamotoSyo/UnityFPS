using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotScript : MonoBehaviour
{
    private Rigidbody rb;

    //弾が地面にあたった後に出すプレハブ
    [SerializeField] private GameObject AfterAttack;
    //弾の速さ
    [SerializeField]private float ShotSpeed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * ShotSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            this.gameObject.SetActive(false);
            Instantiate(AfterAttack, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(this.gameObject, 5f);
        }
        if (other.CompareTag("Player"))
        {

        }
    }
}
