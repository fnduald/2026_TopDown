using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTransform;
    private Camera mainCamera;

    void Start()
    {
        // 씬에서 player라는 오브젝트를 정밀하게 찾습니다.
        GameObject playerObj = GameObject.Find("player");
        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
        }

        // 카메라 배율도 코드로 강제 고정합니다.
        mainCamera = GetComponent<Camera>();
        if (mainCamera != null)
        {
            mainCamera.orthographicSize = 6f; // 화면 크기 적당하게 고정
        }
    }

    void LateUpdate()
    {
        // 게임이 돌아가는 동안 플레이어의 위치를 실시간으로 똑같이 쫓아갑니다.
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, -10f);
        }
    }
}