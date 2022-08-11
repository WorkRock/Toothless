using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IG_SoundManager : MonoBehaviour
{
    //오디오 타입 구조체 선언
    [System.Serializable]   //구조체를 인스펙터에 보이게 함
    public struct AudioType
    {
        public string name;
        public AudioClip audio;
    }

    public AudioSource audioSource1;    //플레이어 음성
    public AudioSource audioSource2;    //ㅇ
    public AudioSource audioSource3;

    //오디오 목록
    public AudioType[] AudioList;

    private bool isSoundOn;
   
    void Start()
    {
        //소리 겹침 방지하기 위해 오디오 소스3 까지 사용
        audioSource1 = gameObject.AddComponent<AudioSource>();
        audioSource2 = gameObject.AddComponent<AudioSource>();
        audioSource3 = gameObject.AddComponent<AudioSource>();
    }

    void Update()
    {
        //게임 사운드 on/off 여부 지속적으로 체크
        isSoundOn = ScoreManager.GetIsSoundOn();
    }


    public void PlayAudio(string name)
    {
        //사운드 off 상태이면 return
        if (!isSoundOn)
            return;

        //사운드 on 이면 실행
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
        if (!isSoundOn)
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

    public void PlayAudio3(string name)
    {
        if (!isSoundOn)
            return;

        for (int i = 0; i < AudioList.Length; i++)
        {
            if (AudioList[i].name.Equals(name))
            {
                audioSource3.clip = AudioList[i].audio;
                audioSource3.Play();
            }
        }
    }
}
