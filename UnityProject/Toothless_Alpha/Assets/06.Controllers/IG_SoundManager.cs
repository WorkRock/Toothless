using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IG_SoundManager : MonoBehaviour
{
    //오디오 타입 구조체 선언
    [System.Serializable]
    public struct AudioType
    {
        public string name;
        public AudioClip audio;
    }

    public AudioSource audioSource1;
    public AudioSource audioSource2;
    //오디오 목록
    public AudioType[] AudioList;

    public int isSoundOn;
   
    void Start()
    {
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        isSoundOn = PlayerPrefs.GetInt("isSoundOn");
    }


    public void PlayAudio(string name)
    {
        if (isSoundOn == 0)
            return;

        for(int i = 0; i < AudioList.Length; i++)
        {
            if(AudioList[i].name.Equals(name))
            {
                audioSource1.clip = AudioList[i].audio;
                audioSource1.Play();
            }
        }
    }

    public void PlayAudio2(string name)
    {
        if (isSoundOn == 0)
            return;

        for (int i = 0; i < AudioList.Length; i++)
        {
            if (AudioList[i].name.Equals(name))
            {
                audioSource2.clip = AudioList[i].audio;
                audioSource2.Play();
            }
        }
    }
}
