using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneScript : MonoBehaviour
{
    [SerializeField] private GameObject _wayOfPlaying;
  
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            _wayOfPlaying.SetActive(false);
        }
    }

    public void NextScene() 
    {
        Debug.Log("yobareya");
        SceneManager.LoadScene("MainScene");
    }

    public void WayOfPlaying() 
    {
        _wayOfPlaying.SetActive(true);
    }
}
