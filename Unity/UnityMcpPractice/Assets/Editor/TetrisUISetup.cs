using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Tetris UI 요소를 자동으로 생성하고 연결하는 Editor 유틸리티
/// Menu: Tools > Tetris > Setup UI
/// </summary>
public class TetrisUISetup : Editor
{
    [MenuItem("Tools/Tetris/Setup UI")]
    public static void SetupUI()
    {
        // Canvas 찾기
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("[TetrisUISetup] Canvas를 찾을 수 없습니다.");
            return;
        }

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        // GameManager, Spawner 찾기
        GameManager gameManager = FindAnyObjectByType<GameManager>();
        Spawner spawner = FindAnyObjectByType<Spawner>();

        // --- HUD 패널 (좌상단: Score, Level, Lines) ---
        GameObject hudPanel = CreatePanel(canvas.transform, "HUDPanel",
            new Vector2(160, 200), new Vector2(-90, -120),
            new Vector2(0, 1), new Vector2(0, 1));

        TMP_Text scoreLabel = CreateLabel(hudPanel.transform, "ScoreLabel", "SCORE",
            new Vector2(0, -10), new Vector2(160, 30));
        TMP_Text scoreText = CreateLabel(hudPanel.transform, "ScoreText", "0",
            new Vector2(0, -45), new Vector2(160, 40), 28);

        TMP_Text levelLabel = CreateLabel(hudPanel.transform, "LevelLabel", "LEVEL",
            new Vector2(0, -90), new Vector2(160, 30));
        TMP_Text levelText = CreateLabel(hudPanel.transform, "LevelText", "1",
            new Vector2(0, -125), new Vector2(160, 40), 28);

        TMP_Text linesLabel = CreateLabel(hudPanel.transform, "LinesLabel", "LINES",
            new Vector2(0, -170), new Vector2(160, 30));
        TMP_Text linesText = CreateLabel(hudPanel.transform, "LinesText", "0",
            new Vector2(0, -205), new Vector2(160, 40), 28);

        // GameManager에 연결
        if (gameManager != null)
        {
            gameManager.scoreText = scoreText;
            gameManager.levelText = levelText;
            gameManager.linesText = linesText;
            EditorUtility.SetDirty(gameManager);
            Debug.Log("[TetrisUISetup] GameManager UI 연결 완료");
        }

        // --- Next Piece 패널 (우상단) ---
        GameObject nextPanel = CreatePanel(canvas.transform, "NextPiecePanel",
            new Vector2(160, 160), new Vector2(90, -100),
            new Vector2(1, 1), new Vector2(1, 1));

        CreateLabel(nextPanel.transform, "NextLabel", "NEXT",
            new Vector2(0, -10), new Vector2(160, 30));

        GameObject nextDisplay = new GameObject("NextPieceDisplay");
        nextDisplay.transform.SetParent(nextPanel.transform, false);
        RectTransform nextDisplayRect = nextDisplay.AddComponent<RectTransform>();
        nextDisplayRect.anchoredPosition = new Vector2(0, -90);
        nextDisplayRect.sizeDelta = new Vector2(100, 100);

        // Spawner에 연결
        if (spawner != null)
        {
            spawner.nextPreviewParent = nextDisplay.transform;
            EditorUtility.SetDirty(spawner);
            Debug.Log("[TetrisUISetup] Spawner nextPreviewParent 연결 완료");
        }

        // --- Game Over 패널 (화면 중앙) ---
        GameObject gameOverPanel = CreatePanel(canvas.transform, "GameOverPanel",
            new Vector2(300, 250), new Vector2(0, 0),
            new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f));
        gameOverPanel.SetActive(false);

        // 패널 배경색 설정
        Image bgImage = gameOverPanel.GetComponent<Image>();
        if (bgImage != null)
            bgImage.color = new Color(0, 0, 0, 0.85f);

        CreateLabel(gameOverPanel.transform, "GameOverText", "GAME OVER",
            new Vector2(0, 70), new Vector2(280, 60), 36);

        TMP_Text finalScoreText = CreateLabel(gameOverPanel.transform, "FinalScoreText", "Score: 0",
            new Vector2(0, 10), new Vector2(280, 40), 22);

        // Restart 버튼
        GameObject restartBtn = CreateButton(gameOverPanel.transform, "RestartButton",
            "RESTART", new Vector2(0, -60), new Vector2(180, 50));

        Button btnComponent = restartBtn.GetComponent<Button>();
        if (btnComponent != null && gameManager != null)
        {
            UnityEditor.Events.UnityEventTools.AddPersistentListener(
                btnComponent.onClick,
                gameManager.RestartGame
            );
        }

        // GameManager에 게임오버 패널 연결
        if (gameManager != null)
        {
            gameManager.gameOverPanel = gameOverPanel;
            gameManager.finalScoreText = finalScoreText;
            EditorUtility.SetDirty(gameManager);
            Debug.Log("[TetrisUISetup] GameOverPanel 연결 완료");
        }

        // 씬 저장
        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());

        Debug.Log("[TetrisUISetup] UI 설정 완료! 씬을 저장하세요 (Ctrl+S).");
        Selection.activeGameObject = canvas.gameObject;
    }

    // ─── 헬퍼 메서드들 ───────────────────────────────────

    static GameObject CreatePanel(Transform parent, string name,
        Vector2 size, Vector2 anchoredPos,
        Vector2 anchorMin, Vector2 anchorMax)
    {
        // 기존 오브젝트가 있으면 삭제
        Transform existing = parent.Find(name);
        if (existing != null)
        {
            DestroyImmediate(existing.gameObject);
            Debug.Log($"[TetrisUISetup] 기존 {name} 삭제 후 재생성");
        }

        GameObject panel = new GameObject(name);
        panel.transform.SetParent(parent, false);

        RectTransform rect = panel.AddComponent<RectTransform>();
        rect.anchorMin = anchorMin;
        rect.anchorMax = anchorMax;
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPos;

        Image img = panel.AddComponent<Image>();
        img.color = new Color(0, 0, 0, 0.6f);

        return panel;
    }

    static TMP_Text CreateLabel(Transform parent, string name, string text,
        Vector2 anchoredPos, Vector2 size, int fontSize = 18)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 1f);
        rect.anchorMax = new Vector2(0.5f, 1f);
        rect.pivot = new Vector2(0.5f, 1f);
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPos;

        TMP_Text tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text = text;
        tmp.fontSize = fontSize;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return tmp;
    }

    static GameObject CreateButton(Transform parent, string name, string label,
        Vector2 anchoredPos, Vector2 size)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent, false);

        RectTransform rect = go.AddComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.sizeDelta = size;
        rect.anchoredPosition = anchoredPos;

        Image img = go.AddComponent<Image>();
        img.color = new Color(0.2f, 0.6f, 1f, 1f);

        Button btn = go.AddComponent<Button>();

        GameObject textGo = new GameObject("Text");
        textGo.transform.SetParent(go.transform, false);

        RectTransform textRect = textGo.AddComponent<RectTransform>();
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;

        TMP_Text tmp = textGo.AddComponent<TextMeshProUGUI>();
        tmp.text = label;
        tmp.fontSize = 20;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color = Color.white;

        return go;
    }
}
