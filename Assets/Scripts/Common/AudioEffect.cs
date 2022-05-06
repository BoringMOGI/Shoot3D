using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioEffect : MonoBehaviour, IObjectPool<AudioEffect>
{
    [SerializeField] AudioSource source;        // ���� ����� �ҽ�.

    // ��������Ʈ : �Լ��� ������ ����.
    // ����������, delegate, ��ȯ��, ��������Ʈ��, �Ű�����.
    // public delegate void ReturnPoolEvent(AudioEffect se);
    // public event ReturnPoolEvent onReturn;

    ReturnPoolEvent<AudioEffect> onReturn;

    public void PlaySE(AudioClip clip)
    {
        source.clip = clip;         // �ŰԺ����� ���� clip�� source�� ����.
        source.loop = false;        // loop�ɼ� ��Ȱ��ȭ.
        source.Play();              // source�� ����Ѵ�.

        StartCoroutine(CheckPlay());
    }
    IEnumerator CheckPlay()
    {
        while (source.isPlaying)        // ���� �÷��� ���̶��
            yield return null;          // 1������ ���.

        onReturn?.Invoke(this);         // ��ϵ� �̺�Ʈ�� ���� ��ȯ.
    }

    public void Setup(ReturnPoolEvent<AudioEffect> onReturn)
    {
        this.onReturn = onReturn;
    }
}
