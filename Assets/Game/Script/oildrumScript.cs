using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class oildrumScript : MonoBehaviour
{
    private int DestroyCount = 0;
    [SerializeField] GameObject ExproPefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bullet") 
        {
            DestroyCount++;
            if (DestroyCount > 10) 
            {
                this.gameObject.SetActive(false);
                var Ins = Instantiate(ExproPefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
                Destroy(this.gameObject, 5f);
            }
        }
    }
}
