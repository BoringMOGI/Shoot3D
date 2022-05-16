using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInfoUI : Singleton<WeaponInfoUI>
{
    [SerializeField] Text maxBulletText;
    [SerializeField] Text currentBulletText;
    [SerializeField] Text fireTypeText;
    [SerializeField] Text grenadeCountText;

    public void UpdateBulletText(int current, int max)
    {
        maxBulletText.text = max.ToString();
        currentBulletText.text = current.ToString();
    }
    public void UpdateFireType(string str)
    {
        fireTypeText.text = str;
    }
    public void UpdateGrenadeCount(int count)
    {
        grenadeCountText.text = count.ToString();
    }
}
