# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is a **Unity 6000.3.8f1** project configured with **MCP for Unity** (Model Context Protocol), enabling AI assistants to directly interact with the Unity Editor. This integration allows Claude Code to create, modify, and manage Unity assets, scenes, scripts, and game objects programmatically.

## MCP for Unity Integration

### What is MCP for Unity?

MCP for Unity (v9.4.6) is a bridge that connects AI assistants to Unity via the Model Context Protocol. It provides a comprehensive set of tools to control Unity Editor operations directly from Claude Code.

**GitHub**: https://github.com/CoplayDev/unity-mcp
**Discord**: https://discord.gg/y4p8KfzrN4

### Opening the MCP Window

Access the MCP for Unity control panel:
```
Unity Menu: Window > MCP for Unity
Keyboard Shortcut: Cmd+Shift+M (macOS) / Ctrl+Shift+M (Windows/Linux)
```

### Initial Setup

If MCP is not configured yet:

1. Open **Window > MCP for Unity**
2. Click **"Auto-Setup"**
3. Follow prompts to:
   - Register with Claude Code CLI
   - Start the Unity Bridge
   - Verify server installation

The window shows four main sections:
- **Server Status**: Shows if MCP server is installed and running
- **Unity Bridge**: Controls the bridge process (must be "Running")
- **MCP Client Configuration**: Claude Code registration status
- **Script Validation**: Code validation level settings

### Troubleshooting MCP Connection

- If Claude CLI not found: Click "Choose Claude Install Location" in MCP Client Configuration
- If Unity Bridge is stopped: Click "Start Bridge"
- For detailed logs: Enable "Show Debug Logs" in the MCP window header
- Full troubleshooting: https://github.com/CoplayDev/unity-mcp/wiki/2.-Fix-Unity-MCP-and-Claude-Code

## Available MCP Tools

When the Unity Bridge is running, you have access to these Unity-specific MCP tools through Claude Code:

### Core Management Tools
- **ManageScript**: Create, modify, read, and validate C# scripts with full Roslyn semantic analysis
- **ManageScene**: Create, open, save, and modify Unity scenes; manage scene hierarchy
- **ManageAsset**: Import, create, delete, and modify Unity assets
- **ManageComponents**: Add, configure, remove, and inspect components on GameObjects
- **ManageMaterial**: Create and modify materials, set shader properties, assign textures
- **ManageTexture**: Import, create, and configure textures and sprites
- **ManageShader**: Create and modify shader files
- **ManageScriptableObject**: Create and manage ScriptableObjects
- **ManageAnimation**: Create animation clips, controllers, and configure animation states

### GameObject Operations
- **GameObjectCreate**: Instantiate GameObjects, primitives, or prefabs
- **GameObjectDelete**: Remove GameObjects from scenes
- **GameObjectModify**: Change transforms, names, layers, tags
- **GameObjectDuplicate**: Clone existing GameObjects
- **GameObjectMoveRelative**: Apply relative transformations
- **FindGameObjects**: Search for GameObjects by name, tag, or component

### Editor Control
- **ExecuteMenuItem**: Trigger Unity menu items programmatically
- **RefreshUnity**: Refresh the asset database and reimport assets
- **ReadConsole**: Access Unity Console logs, warnings, and errors
- **RunTests**: Execute Unity Test Framework tests (PlayMode and EditMode)
- **BatchExecute**: Run multiple MCP commands in sequence

### Animation Tools
- **AnimatorControl**: Manage Animator parameters and states
- **AnimatorRead**: Inspect Animator configurations
- **ClipCreate**: Generate animation clips programmatically
- **ControllerCreate**: Create Animator Controllers with states and transitions
- **ControllerLayers**: Manage Animator Controller layers
- **ControllerBlendTrees**: Configure blend tree nodes

## Unity Development Commands

### Script Validation Levels

Set validation level in **Window > MCP for Unity > Script Validation**:
- **Basic**: Syntax checks only
- **Standard**: Syntax + Unity practices (recommended)
- **Comprehensive**: All checks + semantic analysis
- **Strict**: Full semantic validation (requires Roslyn)

### Asset Refresh

After creating or modifying assets outside Unity:
```
Use MCP tool: RefreshUnity
Or Unity Menu: Assets > Refresh
```

### Testing

Run tests using MCP:
```
Use MCP tool: RunTests with parameters for test mode (EditMode/PlayMode)
Or Unity Menu: Window > General > Test Runner
```

### Console Monitoring

Access Unity Console logs via MCP:
```
Use MCP tool: ReadConsole to fetch recent logs, warnings, and errors
```

## Project Structure

### Key Directories
- **Assets/**: Project assets (currently minimal - Input Actions and Scenes)
- **Assets/Scenes/**: Unity scene files (SampleScene.unity)
- **Assets/Screenshots/**: Auto-generated folder for MCP screenshot captures
- **Library/**: Unity cache and package cache (not tracked in git)
- **ProjectSettings/**: Unity project configuration files
- **Packages/**: Package dependencies manifest

### Important Files
- **Packages/manifest.json**: Package dependencies including `com.coplaydev.unity-mcp`
- **Assets/InputSystem_Actions.inputactions**: New Input System action map with Player actions (Move, Look, Attack)
- **ProjectSettings/ProjectSettings.asset**: Core Unity project settings
- **ProjectSettings/ProjectVersion.txt**: Unity editor version (6000.3.8f1)

## Input System

This project uses **Unity's New Input System** (v1.18.0).

### Input Actions Configuration
Located at: `Assets/InputSystem_Actions.inputactions`

Available action maps:
- **Player**: Contains Move (Vector2), Look (Vector2), and Attack (Button) actions

### Generating Input Action Classes
When modifying `.inputactions` files, regenerate C# wrapper classes:
```
Select the .inputactions asset in Unity
Inspector > Generate C# Class (if enabled)
Or manually trigger code generation
```

## Architecture Notes

### MCP Communication Flow
1. Claude Code sends MCP tool requests → 2. Unity Bridge receives and routes commands → 3. MCP Tool handlers execute in Unity Editor → 4. Results returned to Claude Code via Bridge

The Unity Bridge must be running for any MCP operations to work.

### Asset Management
- All asset paths should use forward slashes (`/`) and be relative to the Assets folder
- Use `RefreshUnity` after file system operations to ensure Unity recognizes changes
- Asset GUIDs are stable identifiers; avoid relying on file paths alone for references

### Scene Management
- Only one scene can be actively edited at a time in MCP operations
- Always save scenes explicitly using `ManageScene` before switching
- Use scene paths like "Assets/Scenes/SceneName.unity" for operations

### Script Creation Best Practices
- Use `ManageScript` with appropriate validation level based on complexity
- Scripts must be in folders under Assets/ to be recognized by Unity
- MonoBehaviour scripts should match their class name with filename
- Use namespaces to organize code and avoid naming conflicts

## Common Workflows

### Creating a New GameObject with Component
1. Use `GameObjectCreate` to create the GameObject in the scene
2. Use `ManageComponents` to add required components
3. Use `GameObjectModify` to set transform properties
4. Optionally use `ManageScript` to create custom component scripts

### Building a Scene
1. Use `ManageScene` to create or open a scene
2. Use various GameObject tools to populate the scene
3. Use `ManageAsset` to create prefabs from configured GameObjects
4. Use `ManageScene` to save the scene

### Creating Animated Objects
1. Create GameObject and add Animator component
2. Use `ControllerCreate` to make an Animator Controller
3. Use `ClipCreate` to generate animation clips
4. Use `ControllerLayers` to configure controller states and transitions
5. Assign the controller to the GameObject's Animator

### Testing Changes
1. Make modifications using MCP tools
2. Use `RefreshUnity` to ensure Unity sees all changes
3. Use `ReadConsole` to check for compilation errors
4. Use `RunTests` to execute automated tests
5. Manually test in Unity Editor Play Mode

## Tips for Working with Claude Code

- Always ensure the Unity Bridge is running before attempting MCP operations
- Use `ReadConsole` frequently to catch errors early
- When creating multiple related assets, use `BatchExecute` for atomic operations
- Screenshots can be captured programmatically using the ScreenshotUtility Runtime helper
- For complex multi-step operations, verify intermediate results with read operations before proceeding
- Unity asset paths are case-sensitive on some platforms - maintain consistent casing

## Unity-Specific Considerations

### Serialization
- Unity uses its own serialization system; not all C# features serialize
- Public fields and fields with `[SerializeField]` are serialized
- Use `[System.Serializable]` for custom classes that should serialize

### Component Lifecycle
- Key methods: `Awake()`, `OnEnable()`, `Start()`, `Update()`, `OnDisable()`, `OnDestroy()`
- Use `Awake()` for initialization, `Start()` for setup after all components are initialized
- Coroutines use `IEnumerator` and `yield return` statements

### Physics and Collisions
- 2D: Use Rigidbody2D, Collider2D (BoxCollider2D, CircleCollider2D, etc.)
- 3D: Use Rigidbody, Collider (BoxCollider, SphereCollider, etc.)
- Configure collision layers in ProjectSettings/Physics2DSettings or DynamicsManager

### Platform-Specific Code
Use platform defines:
```csharp
#if UNITY_EDITOR
    // Editor-only code
#elif UNITY_STANDALONE
    // Standalone build code
#endif
```

## Support Resources

- **MCP for Unity Wiki**: https://github.com/CoplayDev/unity-mcp/wiki
- **Unity Documentation**: https://docs.unity3d.com/6000.3/Documentation/Manual/
- **Unity Scripting Reference**: https://docs.unity3d.com/6000.3/Documentation/ScriptReference/
- **New Input System**: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.18/manual/
