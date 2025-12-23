using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Collider2D collider2d;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private Player player;

    private ObjectType objectType; // 해당 Enemy가 어떤 종류인지

    private float maxHp;
    [SerializeField] private float currentHp;

    private Vector2 dirVec;
    private float moveSpeed;

    private bool isLive; // Enemy가 살아있는지 아닌지

    private void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver)
        {
            rigidbody2d.linearVelocity = Vector2.zero; // 물리속도가 영향을 주지 않도록 
            return;
        }

        // 게임이 정지상태면 Update X
        if (GameManager.Instance.IsGameStop)
        {
            return;
        }

        // 죽었거나, Hit 상태면 이동 X
        if (!isLive || animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            return;
        }

        Move();
    }

    private void LateUpdate()
    {
        if (!isLive)
        {
            return;
        }

        Flip();
    }

    // Object 활성화 시 초기값 세팅
    public void Init(SpawnData spawnData)
    {
        isLive = true;
        SetComponent();
        objectType = spawnData.ObjType;
        animator.runtimeAnimatorController = spawnData.AniController;
        maxHp = spawnData.Hp;
        currentHp = maxHp;
        moveSpeed = spawnData.MoveSpeed;
    }

    private void Move()
    {
        dirVec = player.transform.position - transform.position; // 플레이어 위치에서 적 위치 빼면 벡터방향나옴
        dirVec = dirVec.normalized; // 해당 벡터 방향 정규화 처리 해주고
        rigidbody2d.linearVelocity = dirVec * moveSpeed; // 해당방향으로 물리이동해주기
    }

    private void Flip()
    {   // 플레이어 좌표 에 따라 좌우 바꿔주는 로직
        spriteRenderer.flipX = (dirVec.x < 0 ? true : (dirVec.x > 0 ? false : spriteRenderer.flipX));
    }

    public void OnHit(float damage)
    {
        // 이미 죽었으면 충돌 처리 X
        if (!isLive)
        {
            return;
        }

        currentHp -= damage;

        if (currentHp <= 0)
        {
            isLive = false;
            SetComponent();
            GameManager.Instance.OnKillEnemy();

            if (!GameManager.Instance.IsGameOver)
            {
                AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.Dead);
            }
        }
        else
        {
            animator.SetTrigger("Hit");
            StartCoroutine(KnockBack());

            // 게임을 이겼을 때 모든 적이 처치되는 상황에서는 재생 X
            if (!GameManager.Instance.IsGameOver)
            {
                AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.Hit);
            }
        }
    }

    IEnumerator KnockBack()
    {
        yield return null;
        rigidbody2d.AddForce(-dirVec * 3f, ForceMode2D.Impulse);
    }

    public void OnDeath()
    {
        gameObject.SetActive(false);
    }

    // 생존 유무에 따라 Component 값 설정
    private void SetComponent()
    {
        if (isLive)
        {
            collider2d.enabled = true;
            rigidbody2d.simulated = true;
            spriteRenderer.sortingOrder = 0;
            animator.SetBool("Dead", false);
        }
        else
        {
            collider2d.enabled = false;
            rigidbody2d.simulated = false;
            spriteRenderer.sortingOrder = -1;
            animator.SetBool("Dead", true);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameManager.Instance.IsGameStop)
        {
            return;
        }

        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>();
            player.OnHit(10 * Time.deltaTime); // 1초에 10의 Damage
        }
    }
}
