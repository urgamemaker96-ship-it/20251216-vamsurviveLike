using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private Collider2D myCollider;

    private float groundLen = 30f; // Ground 한 변의 길이
    private float repositionLen = 20f; // Enemy가 재배치될 거리
    private float epslion = 0.1f; // 오차 보정

    private void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Player의 Box Collider인 Area
        if (collider.CompareTag("Area"))
        {
            Player player = GameManager.Instance.Player;

            Vector3 playerPos = player.transform.position;
            Vector3 myPos = transform.position;

            // Tile Map과 Player가 X, Y 축으로 얼마나 떨어져있는지
            float diffX = Mathf.Abs(playerPos.x - myPos.x);
            float diffY = Mathf.Abs(playerPos.y - myPos.y);

            // Player가 움직이는 방향
            Vector3 playerVelo = player.GetComponent<Rigidbody2D>().linearVelocity;
            float dirX = (playerVelo.x != 0 ? Mathf.Sign(playerVelo.x) : 0f);
            float dirY = (playerVelo.y != 0 ? Mathf.Sign(playerVelo.y) : 0f);

            switch (transform.tag)
            {
                case "Ground":
                    // X, Y축으로 Tile Map의 길이만큼 멀어졌으면 Player 방향으로 X, Y축 이동
                    // 미세한 움직임으로 오류 발생 방지를 위해 오차 보정
                    if ((diffX > groundLen) || (Mathf.Abs(diffX - groundLen) <= epslion))
                    {
                        transform.position += Vector3.right * dirX * groundLen * 2;
                    }
                    if ((diffY > groundLen) || (Mathf.Abs(diffY - groundLen) <= epslion))
                    {
                        transform.position += Vector3.up * dirY * groundLen * 2;
                    }
                    break;
                case "Enemy":
                    // 살아있을 때만 이동
                    if (myCollider.enabled)
                    {
                        // Player가 움직이는 방향의 반대편으로 화면 밖에 재배치
                        Vector3 randomVec = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f); // 랜덤좌표 추가
                        transform.position = playerPos + playerVelo.normalized * repositionLen; 
                    }
                    break;
            }
        }
    }
}
