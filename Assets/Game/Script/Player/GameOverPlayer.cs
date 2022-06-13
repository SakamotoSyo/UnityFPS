using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _vcam2;
    [SerializeField] private UnityChanStatus _playStatus;

    private void Start()
    {
     
    }

    private void Update()
    {
        _vcam2.SetActive(true);

        if (Input.GetKey(KeyCode.Space))
        {
            _playStatus.SetHp(100);
            _playStatus.SetMoney(-_playStatus.GetMoney());
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
