using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("# Main Info")]
    [SerializeField] private ObjectType itemType;
    public ObjectType ItemType => itemType;
    [SerializeField] private ObjectType objType;
    public ObjectType ObjType => objType;
    [SerializeField] private string itemName;
    public string ItemName => itemName;
    [TextArea]
    [SerializeField] private string itemDesc; // Item 설명
    public string ItemDesc => itemDesc;
    [SerializeField] private Sprite itemIcon;
    public Sprite ItemIcon => itemIcon;

    [Header("# Level Data")]
    [SerializeField] private float[] damages; // Weapon이면 Level마다 Damage 증가 수, Gear면 공속, 이속 증가율
    public float[] Damages => damages;
    [SerializeField] private int[] counts; // Level마다 Count 증가 수
    public int[] Counts => counts;

    [Header("# Weapon")]
    [SerializeField] private GameObject projectile;
    [SerializeField] private Sprite handSprite;
    public Sprite HandSprite => handSprite;
}