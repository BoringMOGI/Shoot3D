using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameData/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemSprite;
}
