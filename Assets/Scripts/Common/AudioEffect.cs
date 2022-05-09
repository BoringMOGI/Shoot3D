using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour, IObjectPool<AudioEffect>
{
    [SerializeField] AudioSource source;        // 나의 오디오 소스.

    // 델리게이트 : 함수를 가지는 변수.
    // 접근제한자, delegate, 반환형, 델리게이트명, 매개변수.
    // public delegate void ReturnPoolEvent(AudioEffect se);
    // public event ReturnPoolEvent onReturn;

    ReturnPoolEvent<AudioEffect> onReturn;

    public void PlaySE(AudioClip clip, float volumn)
    {
        source.clip = clip;         // 매게변수로 받은 clip을 source에 삽입.
        source.loop = false;        // loop옵션 비활성화.
        source.volume = volumn;     // 볼륨.
        source.Play();              // source를 재생한다.

        StartCoroutine(CheckPlay());
    }
    IEnumerator CheckPlay()
    {
        while (source.isPlaying)        // 만약 플레이 중이라면
            yield return null;          // 1프레임 대기.

        onReturn?.Invoke(this);         // 등록된 이벤트를 통해 반환.
    }

    public void Setup(ReturnPoolEvent<AudioEffect> onReturn)
    {
        this.onReturn = onReturn;
    }
}
