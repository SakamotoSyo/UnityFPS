using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager
{

    private static PlayerManager _instance = new PlayerManager();

    //�J�v�Z����
    public static PlayerManager Instance => _instance;

    private GameObject _player;

    public GameObject Player => _player;

    public void SetPlayer(GameObject player) 
    {
        _player = player;
    }

    

}
   



