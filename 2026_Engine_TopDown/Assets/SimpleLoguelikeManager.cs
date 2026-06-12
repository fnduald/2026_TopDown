using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class SimpleLoguelikeManager : MonoBehaviour
{
    [System.Serializable]
    public class MonsterData
    {
        public string monsterName;
        public int hp;
        public int atk;
        public Vector2Int gridPos; // 몬스터가 위치한 가상 격자 좌표 (X, Y)
        public GameObject monsterObject; // 실제 화면에 보일 게임 오브젝트
    }

    [Header("유니티 타일맵 세팅")]
    public Tilemap tilemap;
    public TileBase floorTile; // 바닥 타일 에셋 (인스펙터에서 연결)
    public TileBase wallTile;  // 벽 타일 에셋 (인스펙터에서 연결)

    [Header("몬스터 프리팹 세팅")]
    public GameObject monsterPrefab; // 빨간 네모 프리팹 (인스펙터에서 연결)

    // [전체 데이터 구성 핵심] 
    private int gridSize = 11;
    private int[,] mapGridData; // 0: 빈 공간, 1: 벽, 2: 몬스터 자리
    private List<MonsterData> currentMonstersData = new List<MonsterData>(); // 현재 맵의 모든 몬스터 데이터 리스트

    void Start()
    {
        CreateAndGenerateLoguelikeMap();
    }

    public void CreateAndGenerateLoguelikeMap()
    {
        // 1. 기존 데이터 및 오브젝트 완전 초기화 (청소)
        tilemap.ClearAllTiles();
        foreach (var m in currentMonstersData)
        {
            if (m.monsterObject != null) Destroy(m.monsterObject);
        }
        currentMonstersData.Clear();

        // 11x11 빈 데이터 도화지 생성
        mapGridData = new int[gridSize, gridSize];

        // 2. 바닥 타일 깔면서 기본 데이터(0) 채우기
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                mapGridData[x, y] = 0; // 0은 빈 바닥 데이터
                tilemap.SetTile(new Vector3Int(x, y, 0), floorTile);
            }
        }

        // 3. 랜덤 주사위 굴려 벽 데이터(1) 15개 배치하기
        int wallsPlaced = 0;
        while (wallsPlaced < 15)
        {
            int rx = Random.Range(0, gridSize);
            int ry = Random.Range(0, gridSize);

            // 플레이어 시작 자리[5,5]가 아니고, 아직 빈 곳(0)인 경우에만
            if ((rx == 5 && ry == 5) == false && mapGridData[rx, ry] == 0)
            {
                mapGridData[rx, ry] = 1; // 1은 벽 데이터
                tilemap.SetTile(new Vector3Int(rx, ry, 0), wallTile); // 화면에 벽 그리기
                wallsPlaced++;
            }
        }

        // 4. 필수 데이터 구성: 랜덤 위치에 몬스터 데이터 생성 및 오브젝트 배치
        int monstersPlaced = 0;
        while (monstersPlaced < 3)
        {
            int rx = Random.Range(0, gridSize);
            int ry = Random.Range(0, gridSize);

            // 플레이어 자리 아니고, 벽이 아니고, 텅 빈 바닥일 때만 데이터 주입
            if ((rx == 5 && ry == 5) == false && mapGridData[rx, ry] == 0)
            {
                mapGridData[rx, ry] = 2; // 2는 몬스터 데이터 자리

                // 실제 화면에 프리팹 소환 (격자 정중앙 맞춤 축 보정 +0.5f)
                Vector3 spawnPosition = new Vector3(rx + 0.5f, ry + 0.5f, 0);
                GameObject spawnedObj = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);

                // [필수 데이터 구성] 구조체에 체력, 공격력, 위치 실시간 기입
                MonsterData newMonster = new MonsterData();
                newMonster.monsterName = "슬라임_" + monstersPlaced;
                newMonster.hp = 30;
                newMonster.atk = 5;
                newMonster.gridPos = new Vector2Int(rx, ry);
                newMonster.monsterObject = spawnedObj;

                // 전체 데이터 리스트에 추가
                currentMonstersData.Add(newMonster);
                monstersPlaced++;
            }
        }

        Debug.Log($"[시스템] 맵 생성 완료! 배치된 벽: {wallsPlaced}개, 구성된 몬스터 데이터: {currentMonstersData.Count}마리");
    }

    // 외부(PlayerController 등)에서 이동할 때 벽이나 몹 데이터가 있는지 체크해주는 창구
    public bool CanMoveTo(int targetX, int targetY)
    {
        // 격자 판 밖으로 나가는 것 방지 데이터 검사
        if (targetX < 0 || targetX >= gridSize || targetY < 0 || targetY >= gridSize) return false;

        // 데이터가 1(벽)이면 못 움직임
        if (mapGridData[targetX, targetY] == 1) return false;

        return true;
    }
}