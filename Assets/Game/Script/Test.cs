using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    int b = 0;
    void Start()
    {
        aaa(b);
        aaa(b);
        aaa(b);
        Debug.Log(b);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void aaa(int b)
    {
        ++b; 
    }
}
