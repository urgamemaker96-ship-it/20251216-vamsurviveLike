using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public Scanner Scanner { get; private set; }
    private Hand[] hands;
    [SerializeField] private RuntimeAnimatorController[] runtimeAniCon;

    private int playerId;

    [SerializeField] private float maxHp;
    [SerializeField] private float currentHp;

    [SerializeField] private float originMoveSpeed = 5f; // Player의 초기 속도
    public float OriginMoveSpeed => originMoveSpeed;
    [SerializeField] private float moveSpeed; // Player의 현재 속도
    private Vector2 moveDir;

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        Scanner = GetComponentInChildren<Scanner>();
        hands = GetComponentsInChildren<Hand>(true); // 인자로 true를 주면 비활성화 Object도 Get
    }

    private void OnEnable()
    {
        animator.runtimeAnimatorController = runtimeAniCon[GameManager.Instance.PlayerId];
    }

    private void Start()
    {
        // Player 직업 별 고유 속도 적용
        moveSpeed = originMoveSpeed * Character.Speed;
        currentHp = maxHp;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameStop)
        {
            return;
        }

        Move();
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameStop)
        {
            return;
        }

        Flip();
        SetAnimation();
    }

    private void Move()
    {
        rigidbody2d.linearVelocity = moveDir * moveSpeed;
    }

    private void OnMove(InputValue value)
    {
        //인풋 시스템 프로세스에서 vector2 로 설정해줫기 때문에 정규화 안시켜줘도 된다, 대각선 이동도 속도가 똑같음.
        moveDir = value.Get<Vector2>();
    }

    // Player 방향 전환 시 Sprite 반전
    private void Flip()
    {
        if (moveDir.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveDir.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    public void OnHit(float damage)
    {
        if (GameManager.Instance.IsGameOver)
        {
            return;
        }

        currentHp -= damage;
        GameManager.Instance.GameInfoHud.UpdateHpSillder(currentHp, maxHp);

        if (currentHp < 0) // 사망
        {
            OnDeath();
        }
    }

    private void SetAnimation()
    {
        bool isMove = (moveDir.x != 0) || (moveDir.y != 0);
        animator.SetBool("isMove", isMove);
    }

    public void SpeedUp(float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
    }

    public void GetHealItem()
    {
        currentHp = maxHp;
        GameManager.Instance.GameInfoHud.UpdateHpSillder(currentHp, maxHp);
    }

    public void ShowHand(ItemData data)
    {
        // hand[0] = (오른손, 근접 무기), hand[1] = (왼손, 원거리 무기)
        int handIndex = (data.ItemType == ObjectType.Melee ? 0 : 1);

        hands[handIndex].SetHandSprite(data.HandSprite);
        hands[handIndex].gameObject.SetActive(true);
    }

    private void OnDeath()
    {
        GameManager.Instance.GameOver(false);
        
        // Player 자식 중 Shadow와 Area를 제외한 Index=2 부터의 Object들 비활성화
        for (int index = 2; index < transform.childCount; index++)
        {
            transform.GetChild(index).gameObject.SetActive(false);
        }

        // 죽음 Animation 전환
        animator.SetTrigger("isDead");
    }
}
