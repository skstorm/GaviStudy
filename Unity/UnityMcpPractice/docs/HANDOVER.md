# 인수인계 문서 - Unity Tetris Game

> 작성일: 2026-02-22 (3차 업데이트)
> 이전 세션 상태: 좌표계 버그 수정 완료, 플레이 테스트 대기 중

---

## 1. 프로젝트 개요

**Unity 6000.0.38f1** 프로젝트에서 **MCP for Unity** (curl 우회 방식) 를 사용해
클래식 테트리스 게임을 개발 완료.

- **프로젝트 경로**: `E:\Study\GaviStudy\Unity\UnityMcpPractice`
- **작업 씬**: `Assets/Scenes/TetrisScene.unity` (SampleScene은 사용하지 않음)
- **PDCA 현재 단계**: Do 완료 → 플레이 테스트 후 Analyze 단계 진입 가능

---

## 2. MCP 연결 방법 (중요!)

### Claude Code에서 MCP 툴이 안 보일 때 (clear 후 등)
직접 curl로 MCP 서버를 호출해야 함.

```bash
# Step 1: 세션 ID 획득
SESSION_ID=$(curl -si -X POST http://localhost:8080/mcp \
  -H "Content-Type: application/json" \
  -H "Accept: application/json, text/event-stream" \
  -d '{
    "jsonrpc": "2.0",
    "method": "initialize",
    "params": {"protocolVersion": "2024-11-05","capabilities": {},"clientInfo": {"name": "claude","version": "1.0"}},
    "id": 1
  }' 2>&1 | grep "mcp-session-id" | awk '{print $2}' | tr -d '\r')

# Step 2: 이후 모든 요청에 세션 ID 포함
curl -s -X POST http://localhost:8080/mcp \
  -H "Content-Type: application/json" \
  -H "Accept: application/json, text/event-stream" \
  -H "mcp-session-id: $SESSION_ID" \
  -d '{"jsonrpc":"2.0","method":"tools/call","params":{"name":"TOOL_NAME","arguments":{...}},"id":2}'
```

### 올바른 MCP 툴 파라미터
| 툴 | 파라미터 | 예시 |
|----|---------|------|
| `manage_scene load` | `name="Scenes/TetrisScene"` | 경로 포함 필요 |
| `execute_menu_item` | `menu_path="Tools/Tetris/..."` | `menu_item` 아님 |
| `read_console` | `count=15` | |
| `find_gameobjects` | `search_term="Canvas"` | `name` 아님 |
| `validate_script` | `uri="Assets/Scripts/..."` | `path` 아님! |

### TetrisScene 열기
```bash
manage_scene action=load, name="Scenes/TetrisScene"
```

### 트러블슈팅 문서
`E:\Study\GaviStudy\트러블해결\mcp트러블해결.md`

---

## 3. 완료된 작업 (전체)

### 스크립트 (`Assets/Scripts/Tetris/`)
| 파일 | 역할 | 상태 |
|------|------|------|
| `Board.cs` | 10x20 그리드, 충돌검사, 라인클리어 **[좌표계 수정 완료]** | ✅ |
| `Tetromino.cs` | 이동/회전/하드드롭, New Input System, Ghost Block | ✅ |
| `GameManager.cs` | 싱글턴, 점수/레벨, 게임오버/재시작 | ✅ |
| `Spawner.cs` | 랜덤 블록 생성, Resources 자동 로드, **Preview 버그 수정** | ✅ |

### Editor 스크립트 (`Assets/Editor/`)
| 파일 | 역할 | 상태 |
|------|------|------|
| `TetrisUISetup.cs` | UI 자동 생성 및 연결 (메뉴: Tools/Tetris/Setup UI) | ✅ |
| `TetrisVisualSetup.cs` | Material·배경색·보드외곽선 설정 (메뉴: Tools/Tetris/Setup Visuals) | ✅ |

### 씬 오브젝트 (`TetrisScene`)
| 오브젝트 | 컴포넌트 | 상태 |
|----------|----------|------|
| Main Camera | Orthographic, Size=12, 배경 어두운 색 | ✅ |
| GameManager | GameManager.cs + UI 레퍼런스 연결됨 | ✅ |
| Board | Board.cs, Position(-4.5, -9.5, 0) | ✅ |
| Spawner | Spawner.cs + nextPreviewParent 연결됨 | ✅ |
| Canvas | HUDPanel, NextPiecePanel, GameOverPanel | ✅ |
| BoardBorder | 보드 외곽선 3개 (좌/우/바닥) | ✅ |

### Canvas UI 구조
```
Canvas
├── HUDPanel (좌상단)
│   ├── ScoreLabel + ScoreText  → GameManager.scoreText 연결
│   ├── LevelLabel + LevelText  → GameManager.levelText 연결
│   └── LinesLabel + LinesText  → GameManager.linesText 연결
├── NextPiecePanel (우상단)
│   ├── NextLabel
│   └── NextPieceDisplay       → Spawner.nextPreviewParent 연결
└── GameOverPanel (중앙, 기본 비활성)
    ├── GameOverText
    ├── FinalScoreText          → GameManager.finalScoreText 연결
    └── RestartButton           → GameManager.RestartGame() 연결
```

### 프리팹 (`Assets/Prefabs/Tetris/` 및 `Assets/Resources/Tetris/`)
I_Piece, O_Piece, T_Piece, S_Piece, Z_Piece, J_Piece, L_Piece — 7종 ✅

### Materials (`Assets/Materials/Tetris/`)
I_Piece_Mat(시안), O_Piece_Mat(노랑), T_Piece_Mat(보라), S_Piece_Mat(초록),
Z_Piece_Mat(빨강), J_Piece_Mat(파랑), L_Piece_Mat(주황), Border_Mat(회색)

---

## 4. 수정된 버그 (누적)

| 버그 | 세션 | 원인 | 해결 |
|------|------|------|------|
| `tetrominoPrefabs가 할당되지 않았습니다.` | 1차 | 인스펙터 미연결 | Spawner.Awake()에서 Resources.Load 자동 로드 |
| TMP 임포터 창 오류 | 2차 | TMP Essential Resources 미임포트 | Window > TextMeshPro > Import TMP Essential Resources |
| **게임이 전혀 움직이지 않음** | 2차 | Preview Tetromino가 Canvas 픽셀 좌표(~960,540)에 생성 → 즉시 GameOver 발동 | `tetromino.enabled = false`로 Start() 실행 차단 |
| **블록 좌표 그리드 매핑 오류** | 3차 | Board.IsValidPosition이 월드 좌표를 그대로 그리드 인덱스로 사용 (Board 오프셋 미적용) | `WorldToGrid()` 메서드로 보드 로컬 좌표 변환 |
| "The referenced script (Unknown)" x5 | 3차 | TMP 임포트 전 상태의 오래된 콘솔 메시지 | Unity 리프레시 후 자동 해소 |

---

## 5. 남은 작업

### 필수 (다음 세션 최우선)
- [ ] **플레이 테스트**: Play 버튼 눌러서 실제 게임 동작 검증
  - 블록이 보드 테두리 안에 생성되는지
  - 방향키 이동/회전/하드드롭 동작
  - 바닥에 쌓이고 다음 블록 생성
  - 줄 삭제 + 점수 증가
  - 게임오버 패널 표시

### 버그 발견 시 예상 후보
- [ ] 블록이 보드 밖에 생성되면 → Spawner의 `spawnPosition` 조정
- [ ] 블록들이 겹쳐 보이면 → 프리팹 블록 로컬 위치 확인
- [ ] 줄 삭제 후 위 블록이 내려오지 않으면 → Board.MoveAllRowsDown 검증

### 선택 (나중에)
- [ ] Gap Analysis 실행 (`/pdca analyze unity-tetris`)
- [ ] 사운드 이펙트 추가
- [ ] 모바일 입력 지원

---

## 6. 주요 설계 결정 사항

### 좌표계 (중요! 3차 세션에서 수정됨)

```
Board 오브젝트 월드 위치: (-4.5, -9.5, 0)
보드 그리드 범위: x=[0~9], y=[0~19]

월드 좌표 → 그리드 인덱스 변환:
  grid.x = worldPos.x - boardPos.x = worldPos.x + 4.5
  grid.y = worldPos.y - boardPos.y = worldPos.y + 9.5

스폰 위치: (-0.5, 8.5, 0) → 그리드 (4, 18) [보드 중앙 상단]
I_Piece 블록 4개: 그리드 (4,18), (5,18), (6,18), (7,18) ← 올바른 매핑
```

`Board.cs`의 `WorldToGrid()` 메서드가 이 변환을 담당:
```csharp
private Vector2Int WorldToGrid(Vector3 worldPos)
{
    Vector3 local = worldPos - transform.position;
    return new Vector2Int(Mathf.RoundToInt(local.x), Mathf.RoundToInt(local.y));
}
```

### 입력 시스템
- Unity New Input System (`UnityEngine.InputSystem`)
- `Keyboard.current.leftArrowKey.wasPressedThisFrame` 방식
- 연속 입력: moveTimer / moveInterval = 0.1f

### Ghost Block 구현
- `Tetromino.Start()`에서 `CreateGhost()` 호출
- 이동/회전마다 `UpdateGhost()` 호출
- 피스를 임시로 내려서 드롭 오프셋 계산 후 원래 위치 복원
- 반투명 Standard 셰이더 (alpha=0.25), renderQueue=3000

### Preview Tetromino 주의사항
- `Spawner.UpdatePreview()`에서 `tetromino.enabled = false` 즉시 실행 필수
- `Destroy()`는 지연 실행이라 Start()가 먼저 실행됨 → enabled=false 사용
- 위치: 월드 (6.5, 6, 0) — Canvas가 아닌 월드 좌표!

### 낙하 속도 공식
```csharp
fallInterval = Mathf.Max(0.05f, 1.0f - (Level - 1) * 0.1f)
```

### 점수표
| 줄 수 | 점수 × 레벨 |
|-------|------------|
| 1줄 | 100 |
| 2줄 | 300 |
| 3줄 | 500 |
| 4줄 | 800 |

---

## 7. PDCA 문서 위치

| 단계 | 경로 |
|------|------|
| Plan | `docs/01-plan/features/unity-tetris.plan.md` |
| Design | `docs/02-design/features/unity-tetris.design.md` |
| 메모리 | `docs/.bkit-memory.json` (currentFeature: unity-tetris, phase: do) |

---

## 8. 다음 세션 시작 절차

1. Unity Editor에서 `TetrisScene` 열기
2. MCP Bridge 실행 확인 (Ctrl+Shift+M)
3. 포트 8080 확인: `netstat -an | grep 8080`
4. 필요시 curl 세션 열기 (위 2번 참조)
5. **Play 버튼으로 게임 테스트 → 체크리스트 확인**

---

## 9. 알려진 이슈

| 이슈 | 상태 | 해결 방법 |
|------|------|-----------|
| `/clear` 후 MCP 툴 사라짐 | 알려진 동작 | curl 우회 방식 사용 |
| MCP WebSocket 재연결 로그 | 무해함 | 무시 가능 |
| `execute_menu_item("Edit/Play")` 실패 | Unity 6 제한 | Unity Editor에서 직접 Play 버튼 클릭 |
| `find_gameobjects` - 비활성 오브젝트 미검색 | 알려진 동작 | GameOverPanel처럼 비활성 오브젝트는 씬 파일 직접 확인 |
