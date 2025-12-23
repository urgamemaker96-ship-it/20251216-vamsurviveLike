using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private float scanRange;
    [SerializeField] private LayerMask targetLayer;
    private RaycastHit2D[] targets;
    public Transform NearestTarget { get; private set; } // Player와 가장 가까운 Target

    private void FixedUpdate()
    {
        if (GameManager.Instance.IsGameOver || GameManager.Instance.IsGameStop)
        {
            return;
        }
        
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0f, targetLayer);
        NearestTarget = GetNearest();
    }

    private Transform GetNearest()
    {
        Transform result = null;
        float minDist = scanRange;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            float diff = Vector3.Distance(myPos, targetPos); // 두 Position 사이의 거리

            if (diff < minDist) // 더 가까운 Enemy가 존재한다면
            {
                minDist = diff;
                result = target.transform;
            }
        }

        return result;
    }
}
