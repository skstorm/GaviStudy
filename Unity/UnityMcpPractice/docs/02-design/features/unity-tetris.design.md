# Design: Unity Tetris Game

> 상태: Design 작성 완료
> 생성일: 2026-02-21
> 피처명: unity-tetris
> 참조: docs/01-plan/features/unity-tetris.plan.md

---

## 1. 씬 구조 (Scene Structure)

### 씬 파일
- **경로**: `Assets/Scenes/TetrisScene.unity`
- SampleScene은 건드리지 않음
- MCP `manage_scene` 으로 새 씬 생성 후 활성화

### Hierarchy 구조

```
TetrisScene
├── Main Camera                  (Orthographic, Size=12, Position: 0,0,-10)
├── GameManager                  (GameManager.cs)
├── Board                        (Board.cs)
│   └── Grid                     (10x20 셀 렌더링 부모)
├── Spawner                      (Spawner.cs)
├── GhostPiece                   (고스트 블록, 선택 사항)
└── Canvas (Screen Space Overlay)
    ├── GamePanel
    │   ├── ScoreLabel (Text)
    │   ├── ScoreValue (Text)
    │   ├── LevelLabel (Text)
    │   ├── LevelValue (Text)
    │   ├── LinesLabel (Text)
    │   └── LinesValue (Text)
    ├── NextPiecePanel
    │   ├── NextLabel (Text)
    │   └── NextPreview (Image/GameObject)
    └── GameOverPanel (기본 비활성)
        ├── GameOverText (Text)
        ├── FinalScoreText (Text)
        └── RestartButton (Button)
```

---

## 2. 폴더 구조 (Folder Structure)

```
Assets/
├── Scenes/
│   ├── SampleScene.unity        ← 기존 씬 (건드리지 않음)
│   └── TetrisScene.unity        ← 신규 생성
├── Scripts/
│   └── Tetris/
│       ├── GameManager.cs
│       ├── Board.cs
│       ├── Tetromino.cs
│       ├── Spawner.cs
│       └── TetrominoData.cs     (ScriptableObject - 선택)
├── Prefabs/
│   └── Tetris/
│       ├── Block.prefab          (단일 셀 블록)
│       └── TetrominoPrefabs/
│           ├── I_Piece.prefab
│           ├── O_Piece.prefab
│           ├── T_Piece.prefab
│           ├── S_Piece.prefab
│           ├── Z_Piece.prefab
│           ├── J_Piece.prefab
│           └── L_Piece.prefab
└── Materials/
    └── Tetris/
        ├── Block_I.mat           (청록)
        ├── Block_O.mat           (노랑)
        ├── Block_T.mat           (보라)
        ├── Block_S.mat           (초록)
        ├── Block_Z.mat           (빨강)
        ├── Block_J.mat           (파랑)
        └── Block_L.mat           (주황)
```

---

## 3. 컴포넌트 상세 설계 (Component Design)

### 3-1. GameManager.cs

```csharp
public class GameManager : MonoBehaviour
{
    // 싱글턴
    public static GameManager Instance { get; private set; }

    // 상태
    public enum GameState { Playing, GameOver }
    public GameState State { get; private set; }

    // 점수
    public int Score { get; private set; }
    public int Level { get; private set; }
    public int LinesCleared { get; private set; }

    // 레벨당 라인 수
    private const int LinesPerLevel = 10;

    // 점수표
    private static readonly int[] ScoreTable = { 0, 100, 300, 500, 800 };

    // 메서드
    void Awake()           // 싱글턴 설정
    void Start()           // 게임 시작
    public void AddScore(int lines)    // 점수 추가 및 레벨업 체크
    public void GameOver()             // 게임 오버 처리
    public void RestartGame()          // 씬 재로드 (SceneManager.LoadScene)
    private void UpdateUI()            // ScoreValue, LevelValue, LinesValue 갱신
}
```

**낙하 속도 공식**:
```
fallInterval = Mathf.Max(0.05f, 1.0f - (Level - 1) * 0.1f)
```

---

### 3-2. Board.cs

```csharp
public class Board : MonoBehaviour
{
    public static readonly int Width = 10;
    public static readonly int Height = 20;
    public static readonly int SpawnRow = 19;  // 최상단

    // 그리드 데이터: null이면 빈 칸, Transform이면 고정된 블록
    private Transform[,] grid = new Transform[Width, Height];

    // 메서드
    public bool IsValidPosition(Tetromino piece)
        // piece의 4개 블록 좌표 검사
        // x: 0~9, y: 0~19 범위 + 이미 점유된 셀 검사

    public void PlacePiece(Tetromino piece)
        // 블록을 grid 배열에 등록
        // piece의 자식 Transform들을 grid[x,y]에 저장

    public int ClearLines()
        // 꽉 찬 줄 탐지 (y=0부터 위로 순회)
        // 꽉 찬 줄 삭제 후 위 블록을 아래로 이동
        // 제거된 줄 수 반환

    private bool IsLineFull(int y)
        // grid[x,y] x=0..9 모두 null이 아닌지 확인

    private void DeleteLine(int y)
        // grid[x,y] 모든 오브젝트 Destroy

    private void MoveRowDown(int y, int delta)
        // y행의 모든 블록을 y-delta 위치로 이동
}
```

**좌표계**:
- 원점(0,0) = 보드 좌하단
- Board 오브젝트 Position: (-4.5f, -9.5f, 0) → 셀을 정수 좌표로 배치 가능

---

### 3-3. Tetromino.cs

```csharp
public class Tetromino : MonoBehaviour
{
    // 7가지 테트로미노 오프셋 (로컬 좌표, pivot 기준)
    public static readonly Vector2Int[][] Shapes = new Vector2Int[][]
    {
        // I: (0,0)(1,0)(2,0)(3,0)
        new[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(3,0) },
        // O: (0,0)(1,0)(0,1)(1,1)
        new[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(0,1), new Vector2Int(1,1) },
        // T: (0,0)(1,0)(2,0)(1,1)
        new[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(1,1) },
        // S: (1,0)(2,0)(0,1)(1,1)
        new[] { new Vector2Int(1,0), new Vector2Int(2,0), new Vector2Int(0,1), new Vector2Int(1,1) },
        // Z: (0,0)(1,0)(1,1)(2,1)
        new[] { new Vector2Int(0,0), new Vector2Int(1,0), new Vector2Int(1,1), new Vector2Int(2,1) },
        // J: (0,0)(0,1)(1,1)(2,1)
        new[] { new Vector2Int(0,0), new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1) },
        // L: (2,0)(0,1)(1,1)(2,1)
        new[] { new Vector2Int(2,0), new Vector2Int(0,1), new Vector2Int(1,1), new Vector2Int(2,1) },
    };

    private Board board;
    private float fallTimer;
    private float fallInterval;    // GameManager에서 받아옴

    // 4개 블록 자식 Transform 참조
    private Transform[] blocks = new Transform[4];

    void Update()
        // 1. 입력 처리 (Keyboard.current)
        //    - LeftArrow → MoveLeft()
        //    - RightArrow → MoveRight()
        //    - DownArrow → MoveDown() (소프트드롭)
        //    - UpArrow/Z → Rotate()
        //    - Space → HardDrop()
        // 2. 자동 낙하 타이머

    public void MoveLeft()   // position.x -= 1, 유효하지 않으면 복원
    public void MoveRight()  // position.x += 1, 유효하지 않으면 복원
    public bool MoveDown()   // position.y -= 1, 유효하지 않으면 복원 후 고정
    public void Rotate()     // 90도 회전 (자식 블록 좌표 변환), 유효하지 않으면 복원
    public void HardDrop()   // MoveDown() 반복 호출 후 고정

    private void Lock()
        // board.PlacePiece(this)
        // board.ClearLines() → GameManager.AddScore()
        // Spawner.SpawnNext()
        // this.enabled = false

    private bool IsAtTopBoundary()
        // 고정 시 최상단(y >= Height-1)이면 GameManager.GameOver()
}
```

**회전 알고리즘** (단순 회전):
```
새 좌표 = (pivot.x - (y - pivot.y), pivot.y + (x - pivot.x))
```
Wall kick: 회전 후 유효하지 않으면 좌(-1), 우(+1), 위(+1) 오프셋 순서로 시도.

---

### 3-4. Spawner.cs

```csharp
public class Spawner : MonoBehaviour
{
    public GameObject[] tetrominoPrefabs;   // Inspector에서 7개 프리팹 연결
    public Transform nextPreviewParent;     // NextPiecePanel 하위 Transform

    private int nextIndex;
    private GameObject nextPreviewInstance;

    void Start()
        // nextIndex = Random.Range(0, 7)
        // UpdatePreview()
        // SpawnNext()

    public void SpawnNext()
        // 현재 next를 spawn (position: Board 중앙 상단)
        // nextIndex = Random.Range(0, 7)
        // UpdatePreview()

    private void UpdatePreview()
        // nextPreviewInstance 제거 후 새 프리팹 인스턴스 생성
        // nextPreviewParent 하위에 배치, scale 조정

    private Vector3 GetSpawnPosition()
        // return new Vector3(Board.Width / 2f - 1, Board.Height - 1, 0)
        //        + Board 오브젝트 위치 보정
}
```

---

## 4. 데이터 흐름 (Data Flow)

```
[Input]
  Keyboard.current.leftArrowKey.wasPressedThisFrame
        │
        ▼
[Tetromino.Update()]
  MoveLeft() → transform.position.x -= 1
        │
        ▼
[Board.IsValidPosition(this)]
  범위 초과 or 점유 셀 → return false
        │
     false?
     ├── Yes → 위치 복원 (x += 1)
     └── No  → 이동 확정
        │
        ▼ (MoveDown 실패 시)
[Tetromino.Lock()]
        │
        ├─→ [Board.PlacePiece(this)]
        │        grid[x,y] = block.transform
        │
        ├─→ [Board.ClearLines()]
        │        꽉 찬 줄 제거 → int linesCleared 반환
        │
        ├─→ [GameManager.AddScore(linesCleared)]
        │        Score += ScoreTable[linesCleared]
        │        Level 계산, UI 업데이트
        │
        └─→ [Spawner.SpawnNext()]
                 새 테트로미노 생성
                 최상단 점유? → GameManager.GameOver()
```

---

## 5. 입력 매핑 (Input Mapping)

| 키 | 액션 | 처리 방식 |
|----|------|-----------|
| ← | 좌이동 | wasPressedThisFrame + 연속입력 (0.1초 간격) |
| → | 우이동 | wasPressedThisFrame + 연속입력 (0.1초 간격) |
| ↓ | 소프트드롭 | isPressed (매 프레임, 빠른 낙하) |
| ↑ / Z | 회전 | wasPressedThisFrame |
| Space | 하드드롭 | wasPressedThisFrame |

---

## 6. MCP 구현 순서 (MCP Tool Usage)

| 순서 | MCP 툴 | 작업 내용 |
|------|--------|-----------|
| 1 | `manage_scene` | TetrisScene 생성 및 활성화 |
| 2 | `manage_gameobject` | GameManager, Board, Spawner 오브젝트 생성 |
| 3 | `manage_gameobject` | Canvas, UI 오브젝트 계층 생성 |
| 4 | `manage_gameobject` | Camera 설정 (Orthographic) |
| 5 | `create_script` | GameManager.cs 생성 |
| 6 | `create_script` | Board.cs 생성 |
| 7 | `create_script` | Tetromino.cs 생성 |
| 8 | `create_script` | Spawner.cs 생성 |
| 9 | `manage_components` | 각 오브젝트에 스크립트 컴포넌트 추가 |
| 10 | `manage_gameobject` | Block 프리팹 기반 그리드 시각화 |
| 11 | `read_console` | 컴파일 에러 확인 |
| 12 | `manage_scene` | 씬 저장 |

> MCP 연결 끊김 시: curl 직접 호출 방식으로 대응
> (참조: E:\Study\GaviStudy\트러블해결\mcp트러블해결.md)

---

## 7. 완료 기준 (Definition of Done)

- [ ] `Assets/Scenes/TetrisScene.unity` 존재
- [ ] `Assets/Scripts/Tetris/` 하위 4개 스크립트 생성
- [ ] Unity 콘솔 컴파일 에러 없음
- [ ] Play Mode에서 블록이 생성되고 낙하함
- [ ] 키보드 이동/회전 동작
- [ ] 라인 클리어 및 점수 증가 동작
- [ ] 게임 오버 후 재시작 가능
