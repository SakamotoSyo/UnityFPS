using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadScript : MonoBehaviour
{
    //グレネードの爆発
    [SerializeField] private GameObject GrenadExploPrefab;
    [SerializeField]private float WaitTime;
    private float CountTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CountTime += Time.deltaTime;
        if (WaitTime < CountTime) 
        {
            Debug.Log("aaaa");
            this.gameObject.SetActive(false);
            var GrenadEx = Instantiate(GrenadExploPrefab, this.gameObject.transform.position, this.gameObject.transform.rotation);
            Destroy(this.gameObject, 5f);
            Destroy(GrenadEx, 5f);
        }
        
    }
}
