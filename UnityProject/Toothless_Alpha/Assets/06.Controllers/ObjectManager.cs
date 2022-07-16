using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    //생성할 오브젝트 프리팹 연결
    public GameObject Dragon_Atk_FirePref;

    //오브젝트 풀 생성
    GameObject[] Dragon_Atk_Fires;

    //타겟풀 생성
    GameObject[] targetPool;


    void Awake()
    {
        // 1. 최대 등장할 오브젝트 갯수만큼 풀 크기 설정
        Dragon_Atk_Fires = new GameObject[10];

        // 2. 생성
        Generate();
    }

    //풀에 생성만 해놓고 비활성화 해놓는 함수
    public void Generate()
    {
        for(int i = 0; i < Dragon_Atk_Fires.Length; i++)
        {
            Dragon_Atk_Fires[i] = Instantiate(Dragon_Atk_FirePref);
            Dragon_Atk_Fires[i].SetActive(false);
        }
    }

    //외부에서 오브젝트 풀에 접근할 함수
    public GameObject MakeObj(string type)
    {
        //프리팹의 타입 검사해서 타겟풀에 해당 오브젝트 풀을 할당
        switch (type)
        {
            case "Fire":
                targetPool = Dragon_Atk_Fires;
                break;
        }

        for(int i = 0; i < targetPool.Length; i++)
        {
            //타겟풀의 각 오브젝트들을 검사해서, 비활성화 상태인 경우 1. 활성화 시킨 후 2. 리턴
            if(!targetPool[i].activeSelf)
            {
                targetPool[i].SetActive(true);
                return targetPool[i];
            }
        }

        return null;    //예외 처리 -> 현재 비활성화된 오브젝트가 없는 경우 반환 x
    }
}
