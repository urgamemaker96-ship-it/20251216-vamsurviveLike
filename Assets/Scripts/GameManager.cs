using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Game 전반적인 Control을 위한 변수
    [Header("# Game Control")]
    [SerializeField] private PoolManager poolManager; // Object Pool 관리 Object
    public PoolManager PoolManager => poolManager;
    [SerializeField] private GameInfoHud gameInfoHud; // 게임의 전반적인 정보 UI 관리 Object
    public GameInfoHud GameInfoHud => gameInfoHud;
    [SerializeField] private GameResult gameResult; // 결과 Panel
    [SerializeField] private GameObject characterSelect; // 게임 시작 시 캐릭터를 선택하는 Panel
    [SerializeField] private GameObject enemyCleaner; // 적을 모두 처치하는 Object
    [SerializeField] private LevelUp levelUpPanel; // LevelUp 시 능력 선택 Panel

    // Player 관련 정보
    [SerializeField] private Player player;
    public Player Player => player;
    private int playerId;
    public int PlayerId => playerId;

    // Level 정보
    [Header("# UI Info")]
    [SerializeField] private int level;
    [SerializeField] private int kill;
    public int Kill => kill;
    [SerializeField] private int exp;
    private int[] levelUpExp = { 5, 10, 20, 30 };

    // 상태 변수
    private bool isGameStop = false;
    public bool IsGameStop => isGameStop;
    private bool isGameOver = false;
    public bool IsGameOver => isGameOver;
    private bool isResult; // Game 결과, true면 승리, false면 패배
    public bool IsResult => isResult;
    public float GameTime { get; private set; } // 현재 게임의 진행 시간
    private float maxGameTime = 3f * 60f; // 게임의 최대 진행 시간, N * 60초, N분
    public float MaxGameTime => maxGameTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameStop();
    }

    private void Update()
    {
        if (isGameOver || isGameStop)
        {
            return;
        }

        GameTime += Time.deltaTime;

        if (GameTime > maxGameTime)
        {
            GameOver(true);
        }
        else
        {
            gameInfoHud.UpdateRemainTimeText(maxGameTime - GameTime);
        }
    }

    public void CharacterSelect(int id)
    {
        AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.Select);

        // Player 정보 초기화
        playerId = id;
        player.gameObject.SetActive(true);
        levelUpPanel.ItemInitSelect(playerId % 2);

        // GameInfoHud 활성화
        gameInfoHud.gameObject.SetActive(true);

        // Character Select Panel Off
        characterSelect.SetActive(false);

        // Game Start
        GameResume();
        AudioManager.Instance.PlayBgmAudio(true); // 배경음악 재생
    }

    public void OnKillEnemy()
    {
        if (isGameOver)
        {
            return;
        }

        kill++;
        GetExp();
    }

    private void GetExp()
    {
        exp++;

        // Level별 요구 경험치를 충족했으면 Level Up
        if (exp == levelUpExp[Mathf.Min(level, levelUpExp.Length - 1)])
        {
            level++;
            exp = 0;
            levelUpPanel.ShowLevelUpPanel();
            gameInfoHud.UpdateLevelText(level);
        }

        gameInfoHud.UpdateExpSlider(exp, levelUpExp[Mathf.Min(level, levelUpExp.Length - 1)]);
        gameInfoHud.UpdateKillText(kill);
    }

    public void GameStop()
    {
        isGameStop = true;
        Time.timeScale = 0f;
    }

    public void GameResume()
    {
        isGameStop = false;
        Time.timeScale = 1f;
    }

    // result가 true면 시간이 지나서 승리, false면 죽어서 종료
    public void GameOver(bool result)
    {
        isGameOver = true;
        isResult = result;

        player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;

        // Enemy Spawn 정지
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        if (enemySpawner != null)
        {
            enemySpawner.StopEnemySpawn();
        }

        // 게임을 이겼으면 Field의 모든 Enemy 처치
        if (result)
        {
            enemyCleaner.SetActive(true);
        }

        // 1초 뒤에 결과창 Open
        StartCoroutine(ShowGameResultRoutine());
    }

    IEnumerator ShowGameResultRoutine()
    {
        yield return new WaitForSeconds(1.0f);

        AudioManager.Instance.PlayBgmAudio(false); // 배경음악 중지
        gameResult.gameObject.SetActive(true);
        gameResult.SetResult(isResult);
        AudioManager.Instance.PlaySfxAudio(isResult ? AudioManager.Sfx.Win : AudioManager.Sfx.Lose);

        GameStop();
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Scenes/GameScene");
    }

    public void Menu()
    {
        SceneManager.LoadScene("Scenes/MenuScene");
    }
}
