using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveCamera : MonoBehaviour
{
    private Animator animator;
    private Vector3 defaultPos;
    [SerializeField] float wave;
    [SerializeField] private Shooting shootingcs;
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && shootingcs.shotCount > 0)
        {
            transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-wave, wave), transform.localPosition.y + Random.Range(-wave, wave), transform.localPosition.z);

        }
        else
        {
            transform.localPosition = defaultPos;
   
        }

    }

}
