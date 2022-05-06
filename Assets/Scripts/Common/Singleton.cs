using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour       // 일반화
    where T : MonoBehaviour                     // T자료형은 MonoBehaviour를 상속하고 있어야한다.
{

    // 싱글톤(Singleton)
    // => 어디에서든 해당 객체에 접근할 수 있는 디자인 패턴 중 하나.
    //    단, 해당 객체는 하나만 존재해야한다.
    static T instance;
    public static T Instance => instance;

    protected void Awake()
    {
        instance = this as T;       // this(Singleton인 나)를 T자료형으로 형 변환 시도.
    }
}
