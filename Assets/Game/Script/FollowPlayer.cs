using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // プレイヤーを追従する
    [SerializeField] private GameObject player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private GameObject subCamera;
    [SerializeField] private float rotateSpeed = 2.0f;
    // Start is called before the first frame update
    Vector3 targetPos;

    void Start()
    {
    
        targetPos = player.transform.position;
    }

    void Update()
    {
        transform.position = player.transform.position + offset;
        // targetの移動量分、自分（カメラ）も移動する
        transform.position += player.transform.position - targetPos;
        targetPos = player.transform.position;


            // マウスの移動量
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            // targetの位置のY軸を中心に、回転（公転）する
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            // カメラの垂直移動（※角度制限なし、必要が無ければコメントアウト）
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 200f);
        
    }
}
