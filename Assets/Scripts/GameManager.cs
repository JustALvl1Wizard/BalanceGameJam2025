using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string playerName = "Player";

    const int MENU_SCENE = 0;
    const int GAME_SCENE = 1;
    const int LEADERBOARD_SCENE = 2;

    [Header("Boundary Settings")]
    [Tooltip("Side length of your cube prefab.")]
    public float cubeSize = 1f;
    [Tooltip("Extra padding below the platform to place the 'line'.")]
    public float boundaryOffset = 0.1f;

    float boundaryY;
    public float elapsedTime { get; private set; }
    bool isRunning = false;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy() => SceneManager.sceneLoaded -= OnSceneLoaded;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == GAME_SCENE)
        {
            // Find the platform collider to get its bottom Y
            Collider plat = UnityEngine.Object.FindFirstObjectByType<Collider>();
            if (plat != null)
            {
                // bottom of platform minus half a cube plus any extra offset
                boundaryY = plat.bounds.min.y - (cubeSize * 0.5f) - boundaryOffset;
            }
        }
    }

    void Update()
    {
        if (!isRunning || SceneManager.GetActiveScene().buildIndex != GAME_SCENE)
            return;

        elapsedTime += Time.deltaTime;

        // If any cube goes below boundaryY, immediate Game Over
        foreach (var cube in GameObject.FindGameObjectsWithTag("StackCube"))
        {
            if (cube.transform.position.y < boundaryY)
            {
                // optional: destroy(cube);
                TriggerGameOver();
                break;
            }
        }
    }

    /// <summary>Called by Start button in Menu.</summary>
    public void StartGame()
    {
        elapsedTime = 0f;
        isRunning = true;
        SceneManager.LoadScene(GAME_SCENE);
    }

    void TriggerGameOver()
    {
        if (!isRunning) return;
        isRunning = false;
        SceneManager.LoadScene(LEADERBOARD_SCENE);
    }

    /// <summary>Called by Quit button.</summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}