using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // 각 Character 마다 고유한 능력 증가치를 가짐

    public static float Speed
    {
        get
        {
            // Player 속도 10% 증가
            return GameManager.Instance.PlayerId == 0 ? 1.1f : 1f;
        }
    }

    public static float WeaponSpeed
    {
        get
        {
            // 근접 무기 회전체 속도 10% 증가
            return GameManager.Instance.PlayerId == 1 ? 1.1f : 1f;
        }
    }

    public static float WeaponRate
    {
        get
        {
            // 연사 속도 10% 증가 (발사 간격 10% Down)
            return GameManager.Instance.PlayerId == 1 ? 0.9f : 1f;
        }
    }

    public static float Damage
    {
        get
        {
            // 기본 데미지 10% 증가
            return GameManager.Instance.PlayerId == 2 ? 1.1f : 1f;
        }
    }

    public static int Count
    {
        get
        {
            // 기본 회전체 및 관통력 1 증가
            return GameManager.Instance.PlayerId == 3 ? 1 : 0;
        }
    }
}
