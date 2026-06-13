using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private int currentX;
    private int currentY;
    private SimpleLoguelikeManager mapManager;

    // 매니저가 생성될 때 몬스터의 초기 위치와 세팅을 잡아주는 함수
    public void SetupMonster(int startX, int startY, SimpleLoguelikeManager manager)
    {
        mapManager = manager;
        currentX = startX;
        currentY = startY;
        UpdateActualPosition();
    }

    // 매니저의 신호를 받아 상하좌우 중 한 곳으로 무작위 이동하는 함수 (벽 자동 회피)
    public void MoveRandomly()
    {
        if (mapManager == null) return;

        // 상(0), 하(1), 좌(2), 우(3) 방향 배열
        int[] dx = { 0, 0, -1, 1 };
        int[] dy = { 1, -1, 0, 0 };

        // 0~3 무작위 숫자를 뽑아 방향 결정
        int randomIndex = Random.Range(0, 4);

        int targetX = currentX + dx[randomIndex];
        int targetY = currentY + dy[randomIndex];

        // 매니저에게 거기가 갈 수 있는 바닥(빨간 벽 내부)인지 검사 요청
        if (mapManager.CanMoveTo(targetX, targetY))
        {
            currentX = targetX;
            currentY = targetY;
            UpdateActualPosition();
        }
        // 만약 선택한 방향이 벽이라면 몬스터는 이번 턴에 무리하게 움직이지 않고 제자리에 안전하게 대기합니다.
    }

    void UpdateActualPosition()
    {
        if (mapManager != null)
        {
            // 타일 격자 한가운데 좌표로 귀신같이 맵 매칭 순간이동
            transform.position = mapManager.GetWorldPositionOfCell(currentX, currentY);
        }
    }
}