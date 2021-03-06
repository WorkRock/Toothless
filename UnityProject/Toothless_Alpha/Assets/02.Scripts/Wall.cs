using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    //배경 스크롤 속도
    public float scrollSpeed = 0.5f;
    //머터리얼 객체 생성
    Material myMaterial;

    // Start is called before the first frame update
    void Start()
    {
        //렌더러의 머터리얼 값을 객체에 연결
        myMaterial = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        //무한 배경
        float newOffSetX = myMaterial.mainTextureOffset.x - scrollSpeed * Time.deltaTime;
        Vector3 newOffset = new Vector3(newOffSetX, 0, 0);

        myMaterial.mainTextureOffset = newOffset;
    }
}
