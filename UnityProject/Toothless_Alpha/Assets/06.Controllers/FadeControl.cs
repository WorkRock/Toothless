using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeControl : MonoBehaviour
{
    [Header("LobbyIntro")]
    public float animTime;         // Fade 애니메이션 재생 시간 (단위:초).  
    private Image fadeImage;
    private Text fadeText;            // UGUI의 Image컴포넌트 참조 변수.  
    private float start = 1f;           // Mathf.Lerp 메소드의 첫번째 값.  
    private float end = 0f;             // Mathf.Lerp 메소드의 두번째 값.  
    private float time = 0f;            // Mathf.Lerp 메소드의 시간 값.

    [SerializeField] private bool isImage; // Image인지 Text인지 판단
    [SerializeField] private float stopTime;
    [SerializeField] private bool isBlackBG;
    [SerializeField] private bool isIntroEnd;

    [SerializeField] private bool stopIn; //false일때 실행되는건데, 초기값을 false로 한 이유는 게임 시작할때 페이드인으로 들어가려고...그게 싫으면 true로 하면됨.
    [SerializeField] private bool stopOut;

    void Awake()
    {
        fadeImage = GetComponent<Image>();
        fadeText = GetComponent<Text>();
        // Image 컴포넌트를 검색해서 참조 변수 값 설정.  

    }

    void Update()
    {
        if (isImage)
        {
            // 투명해지는 = FadeIn 애니메이션 재생.  
            if (stopIn == false && time <= animTime)
            {
                if (isBlackBG)
                {
                    Invoke("PlayFadeIn", stopTime);
                }

                else
                {
                    PlayFadeIn();
                }

            }

            if (stopOut == false && time <= animTime)
            {
                PlayFadeOut();
            }

            if (time >= animTime && stopIn == false)
            {
                stopIn = true;
                stopOut = false;
                if (!isIntroEnd && !isBlackBG)
                {
                    time = 0;
                    isIntroEnd = true;
                }
            }

            if (time >= animTime && stopOut == false)
            {
                stopIn = false; //하얗게 전환되고 나서 씬 전환 후 다시 풀거라 넣었다. 그냥 게임 끝낼거면 넣을 필요 없음.

                stopOut = true;

                if (!isIntroEnd && !isBlackBG)
                {
                    time = 0;
                    isIntroEnd = true;
                }
            }
        }

        else
        {
            // 투명해지는 = FadeIn 애니메이션 재생.  
            if (stopIn == false && time <= 2)
            {
                PlayFadeInText();
            }

            if (stopOut == false && time <= 2)
            {
                PlayFadeOutText();
            }

            if (time >= 2 && stopIn == false)
            {
                stopIn = true;
                stopOut = false;
                time = 0;
            }

            if (time >= 2 && stopOut == false)
            {
                stopIn = false; //하얗게 전환되고 나서 씬 전환 후 다시 풀거라 넣었다. 그냥 게임 끝낼거면 넣을 필요 없음.

                stopOut = true;
                time = 0;
            }
        }


    }


    // 흰색->투명
    void PlayFadeIn()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeImage.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(start, end, time);
        // 계산한 알파 값 다시 설정.  
        fadeImage.color = color;
        // Debug.Log(time);
    }


    // 투명->흰색
    void PlayFadeOut()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeImage.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(end, start, time);  //FadeIn과는 달리 start, end가 반대다.
        // 계산한 알파 값 다시 설정.  
        fadeImage.color = color;
    }

    void PlayFadeInText()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeText.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(start, end, time);
        // 계산한 알파 값 다시 설정.  
        fadeText.color = color;
        // Debug.Log(time);
    }


    // 투명->흰색
    void PlayFadeOutText()
    {
        // 경과 시간 계산.  
        // 2초(animTime)동안 재생될 수 있도록 animTime으로 나누기.  
        time += Time.deltaTime / animTime;

        // Image 컴포넌트의 색상 값 읽어오기.  
        Color color = fadeText.color;
        // 알파 값 계산.  
        color.a = Mathf.Lerp(end, start, time);  //FadeIn과는 달리 start, end가 반대다.
        // 계산한 알파 값 다시 설정.  
        fadeText.color = color;
    }
}
