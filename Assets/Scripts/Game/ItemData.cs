using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "GameData/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public string description;
    public Sprite itemSprite;

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(string.Concat("이름:", itemName));
        builder.AppendLine(string.Concat("설명:", description));
        return builder.ToString();
    }
}
