using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text;

public class LeaderboardManager : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("TMP_Text to show the current player's name and time.")]
    public TMP_Text yourTimeText;
    [Tooltip("Single TMP_Text to list all high scores in two columns.")]
    public TMP_Text scoresText;

    [Header("Buttons")]
    public Button playAgainButton;
    public Button quitButton;

    // Serializable container for JSON storage
    [System.Serializable]
    private class ScoreEntry
    {
        public string name;
        public float time;
    }
    [System.Serializable]
    private class ScoreList
    {
        public List<ScoreEntry> entries = new List<ScoreEntry>();
    }

    void Start()
    {
        // wire up buttons
        playAgainButton.onClick.AddListener(GameManager.Instance.StartGame);
        quitButton.onClick.AddListener(GameManager.Instance.QuitGame);

        // show this run
        string player = GameManager.Instance.playerName;
        float t = GameManager.Instance.elapsedTime;
        yourTimeText.text = $"{player}: {FormatTime(t)}";

        // load, insert, sort, trim
        string json = PlayerPrefs.GetString("highscores", "");
        var sl = string.IsNullOrEmpty(json)
                 ? new ScoreList()
                 : JsonUtility.FromJson<ScoreList>(json);
        sl.entries.Add(new ScoreEntry { name = player, time = t });
        sl.entries.Sort((a, b) => b.time.CompareTo(a.time));
        if (sl.entries.Count > 10)
            sl.entries = sl.entries.GetRange(0, 10);

        // save back
        PlayerPrefs.SetString("highscores", JsonUtility.ToJson(sl));
        PlayerPrefs.Save();

        // build two-column text
        var sb = new StringBuilder();
        int rows = 5; // always show up to 5 rows
        for (int r = 0; r < rows; r++)
        {
            // left column
            string left = "";
            if (r < sl.entries.Count)
            {
                var e = sl.entries[r];
                left = $"{r + 1}. {e.name}: {FormatTime(e.time)}";
            }
            // right column
            string right = "";
            int idx = r + rows;
            if (idx < sl.entries.Count)
            {
                var e = sl.entries[idx];
                right = $"{idx + 1}. {e.name}: {FormatTime(e.time)}";
            }
            // pad left to 40 chars so columns line up (tweak as needed)
            sb.AppendLine(left.PadRight(40) + right);
        }

        scoresText.text = sb.ToString();
    }

    string FormatTime(float t)
    {
        int m = (int)(t / 60);
        int s = (int)(t % 60);
        int ms = (int)((t * 1000) % 1000);
        return $"{m:00}:{s:00}.{ms:000}";
    }
}