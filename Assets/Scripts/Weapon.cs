using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private ObjectType objType; // 이 Weapon의 Type;
    public ObjectType ObjType => objType;
    private Player player;

    private float baseDamage; // 기본 Damage
    private float damage; // 고유 능력으로 인한 최종 증감 Damage
    private int baseCount; // 기본 Count, 근접 무기 개수, 원거리 무기 관통력
    private int count; // 고유 능력으로 인한 최종 증감 Count
    private float originSpeed; // 초기 회전 속도, ShootInterval
    public float OriginSpeed => originSpeed;
    [SerializeField] private float speed; // 회전 속도, ShootInterval
    private float time = 0f;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameStop)
        {
            return;
        }
        
        // 시계 방향 회전
        switch (objType)
        {
            case ObjectType.Melee0: // 삽 (Bullet 0)
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case ObjectType.Range0: // (Bullet 3)
                time += Time.deltaTime;
                Fire();
                break;
            default:
                break;
        }
    }

    public void Init(ItemData data)
    {
        // Basic Set
        name = $"Weapon {data.ObjType}";
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        // Property Set
        objType = data.ObjType;

        switch (objType)
        {
            case ObjectType.Melee0: // 삽 (Bullet 0)
                originSpeed = 150f;
                speed = originSpeed * Character.WeaponSpeed;
                break;
            case ObjectType.Range0: // 엽총 (Bullet 3)
                originSpeed = 0.3f;
                speed = originSpeed * Character.WeaponRate;
                break;
            default:
                break;
        }

        player.ShowHand(data);
    }

    public void LevelUp(float damage, int count)
    {
        baseDamage += damage;
        this.damage = baseDamage * Character.Damage;
        baseCount += count;
        this.count = baseCount + Character.Count;

        if (objType == ObjectType.Melee0)
        {
            SetPosition();
        }

        // 첫 Weapon 생성 시 Gear가 먼저 존재할 때, Gear들의 효과를 모두 적용
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void RateUp(float speed)
    {
        this.speed = speed;
    }

    private void SetPosition()
    {
        AllDisalbe();

        for (int index = 0; index < count; index++)
        {
            Transform bullet = GameManager.Instance.PoolManager.GetObject(objType).transform;
            Vector3 rotateVec = new Vector3(0f, 0f, -360 / count * index); // 회전 방향

            // 기본값 초기화
            bullet.parent = transform;
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            // 방향 및 위치 설정
            bullet.Rotate(rotateVec);
            bullet.Translate(Vector3.up * 1.5f, Space.Self); // 각 Bullet의 방향을 기준으로 1.5만큼 y축 이동
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -1은 무한 관통 (근접 무기)
        }
    }

    // 자식 Object를 모두 비활성화
    private void AllDisalbe()
    {   
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void Fire()
    {
        // Scanner에 탐색된 Enemy가 있다면
        if ((time > speed) && player.Scanner.NearestTarget)
        {
            Transform bullet = GameManager.Instance.PoolManager.GetObject(objType).transform;

            // 기본값 초기화
            bullet.position = transform.position;
            bullet.rotation = Quaternion.identity;

            Vector3 fireDir = player.Scanner.NearestTarget.position - transform.position;
            fireDir = fireDir.normalized;

            // float angle = Mathf.Atan2(fireDir.y, fireDir.x) * 180f / Mathf.PI - 90f;
            // bullet.Rotate(Vector3.forward * angle);
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, fireDir);

            bullet.GetComponent<Bullet>().Init(damage, count, fireDir);

            time = 0f;

            AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.Range);
        }
    }
}
