using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    // �v���C���[��Ǐ]����
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
        // target�̈ړ��ʕ��A�����i�J�����j���ړ�����
        transform.position += player.transform.position - targetPos;
        targetPos = player.transform.position;


            // �}�E�X�̈ړ���
            float mouseInputX = Input.GetAxis("Mouse X");
            float mouseInputY = Input.GetAxis("Mouse Y");
            // target�̈ʒu��Y���𒆐S�ɁA��]�i���]�j����
            transform.RotateAround(targetPos, Vector3.up, mouseInputX * Time.deltaTime * 200f);
            // �J�����̐����ړ��i���p�x�����Ȃ��A�K�v��������΃R�����g�A�E�g�j
            transform.RotateAround(targetPos, transform.right, mouseInputY * Time.deltaTime * 200f);
        
    }
}
