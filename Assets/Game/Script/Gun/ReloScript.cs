using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ReloScript : MonoBehaviour
{
    [SerializeField]private Shooting shooting;
    [SerializeField] private float ReloadInterval;
    private float ReloadWaitTime;
    [SerializeField] private TextMeshProUGUI bulletNumText;
    [SerializeField] private TextMeshProUGUI MaxBulletNumText;
    public bool ReloadBool = false;
    [Header("プレイヤーのアニメーション")]
    [SerializeField]private Animator _anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ReloadWaitTime += Time.deltaTime;
        if (ReloadInterval < ReloadWaitTime) 
        {
            if (ReloadBool && shooting.MaxBulletNum != 0) 
            {
                if (shooting.MaxBulletNum - (30 -shooting.shotCount) > 0)
                {
                    shooting.MaxBulletNum =  shooting.MaxBulletNum - (30 - shooting.shotCount);
                    shooting.shotCount = 30;
                   
                }
                else 
                {
                   shooting.shotCount = shooting.MaxBulletNum + shooting.shotCount;
                    shooting.MaxBulletNum = 0;
                }
                shooting.shotTime = 5000;
                shooting.ReloedTime = 500;
                
                bulletNumText.text = shooting.shotCount.ToString();
                MaxBulletNumText.text = shooting.MaxBulletNum.ToString();
                ReloadBool = false;
            }
            _anim.SetBool("ReloadBool", false);
        }
       
        if (Input.GetKeyDown(KeyCode.R) && !ReloadBool)
        {
            shooting.ReloedTime = 0;
            ReloadWaitTime = 0;
            ReloadBool = true;
            _anim.SetBool("ReloadBool", true);
        }
    }
}
