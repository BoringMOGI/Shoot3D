using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : Singleton<WeaponInfoUI>
{
    [SerializeField] Text maxBulletText;
    [SerializeField] Text currentBulletText;

    public void UpdateBulletText(int current, int max)
    {
        maxBulletText.text = max.ToString();
        currentBulletText.text = current.ToString();
    }
}
