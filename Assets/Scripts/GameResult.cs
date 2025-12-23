using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResult : MonoBehaviour
{
    [SerializeField] private Image title;

    [SerializeField] private Sprite surviveTitle;
    [SerializeField] private Sprite deadTitle;

    public void SetResult(bool result)
    {
        title.sprite = result ? surviveTitle : deadTitle;
    }
}
