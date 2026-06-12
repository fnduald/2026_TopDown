using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // 현재 플레이어의 가상 격자 좌표 (맵 정중앙인 5, 5에서 시작)
    private int currentX = 5;
    private int currentY = 5;

    // 맵 데이터를 가지고 있는 매니저 연결용 변수
    private SimpleLoguelikeManager mapManager;

    void Start()
    {
        // 씬에 배치된 LoguelikeManager를 자동으로 찾아서 연결합니다.
        mapManager = FindObjectOfType<SimpleLoguelikeManager>();

        // 시작할 때 플레이어의 위치를 격자 5, 5 좌표(중앙)로 강제 텔레포트 (+0.5f는 칸 정중앙 보정)
        transform.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0);
    }

    void Update()
    {
        int dx = 0; // X축 이동 방향 (-1: 왼쪽, 1: 오른쪽)
        int dy = 0; // Y축 이동 방향 (-1: 아래쪽, 1: 위쪽)

        // 키보드 방향키나 WASD를 누르는 순간(GetKeyDown) 딱 한 칸씩만 체크
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dy = 1;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dy = -1;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dx = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dx = 1;

        // 움직임 입력이 들어왔다면
        if (dx != 0 || dy != 0)
        {
            // 내가 가고자 하는 목표 X, Y 좌표 계산
            int targetX = currentX + dx;
            int targetY = currentY + dy;

            // ★ [핵심] 매니저에게 물어봅니다: "거기 갈 수 있는 바닥(0) 맞나요?"
            if (mapManager.CanMoveTo(targetX, targetY))
            {
                // 갈 수 있다면 내 격자 좌표를 갱신하고
                currentX = targetX;
                currentY = targetY;

                // 실제 유니티 화면상의 캐릭터 위치도 그 칸으로 딱 맞춰 이동시킵니다.
                transform.position = new Vector3(currentX + 0.5f, currentY + 0.5f, 0);

                Debug.Log($"[플레이어 이동] 현재 좌표: ({currentX}, {currentY})");
            }
            else
            {
                Debug.LogWarning("[이동 실패] 거기는 벽이거나 맵 밖입니다!");
            }
        }
    }
}