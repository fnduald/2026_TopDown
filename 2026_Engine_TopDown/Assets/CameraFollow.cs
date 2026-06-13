using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    // 유니티 인스펙터 창에서 직접 캐릭터를 꽂아줄 변수
    public Transform targetPlayer;

    void Start()
    {
        // 카메라 배율 고정
        Camera mainCamera = GetComponent<Camera>();
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = 6f;
        }
    }

    void LateUpdate()
    {
        // 타겟(플레이어)이 연결되어 있다면 그림자처럼 실시간으로 똑같이 쫓아갑니다.
        if (targetPlayer != null)
        {
            transform.position = new Vector3(targetPlayer.position.x, targetPlayer.position.y, -10f);
        }
    }
}