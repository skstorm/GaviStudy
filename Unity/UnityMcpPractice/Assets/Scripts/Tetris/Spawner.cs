using UnityEngine;

public class Spawner : MonoBehaviour
{
    [Header("Tetromino Prefabs (비워도 됨 - 자동 로드)")]
    public GameObject[] tetrominoPrefabs;

    [Header("Next Piece Preview")]
    public Transform nextPreviewParent;

    // 스폰 위치: 보드 중앙 상단
    private Vector3 spawnPosition = new Vector3(-0.5f, 8.5f, 0f);

    private int nextIndex;
    private GameObject nextPreviewInstance;

    private static readonly string[] PieceNames = { "I_Piece", "O_Piece", "T_Piece", "S_Piece", "Z_Piece", "J_Piece", "L_Piece" };

    void Awake()
    {
        // Inspector에서 할당되지 않은 경우 Resources에서 자동 로드
        if (tetrominoPrefabs == null || tetrominoPrefabs.Length == 0)
        {
            tetrominoPrefabs = new GameObject[PieceNames.Length];
            for (int i = 0; i < PieceNames.Length; i++)
            {
                tetrominoPrefabs[i] = Resources.Load<GameObject>("Tetris/" + PieceNames[i]);
                if (tetrominoPrefabs[i] == null)
                    Debug.LogError("[Spawner] 프리팹 로드 실패: Resources/Tetris/" + PieceNames[i]);
            }
        }
    }

    void Start()
    {
        if (tetrominoPrefabs == null || tetrominoPrefabs.Length == 0)
        {
            Debug.LogError("[Spawner] 로드된 프리팹이 없습니다. Assets/Resources/Tetris/ 폴더를 확인하세요.");
            return;
        }

        nextIndex = Random.Range(0, tetrominoPrefabs.Length);
        SpawnNext();
    }

    public void SpawnNext()
    {
        if (tetrominoPrefabs == null || tetrominoPrefabs.Length == 0) return;

        Instantiate(tetrominoPrefabs[nextIndex], spawnPosition, Quaternion.identity);

        nextIndex = Random.Range(0, tetrominoPrefabs.Length);
        UpdatePreview();
    }

    private void UpdatePreview()
    {
        if (nextPreviewInstance != null)
            Destroy(nextPreviewInstance);

        // 시각 전용 미리보기: Tetromino 컴포넌트를 즉시 비활성화해서 Start() 실행 방지
        // (Destroy는 지연 실행이라 Start()가 먼저 실행됨 → enabled=false 사용)
        nextPreviewInstance = Instantiate(tetrominoPrefabs[nextIndex]);

        var tetromino = nextPreviewInstance.GetComponent<Tetromino>();
        if (tetromino != null) tetromino.enabled = false;

        // 보드 우측에 고정 배치 (UI 대신 월드 좌표 사용)
        nextPreviewInstance.transform.position = new Vector3(6.5f, 6f, 0f);
        nextPreviewInstance.transform.localScale = Vector3.one * 0.7f;
    }
}
