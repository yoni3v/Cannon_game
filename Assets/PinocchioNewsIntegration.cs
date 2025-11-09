using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// PinocchioNews.io Overlay Integration
/// Connects Unity game to web overlay system
/// Tracks score, enemies killed, level and NFT milestones
/// </summary>
public class PinocchioNewsIntegration : MonoBehaviour
{
    [Header("Game Configuration")]
    public string gameId = "nft-cannon-game";
    public string gameName = "NFT Cannon Game";
    public string gameVersion = "1.0.0";

    [Header("Server URLs")]
    public string serverURL = "http://localhost:8006";
    public string nftServiceURL = "http://localhost:8025";

    [Header("Game Stats")]
    public int currentScore = 0;
    public int enemiesKilled = 0;
    public int currentLevel = 1;

    [Header("NFT Milestones")]
    public bool enableNFTMilestones = true;
    public bool debugMode = true;

    // JavaScript bridge functions (WebGL only)
    [DllImport("__Internal")]
    private static extern void UpdateGameStats(string jsonData);

    [DllImport("__Internal")]
    private static extern void ShowNFTNotification(string message);

    // Singleton instance
    public static PinocchioNewsIntegration Instance { get; private set; }

    // Milestone tracking
    private bool firstKillAchieved = false;
    private bool score1000Achieved = false;
    private bool level5Achieved = false;
    private bool enemy100Achieved = false;
    private bool score10000Achieved = false;

    // Stats tracking
    private float statsUpdateInterval = 1f; // Send stats every 1 second
    private float lastStatsUpdate = 0f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        LogDebug("PinocchioNews Integration Initialized");
        LogDebug($"Game: {gameName} v{gameVersion}");
        LogDebug($"Server URL: {serverURL}");

        // Send initial stats
        SendStatsToOverlay();
    }

    void Update()
    {
        // Periodically update overlay with stats
        if (Time.time - lastStatsUpdate > statsUpdateInterval)
        {
            SendStatsToOverlay();
            lastStatsUpdate = Time.time;
        }
    }

    /// <summary>
    /// Call this when player scores points
    /// </summary>
    public void AddScore(int points)
    {
        currentScore += points;
        LogDebug($"Score added: {points}, Total: {currentScore}");

        CheckMilestones();
        SendStatsToOverlay();
    }

    /// <summary>
    /// Call this when enemy is killed
    /// </summary>
    public void RegisterKill()
    {
        enemiesKilled++;
        LogDebug($"Enemy killed! Total: {enemiesKilled}");

        CheckMilestones();
        SendStatsToOverlay();
    }

    /// <summary>
    /// Call this when level changes
    /// </summary>
    public void SetLevel(int level)
    {
        currentLevel = level;
        LogDebug($"Level changed to: {level}");

        CheckMilestones();
        SendStatsToOverlay();
    }

    /// <summary>
    /// Check if any milestones have been achieved
    /// </summary>
    private void CheckMilestones()
    {
        if (!enableNFTMilestones) return;

        // Milestone 1: First Kill
        if (!firstKillAchieved && enemiesKilled >= 1)
        {
            firstKillAchieved = true;
            TriggerMilestone("first_kill", "First Kill!", "You killed your first enemy!");
        }

        // Milestone 2: Score 1000
        if (!score1000Achieved && currentScore >= 1000)
        {
            score1000Achieved = true;
            TriggerMilestone("score_1000", "Score Master!", "You reached 1000 points!");
        }

        // Milestone 3: Level 5
        if (!level5Achieved && currentLevel >= 5)
        {
            level5Achieved = true;
            TriggerMilestone("level_5", "Level 5 Warrior!", "You reached level 5!");
        }

        // Milestone 4: 100 Enemies
        if (!enemy100Achieved && enemiesKilled >= 100)
        {
            enemy100Achieved = true;
            TriggerMilestone("enemy_100", "Enemy Destroyer!", "You killed 100 enemies!");
        }

        // Milestone 5: Score 10000
        if (!score10000Achieved && currentScore >= 10000)
        {
            score10000Achieved = true;
            TriggerMilestone("score_10000", "Score Legend!", "You reached 10,000 points!");
        }
    }

    /// <summary>
    /// Trigger an NFT milestone achievement
    /// </summary>
    private void TriggerMilestone(string milestoneId, string title, string description)
    {
        LogDebug($"?? MILESTONE ACHIEVED: {title}");

        // Create milestone message for JavaScript
        string message = $"{{\"milestone\":\"{milestoneId}\",\"title\":\"{title}\",\"description\":\"{description}\"}}";

        // Send to JavaScript overlay (WebGL only)
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            ShowNFTNotification(message);
        }
        catch (System.Exception e)
        {
            LogDebug($"Failed to show notification: {e.Message}");
        }
#else
        LogDebug($"[EDITOR MODE] Would show notification: {title}");
#endif

        // Send to server API
        StartCoroutine(SendMilestoneToServer(milestoneId, title, description));
    }

    /// <summary>
    /// Send current game stats to overlay (JavaScript)
    /// </summary>
    private void SendStatsToOverlay()
    {
        string jsonData = $"{{\"score\":{currentScore},\"enemies\":{enemiesKilled},\"level\":{currentLevel}}}";

        // Send to JavaScript overlay (WebGL only)
#if UNITY_WEBGL && !UNITY_EDITOR
        try
        {
            UpdateGameStats(jsonData);
        }
        catch (System.Exception e)
        {
            // Silent fail in case JavaScript bridge isn't ready
        }
#endif
    }

    /// <summary>
    /// Send milestone achievement to server API
    /// </summary>
    private IEnumerator SendMilestoneToServer(string milestoneId, string title, string description)
    {
        string url = $"{serverURL}/api/game/milestone";

        string json = $"{{" +
            $"\"gameType\":\"cannon\"," +
            $"\"milestone\":{{\"id\":\"{milestoneId}\",\"name\":\"{title}\"}}," +
            $"\"stats\":{{\"score\":{currentScore},\"enemiesKilled\":{enemiesKilled},\"level\":{currentLevel}}}," +
            $"\"timestamp\":\"{System.DateTime.UtcNow:o}\"" +
            $"}}";

        LogDebug($"Sending milestone to server: {url}");

        using (UnityEngine.Networking.UnityWebRequest request = UnityEngine.Networking.UnityWebRequest.Post(url, json, "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                LogDebug("? Milestone sent to server successfully");
            }
            else
            {
                LogDebug($"?? Failed to send milestone: {request.error}");
            }
        }
    }

    /// <summary>
    /// Reset game stats (for new game)
    /// </summary>
    public void ResetGame()
    {
        currentScore = 0;
        enemiesKilled = 0;
        currentLevel = 1;

        firstKillAchieved = false;
        score1000Achieved = false;
        level5Achieved = false;
        enemy100Achieved = false;
        score10000Achieved = false;

        LogDebug("Game stats reset");
        SendStatsToOverlay();
    }

    /// <summary>
    /// Debug logging (can be disabled)
    /// </summary>
    private void LogDebug(string message)
    {
        if (debugMode)
        {
            Debug.Log($"[PinocchioNews] {message}");
        }
    }

    /// <summary>
    /// Get current score (for other scripts)
    /// </summary>
    public int GetScore()
    {
        return currentScore;
    }

    /// <summary>
    /// Get enemies killed count
    /// </summary>
    public int GetEnemiesKilled()
    {
        return enemiesKilled;
    }

    /// <summary>
    /// Get current level
    /// </summary>
    public int GetLevel()
    {
        return currentLevel;
    }
}