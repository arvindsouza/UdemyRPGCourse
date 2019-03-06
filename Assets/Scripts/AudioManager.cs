using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource[] sfx, bgMusic;

    public static AudioManager instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaySFX(int soundToPlay)
    {
        if(soundToPlay < sfx.Length)
        sfx[soundToPlay].Play();
    }

    public void PlayBgMusic(int musicToPlay)
    {
        if (!bgMusic[musicToPlay].isPlaying)
        {
            StopMusic();
            if (musicToPlay < bgMusic.Length)
                bgMusic[musicToPlay].Play();
        }
    }

    public void StopMusic()
    {
        for(int i = 0; i < bgMusic.Length; i++)
        {
            bgMusic[i].Stop();
        }
    }
}
