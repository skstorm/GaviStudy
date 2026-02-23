using UnityEngine;
using UnityEngine.InputSystem;

public class Tetromino : MonoBehaviour
{
    private Board board;
    private Spawner spawner;

    private float fallTimer = 0f;
    private float fallInterval = 1.0f;

    private float moveTimer = 0f;
    private float moveInterval = 0.1f;

    // Ghost block (하드드롭 위치 미리보기)
    private GameObject ghostObject;

    void Start()
    {
        board = FindFirstObjectByType<Board>();
        spawner = FindFirstObjectByType<Spawner>();

        if (GameManager.Instance != null)
            fallInterval = GameManager.Instance.GetFallInterval();

        Debug.Log($"[Tetromino] Start: {gameObject.name} at {transform.position}, enabled={enabled}");

        if (!board.IsValidPosition(this))
        {
            Debug.LogWarning($"[Tetromino] GAMEOVER triggered by {gameObject.name} at {transform.position}");
            GameManager.Instance?.GameOver();
            enabled = false;
            return;
        }

        CreateGhost();
        UpdateGhost();
    }

    void Update()
    {
        if (GameManager.Instance != null && GameManager.Instance.IsGameOver) return;

        HandleInput();
        HandleAutoFall();
    }

    private void HandleInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        // 좌우 이동 (wasPressedThisFrame + 연속 입력)
        if (kb.leftArrowKey.wasPressedThisFrame) MoveLeft();
        if (kb.rightArrowKey.wasPressedThisFrame) MoveRight();

        // 연속 이동
        if (kb.leftArrowKey.isPressed || kb.rightArrowKey.isPressed)
        {
            moveTimer += Time.deltaTime;
            if (moveTimer >= moveInterval)
            {
                moveTimer = 0f;
                if (kb.leftArrowKey.isPressed) MoveLeft();
                if (kb.rightArrowKey.isPressed) MoveRight();
            }
        }
        else
        {
            moveTimer = 0f;
        }

        // 소프트 드롭
        if (kb.downArrowKey.isPressed)
        {
            fallTimer += Time.deltaTime * 10f;
        }

        // 회전
        if (kb.upArrowKey.wasPressedThisFrame || kb.zKey.wasPressedThisFrame)
            Rotate();

        // 하드 드롭
        if (kb.spaceKey.wasPressedThisFrame)
            HardDrop();
    }

    private void HandleAutoFall()
    {
        fallTimer += Time.deltaTime;
        if (fallTimer >= fallInterval)
        {
            fallTimer = 0f;
            if (!MoveDown())
                Lock();
        }
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left;
        if (!board.IsValidPosition(this))
            transform.position += Vector3.right;
        UpdateGhost();
    }

    private void MoveRight()
    {
        transform.position += Vector3.right;
        if (!board.IsValidPosition(this))
            transform.position += Vector3.left;
        UpdateGhost();
    }

    private bool MoveDown()
    {
        transform.position += Vector3.down;
        if (!board.IsValidPosition(this))
        {
            transform.position += Vector3.up;
            return false;
        }
        return true;
    }

    private void Rotate()
    {
        transform.Rotate(0, 0, 90);
        if (!board.IsValidPosition(this))
        {
            // Wall kick: 좌, 우 순서로 시도
            transform.position += Vector3.left;
            if (!board.IsValidPosition(this))
            {
                transform.position += Vector3.right * 2;
                if (!board.IsValidPosition(this))
                {
                    transform.position += Vector3.left;
                    transform.Rotate(0, 0, -90); // 회전 복원
                }
            }
        }
        UpdateGhost();
    }

    private void HardDrop()
    {
        while (MoveDown()) { }
        Lock();
    }

    private void Lock()
    {
        Debug.Log($"[Tetromino] Lock: {gameObject.name} at {transform.position}");
        DestroyGhost();
        board.PlacePiece(this);
        int lines = board.ClearLines();
        if (lines > 0)
            GameManager.Instance?.AddScore(lines);

        spawner.SpawnNext();
        enabled = false;
    }

    // ─── Ghost Block ──────────────────────────────────────

    /// <summary>
    /// 현재 피스 블록들을 기반으로 고스트 오브젝트 생성
    /// 피스의 색상을 반투명으로 적용
    /// </summary>
    private void CreateGhost()
    {
        // 피스에서 색상 가져오기
        Color pieceColor = Color.white;
        var firstRenderer = GetComponentInChildren<Renderer>();
        if (firstRenderer != null)
            pieceColor = firstRenderer.material.color;

        // 반투명 Material 생성
        Material ghostMat = new Material(Shader.Find("Standard"));
        Color ghostColor = new Color(pieceColor.r, pieceColor.g, pieceColor.b, 0.25f);
        ghostMat.color = ghostColor;
        // Standard Shader Transparent 모드 설정
        ghostMat.SetFloat("_Mode", 3);
        ghostMat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        ghostMat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        ghostMat.SetInt("_ZWrite", 0);
        ghostMat.DisableKeyword("_ALPHATEST_ON");
        ghostMat.EnableKeyword("_ALPHABLEND_ON");
        ghostMat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        ghostMat.renderQueue = 3000;

        ghostObject = new GameObject("Ghost_" + name);

        // 피스 자식(블록)마다 Ghost 큐브 생성
        foreach (Transform child in transform)
        {
            var ghostBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
            ghostBlock.name = "GhostBlock";
            ghostBlock.transform.SetParent(ghostObject.transform, false);
            ghostBlock.transform.localScale = child.localScale;

            // Collider 제거 (충돌 없는 순수 시각용)
            var col = ghostBlock.GetComponent<Collider>();
            if (col != null) Destroy(col);

            ghostBlock.GetComponent<Renderer>().material = ghostMat;
        }
    }

    /// <summary>
    /// 피스 이동/회전 후 Ghost 위치 갱신
    /// </summary>
    private void UpdateGhost()
    {
        if (ghostObject == null) return;

        // 현재 위치 저장 후 하드드롭 Y 계산
        Vector3 savedPos = transform.position;

        while (true)
        {
            transform.position += Vector3.down;
            if (!board.IsValidPosition(this))
            {
                transform.position += Vector3.up;
                break;
            }
        }

        // 드롭 오프셋 (Y값만 다름)
        Vector3 dropOffset = transform.position - savedPos;

        // 원래 위치로 복원
        transform.position = savedPos;

        // Ghost 블록들을 실제 블록 + 오프셋 위치에 배치
        int i = 0;
        foreach (Transform child in transform)
        {
            if (i >= ghostObject.transform.childCount) break;
            ghostObject.transform.GetChild(i).position = child.position + dropOffset;
            i++;
        }

        // 이미 바닥에 있으면 Ghost 숨기기
        ghostObject.SetActive(dropOffset.sqrMagnitude > 0.01f);
    }

    private void DestroyGhost()
    {
        if (ghostObject != null)
            Destroy(ghostObject);
    }

    void OnDestroy()
    {
        DestroyGhost();
    }
}
