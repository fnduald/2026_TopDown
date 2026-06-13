using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private int currentX;
    private int currentY;
    private SimpleLoguelikeManager mapManager;

    public void SpawnAtCell(int startX, int startY, SimpleLoguelikeManager manager)
    {
        mapManager = manager;
        currentX = startX;
        currentY = startY;
        UpdateActualPosition();
    }

    void Update()
    {
        if (mapManager == null) return;

        int dx = 0;
        int dy = 0;

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) dy = 1;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) dy = -1;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) dx = -1;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) dx = 1;

        if (dx != 0 || dy != 0)
        {
            int targetX = currentX + dx;
            int targetY = currentY + dy;

            if (mapManager.CanMoveTo(targetX, targetY))
            {
                currentX = targetX;
                currentY = targetY;
                UpdateActualPosition();
                Debug.Log($"[플레이어 이동] 격자 좌표: ({currentX}, {currentY})");

                // [중요] 내가 한 칸 이동했으므로 매니저에게 모든 몬스터를 움직이라고 명령합니다.
                mapManager.MoveAllMonsters();
            }
            else
            {
                Debug.LogWarning("[플레이어 이동 불가] 벽이거나 맵 바깥입니다!");
            }
        }
    }

    void UpdateActualPosition()
    {
        if (mapManager != null)
        {
            transform.position = mapManager.GetWorldPositionOfCell(currentX, currentY);
        }
    }
}