using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 델리게이트, 인터페이스 일반화.
// => 델리게이트 자료형으로 선언한 변수는 "함수의 주소"를 담을 수 있다.
public delegate void ReturnPoolEvent<PoolType>(PoolType pool);

// 인터페이스는 약속이다.
// => 해당 인터페이스를 상속한 클래스는 인터페이스 내부의 함수를 "전부 구현" 해야한다.
public interface IObjectPool<PoolType>
{
    void Setup(ReturnPoolEvent<PoolType> onReturn);
}

// PoolType자료형은 Component상속하고 있어야만 한다.
// 추가로 IObjectPool 인터페이스를 구현하고 있어야 한다.
public class ObjectPool<ClassType, PoolType> : Singleton<ClassType>
    where ClassType : MonoBehaviour
    where PoolType : Component, IObjectPool<PoolType>
{
    [SerializeField] PoolType poolPrefab;
    [SerializeField] Transform storageParent;
    [SerializeField] int poolCount;

    Stack<PoolType> storage;

    private new void Awake()
    {
        base.Awake();
        storage = new Stack<PoolType>();
        for (int i = 0; i < poolCount; i++)
            CreatePool();
    }

    private void CreatePool()
    {
        PoolType newPool = Instantiate(poolPrefab); // PoolType이 뭔지 모르겠지만 적어도 Object를 상속하기 있기 때문에 복제 가능.
        newPool.transform.SetParent(storageParent); // 부모 오브젝트를 storageParent 오브젝트로 변경.
        newPool.Setup(OnReturnPool);                // 새로 만든 pool에 되돌아오는 이벤트 등록.
        storage.Push(newPool);                      // 스택에 저장.
    }

    private void OnReturnPool(PoolType pool)
    {
        pool.transform.SetParent(storageParent);    // 부모 오브젝트 변경.
        storage.Push(pool);                         // 저장소에 Push.
    }

    protected PoolType GetPool()
    {
        if (storage.Count <= 0)     // 저장소의 개수가 0개 이하라면.
            CreatePool();           // 하나 만들어서 넣는다.

        PoolType pool = storage.Pop();          // 저장소에서 꺼낸다.
        pool.transform.SetParent(transform);    // 부모 오브젝트 변경.
        return pool;                            // 반환.
    }


}
