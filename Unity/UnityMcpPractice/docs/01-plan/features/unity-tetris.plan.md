# Plan: Unity Tetris Game

> 상태: Plan 작성 완료
> 생성일: 2026-02-21
> 피처명: unity-tetris

---

## 1. 개요 (Overview)

Unity 6000.3.8f1 환경에서 MCP for Unity를 활용하여 **클래식 테트리스 게임**을 구현한다.
기본적인 테트리스 규칙(7가지 테트로미노, 라인 클리어, 점수 시스템)을 포함한 플레이어블 게임을 목표로 한다.
기존 SampleScene을 사용하지 않고 **새 씬(TetrisScene)을 생성하여 작업**한다.

---

## 2. 목표 (Goals)

- [x] 7가지 테트로미노(I, O, T, S, Z, J, L) 구현
- [x] 블록 낙하, 회전, 이동 조작 구현
- [x] 라인 클리어 및 점수 시스템 구현
- [x] 게임 오버 / 재시작 기능 구현
- [x] 간단한 UI (점수, 레벨, 다음 블록 미리보기) 구현
- [x] Unity MCP를 통해 씬/스크립트/오브젝트 생성

---

## 3. 범위 (Scope)

### In Scope
- 10x20 그리드 기반 게임 보드
- 테트로미노 생성 및 랜덤 선택
- 키보드 조작 (← → ↓ 이동, ↑ 또는 Z 회전, Space 하드드롭)
- 라인 클리어 감지 및 블록 처리
- 점수 계산 (1줄=100점, 2줄=300점, 3줄=500점, 4줄=800점)
- 레벨 증가에 따른 낙하 속도 가속
- 다음 블록 미리보기 패널
- 게임 오버 감지 및 재시작

### Out of Scope
- 멀티플레이어
- 홀드 기능
- 모바일 터치 조작
- 음악/효과음
- 저장/로드 기능

---

## 4. 기술 스택 (Tech Stack)

| 항목 | 내용 |
|------|------|
| 엔진 | Unity 6000.3.8f1 |
| 언어 | C# |
| 렌더링 | 2D (Sprite Renderer 또는 UI Image) |
| UI | Unity UI (Canvas) |
| 입력 | Unity New Input System (Keyboard) |
| MCP | MCP for Unity v9.4.6 (씬/스크립트/오브젝트 생성) |

---

## 5. 아키텍처 구조 (Architecture)

```
TetrisScene (Assets/Scenes/TetrisScene.unity) ← 새로 생성, SampleScene 미사용
├── GameManager          ← 게임 상태 관리 (시작/종료/재시작)
├── Board                ← 10x20 그리드 데이터 및 렌더링
│   └── Cell[,]          ← 각 셀의 점유 상태
├── Spawner              ← 테트로미노 생성 및 랜덤 선택
├── Tetromino (prefab)   ← 현재 낙하 중인 블록
│   └── Blocks[]         ← 4개의 큐브 셀
└── UI
    ├── ScoreText
    ├── LevelText
    └── NextPiecePanel
```

---

## 6. 주요 컴포넌트 설계 (Key Components)

### GameManager.cs
- 게임 상태 관리 (Playing / GameOver)
- 점수, 레벨 관리
- 씬 재시작

### Board.cs
- 10x20 2차원 배열로 그리드 관리
- 블록 배치 유효성 검사 (IsValidPosition)
- 라인 클리어 감지 및 처리 (ClearLines)
- 블록 고정 (PlacePiece)

### Tetromino.cs
- 7가지 테트로미노 형태 정의 (배열 또는 ScriptableObject)
- 이동 (MoveLeft, MoveRight, MoveDown)
- 회전 (Rotate) - Wall kick 포함
- 하드드롭 (HardDrop)
- 자동 낙하 타이머

### Spawner.cs
- 랜덤 테트로미노 생성
- 다음 블록 미리보기 관리
- 7-bag 랜덤 알고리즘 (선택 사항)

---

## 7. 구현 순서 (Implementation Order)

```
Phase 1: 씬 및 기본 구조 설정
  1-1. Assets/Scenes/TetrisScene.unity 새 씬 생성 (SampleScene 미사용)
  1-2. TetrisScene을 활성 씬으로 설정
  1-3. Board 그리드 오브젝트 생성
  1-4. Camera 설정 (Orthographic, 10x20 뷰)

Phase 2: 핵심 게임 로직
  2-1. Board.cs 구현 (그리드 관리)
  2-2. Tetromino.cs 구현 (블록 이동/회전)
  2-3. Spawner.cs 구현 (블록 생성)

Phase 3: 게임 규칙
  3-1. 충돌 감지 및 고정 로직
  3-2. 라인 클리어 구현
  3-3. 게임 오버 판정

Phase 4: UI 및 게임 관리
  4-1. GameManager.cs 구현
  4-2. Score/Level UI 연결
  4-3. 다음 블록 미리보기 구현
  4-4. 게임 오버 화면 및 재시작
```

---

## 8. 위험 요소 (Risks)

| 위험 | 대응 방안 |
|------|-----------|
| Wall Kick 회전 구현 복잡성 | SRS 표준 대신 단순 회전으로 시작 후 개선 |
| MCP 연결 끊김 | curl 우회 방식으로 대응 (트러블슈팅 문서 참고) |
| Unity 2D vs UI 렌더링 선택 | UI Canvas 기반으로 통일 (해상도 독립적) |
| 새 씬 생성 후 Build Settings 누락 | 씬 생성 후 File > Build Settings에 TetrisScene 추가 필요 |

---

## 9. 완료 기준 (Definition of Done)

- [ ] TetrisScene이 Assets/Scenes/에 생성됨
- [ ] 게임이 Unity Editor에서 실행 가능
- [ ] 7가지 테트로미노가 랜덤으로 생성됨
- [ ] 키보드로 블록 이동/회전/드롭 가능
- [ ] 라인 클리어 시 점수 증가
- [ ] 게임 오버 후 재시작 가능
- [ ] 콘솔 에러 없음
