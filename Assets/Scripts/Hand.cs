using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    [SerializeField] private bool isLeftHand;
    private SpriteRenderer spriteRenderer;
    private SpriteRenderer playerRenderer;

    private Vector3 leftHandPos = new Vector3(0.3f, -0.138f, 0f);
    private Vector3 leftHandPosReverse = new Vector3(-0.025f, -0.138f, 0f);
    private Quaternion rightHandRot = Quaternion.Euler(0f, 0f, -30f);
    private Quaternion rightHandRotReverse = Quaternion.Euler(0f, 0f, -130f);

    private void Awake()
    {
        playerRenderer = GetComponentsInParent<SpriteRenderer>()[1];
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void LateUpdate()
    {
        bool isReverse = playerRenderer.flipX;

        if (isLeftHand)
        {
            transform.localPosition = isReverse ? leftHandPosReverse : leftHandPos;
            spriteRenderer.flipX = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 2 : 0;
        }
        else
        {
            transform.localRotation = isReverse ? rightHandRotReverse : rightHandRot;
            spriteRenderer.flipY = isReverse;
            spriteRenderer.sortingOrder = isReverse ? 0 : 2;
        }
    }

    public void SetHandSprite(Sprite handSprite)
    {
        GetComponent<SpriteRenderer>().sprite = handSprite;
    }
}
