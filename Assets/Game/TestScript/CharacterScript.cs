using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    //�ړ����x�BInspector���琔�l��ύX�ł���B�O���̃N���X����͕ύX�ł��Ȃ��B
    [SerializeField] float moveSpeed;

    //Rigidbody�^��playerRidigbody�ϐ���錾
    Rigidbody playerRigidbody;

    //�����^�̕ϐ����Q�錾
    float moveX, moveZ;
    //�^�U�l�^�̕ϐ����Q�錾
    bool moveZpermission, moveXpermission;

    // Start is called before the first frame update
    //�ŏ��̃t���[���������s����鏈��
    void Start()
    {
        //���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g(Player)��Rigidbody���擾
        playerRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    //���t���[�����s����鏈��
    void Update()
    {
        //�����^�̕ϐ��ɁAHorizontal�Ŏw�肵���L�[�ɑ΂��ē��͂��������ꍇ�A�x�����ɉ������߂�l�u-1�v�`�u1�v�����蓖�Ă���
        moveX = Input.GetAxis("Horizontal");
        //�����^�̕ϐ��ɁAVertical�Ŏw�肵���L�[�ɑ΂��ē��͂��������ꍇ�A�x�����ɉ������߂�l�u-1�v�`�u1�v�����蓖�Ă���
        moveZ = Input.GetAxis("Vertical");

        //Vertical�Ŏw�肵���L�[�ɑ΂��ē��͂��������ꍇ
        if (moveZ != 0)
        {
            //�O��Ɉړ����鏈�����J�n����ׂ̋����o��
            moveZpermission = true;
        }
        //Horizontal�Ŏw�肵���L�[�ɑ΂��ē��͂��������ꍇ
        if (moveX != 0)
        {
            //���E�Ɉړ����鏈�����J�n����ׂ̋����o��
            moveXpermission = true;
        }
    }

    void FixedUpdate()
    {
        //�O��������͍��E�Ɉړ����鏈���J�n�̋����o����A{}���̏��������s����
        if (moveZpermission || moveXpermission)
        {
            //�L�[���͂��������܂�{}���̏������Ă΂�Ȃ��悤�A�s���ɖ߂��Ă���
            moveZpermission = false;
            moveXpermission = false;
            //���̃X�N���v�g���A�^�b�`����Ă���I�u�W�F�N�g(Player)�̌�������Ƃ���X������Z�����Ɉړ�����悤�A
            //�P�t���[���̕ω��ʂɑΉ�������playerRigidbody�̑��x���㏑������
            //��]�ړ��h�~�ׁ̈A�C���X�y�N�^�[����FreezeRotation X Y Z �Ƀ`�F�b�N�����Ă���
            playerRigidbody.velocity = transform.rotation * new Vector3(moveX * moveSpeed, 0, moveZ * moveSpeed) * Time.deltaTime * 100;
        }
        else
        {
            //�ړ������o�Ă��Ȃ����͗͂������Ȃ��悤�Ƀ��Z�b�g����
            playerRigidbody.velocity = Vector3.zero;
        }
    }
}