using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : ObjectPool<AudioManager, AudioEffect>
{
    [SerializeField] AudioClip[] effects;           // ȿ���� �迭.
    AudioSource audioSource;
   
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
      
    }

    public void PlayBGM()
    {
        audioSource.Play();         // BGM�� ����϶�.
    }
    public void StopBGM()
    {
        audioSource.Stop();         // BGM�� �����.
    }
    public void PlaySE(string name)
    {
        // effects�迭 ��ȸ.
        for(int i = 0; i< effects.Length; i++)
        {
            // i��°�� �̸��� �Ű����� name�� ������.
            if(effects[i].name == name)
            {
                AudioClip clip = effects[i];                    // effects�� i��° ����.
                AudioEffect effect = GetPool();                 // ȿ���� ������Ʈ ������.
                effect.PlaySE(clip);                            // clip ����, ���.
                break;
            }
        }
    }

}
