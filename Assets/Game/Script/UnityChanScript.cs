using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnityChanScript : MonoBehaviour
{
    private CharacterController characterController;
    private Animator animator;
    //キャラクターの速度
    private Vector3 velocity;
    //キャラクターの歩くスピード
    [SerializeField] private float walkSpeed = 2f;
    //キャラクターの走るスピード
    [SerializeField] private float runSpeed = 4f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject ShootingObject;
    [SerializeField]private float shotSpeed;
    [SerializeField] private GameObject subCamera; //サブカメラ格納用
    private int shortCount = 30;
    private float shotInterval;
    
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            shotInterval += 1;

            if(shotInterval % 5 == 0 && shortCount > 0)
            {
                shortCount -= 1;

                GameObject bullet = (GameObject)Instantiate(bulletPrefab, ShootingObject.transform.position, Quaternion.Euler(-40, 88, 55));
                Rigidbody bulletRd = bullet.GetComponent<Rigidbody>();
                bulletRd.AddForce(ShootingObject.transform.forward * shotSpeed);

                //射撃されてから３秒後に銃弾のオブジェクトを破壊する
                Destroy(bullet, 3.0f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            shortCount = 30;
        }
    }

    private void FixedUpdate()
    {
        if (characterController.isGrounded)
        {
            velocity = Vector3.zero;
            var input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if(input.magnitude > 0.1f)
            {
                if (!Input.GetMouseButton(1))
                {
                    transform.LookAt(transform.position + input.normalized);
                }
                animator.SetFloat("Speed", input.magnitude);
                if(input.magnitude > 0.5)
                {
                    velocity += transform.forward * runSpeed;
                }
                else
                {
                    velocity += transform.forward * walkSpeed;
                }
            }
            else
            {
                animator.SetFloat("Speed", 0f);
            }

          
        }
        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            animator.SetBool("Bool", true);
            subCamera.SetActive(true);
        }
        else
        {
            animator.SetBool("Bool", false);
            subCamera.SetActive(false);
        }
    }

 
}
