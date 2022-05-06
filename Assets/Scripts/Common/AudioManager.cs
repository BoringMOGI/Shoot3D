using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ObjectPool<AudioManager, AudioEffect>
{
    [SerializeField] AudioClip[] effects;           // 효과음 배열.
    AudioSource audioSource;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
      
    }

    public void PlayBGM()
    {
        audioSource.Play();         // BGM을 재생하라.
    }
    public void StopBGM()
    {
        audioSource.Stop();         // BGM을 멈춰라.
    }
    public void PlaySE(string name)
    {
        // effects배열 순회.
        for(int i = 0; i< effects.Length; i++)
        {
            // i번째의 이름이 매개변수 name과 같으면.
            if(effects[i].name == name)
            {
                AudioClip clip = effects[i];                    // effects의 i번째 대입.
                AudioEffect effect = GetPool();                 // 효과음 오브젝트 꺼내기.
                effect.PlaySE(clip);                            // clip 전달, 재생.
                break;
            }
        }
    }

}
