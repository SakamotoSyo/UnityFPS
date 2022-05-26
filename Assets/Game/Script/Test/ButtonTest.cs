using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTest : MonoBehaviour
{
    [SerializeField]
    Button _button;

    private void Start()
    {
        _button.onClick.AddListener(()=> Debug.Log("aaa")) ;
        
    }

    public void Output()
    {
        print("aaa");
    }
}
