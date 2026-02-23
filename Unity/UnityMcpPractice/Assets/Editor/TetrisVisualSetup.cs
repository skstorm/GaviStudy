using UnityEngine;
using UnityEditor;

/// <summary>
/// Tetris 시각적 요소 설정:
/// 1. 테트로미노 색상 Material 생성 및 적용
/// 2. 카메라 배경색 변경
/// 3. 게임 보드 외곽선 생성
/// Menu: Tools/Tetris/Setup Visuals
/// </summary>
public class TetrisVisualSetup : Editor
{
    static readonly (string name, Color color)[] PieceColors = new[]
    {
        ("I_Piece", new Color(0.0f, 0.9f, 1.0f, 1f)),   // 시안
        ("O_Piece", new Color(1.0f, 0.9f, 0.0f, 1f)),   // 노랑
        ("T_Piece", new Color(0.6f, 0.0f, 1.0f, 1f)),   // 보라
        ("S_Piece", new Color(0.0f, 0.8f, 0.0f, 1f)),   // 초록
        ("Z_Piece", new Color(1.0f, 0.1f, 0.1f, 1f)),   // 빨강
        ("J_Piece", new Color(0.0f, 0.2f, 1.0f, 1f)),   // 파랑
        ("L_Piece", new Color(1.0f, 0.5f, 0.0f, 1f)),   // 주황
    };

    [MenuItem("Tools/Tetris/Setup Visuals")]
    public static void SetupVisuals()
    {
        // 폴더 생성
        if (!AssetDatabase.IsValidFolder("Assets/Materials"))
            AssetDatabase.CreateFolder("Assets", "Materials");
        if (!AssetDatabase.IsValidFolder("Assets/Materials/Tetris"))
            AssetDatabase.CreateFolder("Assets/Materials", "Tetris");

        Shader shader = GetBestShader();
        Debug.Log("[TetrisVisualSetup] 사용 셰이더: " + (shader != null ? shader.name : "NULL!"));

        if (shader == null)
        {
            Debug.LogError("[TetrisVisualSetup] 셰이더를 찾을 수 없습니다. 중단합니다.");
            return;
        }

        // 1. Material 생성 및 프리팹 적용
        ApplyMaterialsToPrefabs(shader);

        // 2. 카메라 배경색
        SetCameraBackground();

        // 3. 보드 외곽선
        CreateBoardBorder(shader);

        UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene());
        AssetDatabase.SaveAssets();
        Debug.Log("[TetrisVisualSetup] 모든 시각 설정 완료!");
    }

    static Shader GetBestShader()
    {
        // 순서대로 시도
        string[] candidates = {
            "Universal Render Pipeline/Lit",
            "Universal Render Pipeline/Simple Lit",
            "Universal Render Pipeline/Unlit",
            "Standard",
            "Sprites/Default",
            "Unlit/Color",
        };
        foreach (var name in candidates)
        {
            Shader s = Shader.Find(name);
            if (s != null) return s;
        }
        return null;
    }

    static Material GetOrCreateMaterial(string path, Color color, Shader shader)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat == null)
        {
            mat = new Material(shader);
            AssetDatabase.CreateAsset(mat, path);
        }
        else
        {
            mat.shader = shader;
        }
        mat.color = color;
        EditorUtility.SetDirty(mat);
        return mat;
    }

    static void ApplyMaterialsToPrefabs(Shader shader)
    {
        foreach (var (pieceName, color) in PieceColors)
        {
            string matPath = $"Assets/Materials/Tetris/{pieceName}_Mat.mat";
            Material mat = GetOrCreateMaterial(matPath, color, shader);

            // Assets/Prefabs/Tetris 경로
            string prefabPath = $"Assets/Prefabs/Tetris/{pieceName}.prefab";
            ApplyMatToPrefab(prefabPath, mat);

            // Assets/Resources/Tetris 경로
            string resPrefabPath = $"Assets/Resources/Tetris/{pieceName}.prefab";
            ApplyMatToPrefab(resPrefabPath, mat);
        }
        Debug.Log("[TetrisVisualSetup] Material 7종 생성 및 적용 완료");
    }

    static void ApplyMatToPrefab(string prefabPath, Material mat)
    {
        if (string.IsNullOrEmpty(AssetDatabase.AssetPathToGUID(prefabPath)))
        {
            Debug.LogWarning($"[TetrisVisualSetup] 프리팹 없음: {prefabPath}");
            return;
        }

        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
        if (prefab == null) return;

        // PrefabUtility를 사용해 안전하게 편집
        string tempPath = prefabPath;
        try
        {
            using (var scope = new PrefabUtility.EditPrefabContentsScope(tempPath))
            {
                var root = scope.prefabContentsRoot;
                var renderers = root.GetComponentsInChildren<Renderer>(true);
                if (renderers.Length > 0)
                {
                    foreach (var r in renderers)
                        r.sharedMaterial = mat;
                }
                else
                {
                    var r = root.GetComponent<Renderer>();
                    if (r != null) r.sharedMaterial = mat;
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"[TetrisVisualSetup] {prefabPath} 편집 실패: {e.Message}");
        }
    }

    static void SetCameraBackground()
    {
        Camera cam = Camera.main;
        if (cam == null) cam = Object.FindAnyObjectByType<Camera>();
        if (cam != null)
        {
            cam.backgroundColor = new Color(0.08f, 0.08f, 0.12f, 1f);
            cam.clearFlags = CameraClearFlags.SolidColor;
            EditorUtility.SetDirty(cam);
            Debug.Log("[TetrisVisualSetup] 카메라 배경색 변경 완료");
        }
        else
        {
            Debug.LogWarning("[TetrisVisualSetup] Camera 없음");
        }
    }

    static void CreateBoardBorder(Shader shader)
    {
        // 기존 삭제
        var existing = GameObject.Find("BoardBorder");
        if (existing != null) Object.DestroyImmediate(existing);

        // 회색 Material
        string borderMatPath = "Assets/Materials/Tetris/Border_Mat.mat";
        Material borderMat = GetOrCreateMaterial(
            borderMatPath,
            new Color(0.35f, 0.35f, 0.45f, 1f),
            shader);

        GameObject border = new GameObject("BoardBorder");

        // Board 오프셋: (-4.5, -9.5, 0), 크기 10x20
        float bx = -4.5f, by = -9.5f;
        int w = 10, h = 20;

        // 왼쪽 벽
        CreateSeg(border.transform, "LeftWall",
            new Vector3(bx - 0.55f, by + h / 2f - 0.5f, 0),
            new Vector3(0.1f, h + 0.2f, 0.1f), borderMat);

        // 오른쪽 벽
        CreateSeg(border.transform, "RightWall",
            new Vector3(bx + w - 0.45f, by + h / 2f - 0.5f, 0),
            new Vector3(0.1f, h + 0.2f, 0.1f), borderMat);

        // 바닥
        CreateSeg(border.transform, "BottomWall",
            new Vector3(bx + w / 2f - 0.5f, by - 0.55f, 0),
            new Vector3(w + 0.2f, 0.1f, 0.1f), borderMat);

        Debug.Log("[TetrisVisualSetup] 보드 외곽선 생성 완료");
    }

    static void CreateSeg(Transform parent, string name,
        Vector3 pos, Vector3 scale, Material mat)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.SetParent(parent);
        go.transform.position = pos;
        go.transform.localScale = scale;
        var col = go.GetComponent<Collider>();
        if (col != null) Object.DestroyImmediate(col);
        go.GetComponent<Renderer>().sharedMaterial = mat;
    }
}
