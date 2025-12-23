using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [SerializeField] private ItemData data;
    public ItemData Data => data;
    [SerializeField] private int level = 0;
    public int Level => level;
    private Weapon weapon;
    private Gear gear;

    private Image iconImage;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI itemNameText;
    private TextMeshProUGUI itemDescText;

    private void Awake()
    {
        iconImage = GetComponentsInChildren<Image>()[1];
        iconImage.sprite = data.ItemIcon;

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        levelText = texts[0];
        itemNameText = texts[1];
        itemDescText = texts[2];

        itemNameText.SetText(data.ItemName);
    }

    public void UpdateItemDescript()
    {
        levelText.SetText($"Lv.{level + 1}");

        switch (data.ItemType)
        {
            case ObjectType.Melee:
            case ObjectType.Range:
                itemDescText.SetText(string.Format(data.ItemDesc, data.Damages[level], data.Counts[level]));
                break;
            case ObjectType.Glove:
            case ObjectType.Shoe:
                itemDescText.SetText(string.Format(data.ItemDesc, data.Damages[level] * 100));
                break;
            case ObjectType.Heal:
                itemDescText.SetText(string.Format(data.ItemDesc));
                break;
        }
    }

    public void OnButtonClicked()
    {
        switch (data.ItemType)
        {
            case ObjectType.Melee:
            case ObjectType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<Weapon>();
                    weapon.Init(data);
                }
                weapon.LevelUp(data.Damages[level], data.Counts[level]); // 현재 Level의 Damage와 Count 설정
                break;
            case ObjectType.Glove:
            case ObjectType.Shoe:
                if (level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<Gear>();
                    gear.Init(data);
                }
                gear.LevelUp(data.Damages[level]);  // 현재 Level의 Rate를 설정
                break;
            case ObjectType.Heal:
                GameManager.Instance.Player.GetHealItem();
                break;
        }

        if (data.ItemType != ObjectType.Heal)
        {
            level++;
            //UpdateItemDescript();
            if (level == data.Damages.Length)
            {
                GetComponent<Button>().interactable = false;
            }
        }

    }
}
