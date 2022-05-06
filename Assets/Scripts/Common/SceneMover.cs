using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;                       // UI 관련 네임스페이스.
using UnityEngine.SceneManagement;          // 씬 관리 네임스페이스.

public class SceneMover : MonoBehaviour
{
    [SerializeField] Image blindImage;
    [SerializeField] float fadeTime;

    public static bool isFading
    {
        get;
        private set;
    }

    /*
    bool isFading;                          // 화면 전환중인가?
    public bool IsFading => isFading;       // 프로퍼티로 외부 노출. (readonly)
    */

    private void Start()
    {
        StartCoroutine(FadeIn());
    }

    /*
    [ContextMenu("FadeOut")]                // Component를 오른쪽 클릭했을 때 나오는 메뉴.
    public void OnFadeOut()
    {
        if (isFading)
            return;

        StartCoroutine(FadeOut());          // FadeOut 코루틴 실행.
    }
    [ContextMenu("FadeIn")] 
    public void OnFadeIn()
    {
        if (isFading)
            return;

        StartCoroutine(FadeIn());
    }
    */

    public void NextScene(string sceneName)
    {
        if (isFading)
            return;

        StartCoroutine(FadeOut(sceneName));
    }
    IEnumerator FadeOut(string sceneName)
    {
        // n(fadeTime)초 동안 blindImage의 a값이 0에서 1로 변경된다.
        isFading = true;

        float MAX_TIME = fadeTime;          // 화면 전환 시간.
        float time = 0f;                    // 타이머.
        Color color = blindImage.color;     // 원색상.

        blindImage.enabled = true;          // blindImage를 활성화한다.

        do
        {
            //Mathf.Clmap(값, 최소값, 최대값) : 값을 최소, 최대에 고정시킨 후 반환.
            time = Mathf.Clamp(time + Time.deltaTime, 0.0f, MAX_TIME);

            color.a = time / MAX_TIME;      // 시간의 흐름에 따른 비율 계산.
            blindImage.color = color;       // blindImage에 색상 대입.
            yield return null;              // 1프레임 쉰다(로 일단 이해하자)
        }
        while (time < MAX_TIME);            // time값이 maxTime보다 적으면 다시 do문 실행.
        
        SceneManager.LoadScene(sceneName);
    }
    IEnumerator FadeIn()
    {
        // n(fadeTime)초 동안 blindImage의 a값이 1에서 0로 변경된다.
        isFading = true;

        float MAX_TIME = fadeTime;          // 화면 전환 시간.
        float time = fadeTime;              // 타이머.
        Color color = blindImage.color;     // 원색상.

        blindImage.enabled = true;          // blindImage를 활성화한다.

        do
        {
            //Mathf.Clmap(값, 최소값, 최대값) : 값을 최소, 최대에 고정시킨 후 반환.
            time = Mathf.Clamp(time - Time.deltaTime, 0.0f, MAX_TIME);

            color.a = time / MAX_TIME;      // 시간의 흐름에 따른 비율 계산.
            blindImage.color = color;       // blindImage에 색상 대입.
            yield return null;              // 1프레임 쉰다(로 일단 이해하자)
        }
        while (time > 0.0f);                // time값이 maxTime보다 적으면 다시 do문 실행.

        blindImage.enabled = false;         // blindImage를 비활성화한다.
        isFading = false;                   // 화면 전환 여부 false.
    }
}
