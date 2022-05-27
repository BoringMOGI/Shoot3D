using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IInteraction
{
    [SerializeField] Item item;
    [SerializeField] Rigidbody rigid;

    public string GetContext()
    {
        return item.itemName;
    }
    public void OnInteracation()
    {
        Inventory.Instance.AddItem(item);
        Destroy(gameObject);
    }

    public void Setup(Item item)
    {
        this.item = item;
    }
    public void Throw(Vector3 direction, float power)
    {
        rigid.AddForce(direction * power, ForceMode.Impulse);
    }
}
