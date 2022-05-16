using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ش� ������Ʈ�� �پ� �־�� ���� �����Ѵ�.
[RequireComponent(typeof(ParticleSystem))]
public class AudoDestroyEffect : MonoBehaviour
{
    ParticleSystem particle;
    private void Start()
    {
        particle = GetComponent<ParticleSystem>();
    }
    private void Update()
    {
        if (!particle.isPlaying)
            Destroy(gameObject);
    }
}
