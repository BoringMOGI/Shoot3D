using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 컴포넌트가 붙어 있어야 함을 강제한다.
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
