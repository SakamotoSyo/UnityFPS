using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    /// <summary>
    /// �e��Prefab
    /// </summary>
    [SerializeField, Tooltip("�e��Prefab")]
    private GameObject bulletPrefab;

    /// <summary>
    /// �C�g�̃I�u�W�F�N�g
    /// </summary>
    [SerializeField, Tooltip("�C�g�̃I�u�W�F�N�g")]
    private GameObject barrelObject;

    /// <summary>
    /// �e�𐶐�����ʒu���
    /// </summary>
    private Vector3 instantiatePosition;
    /// <summary>
    /// �e�̐������W(�ǂݎ���p)
    /// </summary>
    public Vector3 InstantiatePosition
    {
        get { return instantiatePosition; }
    }

    /// <summary>
    /// �e�̑���
    /// </summary>
    [SerializeField, Range(1.0F, 20.0F), Tooltip("�e�̎ˏo���鑬��")]
    private float speed = 1.0F;

    /// <summary>
    /// �e�̏����x
    /// </summary>
    private Vector3 shootVelocity;
    /// <summary>
    /// �e�̏����x(�ǂݎ���p)
    /// </summary>
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }

    void Update()
    {
        // �e�̏����x���X�V
        shootVelocity = barrelObject.transform.up * speed;

        // �e�̐������W���X�V
        instantiatePosition = barrelObject.transform.position;

        // ����
        if (Input.GetKeyDown(KeyCode.G))
        {
            // �e�𐶐����Ĕ�΂�
            GameObject obj = Instantiate(bulletPrefab, instantiatePosition, Quaternion.identity);
            Rigidbody rid = obj.GetComponent<Rigidbody>();
            rid.AddForce(shootVelocity * rid.mass, ForceMode.Impulse);

            // 5�b��ɏ�����
            Destroy(obj, 5.0F);
        }
    }
}
