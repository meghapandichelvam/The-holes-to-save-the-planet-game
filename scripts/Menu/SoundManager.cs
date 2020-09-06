using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public AudioSource bgm;
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.GetInt("Volume") == 1)
        {
            bgm.volume = 1f;

        }else
        {
            bgm.volume = 0f;
        }
    }

   
}
