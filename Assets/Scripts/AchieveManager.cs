using System;
using System.Collections;
using UnityEngine;

public enum Achieve
{
    CharacterPotato,
    CharacterBean,
}

public class AchieveManager : MonoBehaviour
{
    [SerializeField] private GameObject[] lockCharacter;
    [SerializeField] private GameObject[] unlockCharacter;
    private Achieve[] achieves;
    private bool isUnlock;

    [SerializeField] private GameObject noticeUi;
    private WaitForSecondsRealtime noticeTime = new WaitForSecondsRealtime(3f);


    private void Awake()
    {
        achieves = (Achieve[])Enum.GetValues(typeof(Achieve));

        if (!PlayerPrefs.HasKey("InitData"))
        {
            Init();
        }
    }

    private void Init()
    {
        PlayerPrefs.SetInt("InitData", 1);

        foreach (Achieve achieve in achieves)
        {
            PlayerPrefs.SetInt(achieve.ToString(), 0);
        }
    }

    private void Start()
    {
        SetUnlockObject();
    }

    private void LateUpdate()
    {
        foreach (Achieve achieve in achieves)
        {
            CheckAchieve(achieve);
        }
    }

    // 게임 시작 시 
    private void SetUnlockObject()
    {
        for (int index = 0; index < achieves.Length; index++)
        {
            string achieveName = achieves[index].ToString();
            bool isUnlock = (PlayerPrefs.GetInt(achieveName) == 1); // 0이면 Lock, 1이면 Unlock
            lockCharacter[(int)achieves[index]].SetActive(!isUnlock);
            unlockCharacter[(int)achieves[index]].SetActive(isUnlock);
        }
    }

    // 특정 조건 달성 시 인자로 들어온 achieve 해금
    public void UnlockObject(Achieve achieve)
    {
        PlayerPrefs.SetInt(achieve.ToString(), 1);
    }

    private void CheckAchieve(Achieve achieve)
    {
        isUnlock = false;

        switch (achieve)
        {
            case Achieve.CharacterPotato:
                // 킬 수가 500 이상이라면
                if (GameManager.Instance.Kill >= 500)
                {
                    isUnlock = true;
                }
                break;
            case Achieve.CharacterBean:
                // 게임의 결과가 승리라면
                if (GameManager.Instance.IsResult)
                {
                    isUnlock = true;
                }
                break;
        }

        // 조건이 충족됐고, 아직 해금 처리를 안 했다면
        if (isUnlock && PlayerPrefs.GetInt(achieve.ToString()) == 0)
        {
            PlayerPrefs.SetInt(achieve.ToString(), (isUnlock ? 1 : 0));

            // 해금 알림
            for (int index = 0; index < noticeUi.transform.childCount; index++)
            {
                if (index == (int)achieve)
                {
                    noticeUi.transform.GetChild(index).gameObject.SetActive(true);
                }
            }

            StartCoroutine(NoticeUnlockRoutine());
        }
    }

    IEnumerator NoticeUnlockRoutine()
    {
        noticeUi.SetActive(true);

        AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.LevelUp);
        yield return noticeTime;

        noticeUi.SetActive(false);
    }
}
