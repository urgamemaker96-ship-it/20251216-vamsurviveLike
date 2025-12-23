using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    private RectTransform rectTransform;
    private Item[] items;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        items = GetComponentsInChildren<Item>(true);
    }

    public void ShowLevelUpPanel()
    {
        GameManager.Instance.GameStop(); // timeScale 조절
        RandomItemSelect();

        // Level별 Item 설명 Update
        foreach (Item item in items)
        {
            // LevelUp 시 랜덤하게 뽑힌 능력치에 대해서만 Update
            if (item.gameObject.activeSelf)
            {
                item.UpdateItemDescript();
            }
        }

        rectTransform.localScale = Vector3.one; // 화면 Open
        AudioManager.Instance.BgmEffect(true);
        AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.LevelUp);
    }

    public void HideLevelUpPanel()
    {
        rectTransform.localScale = Vector3.zero;
        GameManager.Instance.GameResume();
        AudioManager.Instance.BgmEffect(false);
        AudioManager.Instance.PlaySfxAudio(AudioManager.Sfx.Select);
    }

    // Player 선택 시 해당 직업에 맞는 초기 스킬 세팅
    public void ItemInitSelect(int index)
    {
        items[index].OnButtonClicked();
    }

    // Item 중 랜덤하게 3개 선택
    // 만렙인 Item은 제외
    public void RandomItemSelect()
    {
        Item healItem = items[items.Length - 1];

        // 1. 모든 아이템 비활성화
        foreach (Item item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. Item 중 랜덤하게 3개 선택
        List<Item> itemList = new List<Item>();

        // 중복되지 않은 3개의 Index를 모두 뽑을 때까지 
        while (itemList.Count < 3)
        {
            int randomIndex = Random.Range(0, items.Length);
            itemList.Add(items[randomIndex]);
            itemList = itemList.Distinct().ToList();
        }

        // 3. 만렙 Item은 일회용 소비 Item으로 대체

        for (int index = 0; index < itemList.Count; index++)
        {
            Item randomItem = itemList[index];

            // 최대 Level에 도달한 Item이 선택됐다면
            if (randomItem.Level == randomItem.Data.Damages.Length)
            {
                randomItem = healItem;
            }

            randomItem.gameObject.SetActive(true);
        }
    }
}
