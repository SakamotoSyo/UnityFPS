using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class PlayableDirectorScript : MonoBehaviour
{
    private PlayableDirector director;
    private Playable anim;
    [SerializeField] public PlayableAsset asset;
    // Start is called before the first frame update
    void Start()
    {
        director = GetComponent<PlayableDirector>();
        //director.Play(asset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
