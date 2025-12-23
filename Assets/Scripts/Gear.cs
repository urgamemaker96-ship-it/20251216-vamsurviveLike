using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private ObjectType objType;
    public ObjectType ObjType => objType;
    private float rate;
    private float speed;

    public void Init(ItemData data)
    {
        // Basic Set
        name = $"Gear {data.ObjType}";
        transform.parent = GameManager.Instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        objType = data.ObjType;
    }

    public void LevelUp(float rate)
    {
        this.rate += rate;
        ApplyGear();
    }

    private void ApplyGear()
    {
        switch (objType)
        {
            case ObjectType.Glove0:
                RateUp();
                break;
            case ObjectType.Shoe0:
                SpeedUp();
                break;
            default:
                break;
        }
    }

    private void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        foreach (Weapon weapon in weapons)
        {
            speed = weapon.OriginSpeed; // Weapon의 초기 속도 값

            switch (weapon.ObjType)
            {
                // Item으로 인한 증감량 + 캐릭터 고유 능력으로 인한 증감량 적용
                case ObjectType.Melee0:
                    speed *= (rate + Character.WeaponSpeed);
                    break;
                case ObjectType.Range0:
                    speed *= (Character.WeaponRate - rate);
                    break;
            }

            weapon.RateUp(speed);
        }
    }

    private void SpeedUp()
    {
        Player player = GameManager.Instance.Player;
        speed = player.OriginMoveSpeed;
        speed *= (Character.Speed + rate);
        player.SpeedUp(speed);
    }
}
