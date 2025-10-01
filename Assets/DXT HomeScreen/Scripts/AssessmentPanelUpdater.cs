using UnityEngine;
using UnityEngine.UI;

public class AssessmentPanelUpdater : MonoBehaviour
{
    [Header("UI References")]
    public Text reportText; // "Assessment Report" description
    public Button retryButton;  // The button whose label changes
    public Text retryButtonText; // The text component inside the button

    private UmetyDatabase.DatabaseManager dbm;

    void Start()
    {
        // Find the DatabaseManager in the scene
        dbm = UmetyDatabase.DatabaseManager.dbm;
        UpdatePanel();
    }

    public void UpdatePanel()
    {
        if (dbm == null) return;

        // Example: Show current score info
        reportText.text = string.Format(
            "Question Attempted - {0}/{1}\nScore Gain - {2}%",
            dbm.dataEntries.Count,
            dbm.totalNumberOfQuestions,
            CalculateScore()
        );

        // Decide button label
        if (dbm.dataEntries.Count < dbm.totalNumberOfQuestions)
        {
            retryButtonText.text = "Try";
        }
        else
        {
            retryButtonText.text = "Retry";
        }
    }

    private int CalculateScore()
    {
        if (dbm.totalNumberOfQuestions == 0) return 0;

        int totalScore = 0;
        foreach (var entry in dbm.dataEntries)
        {
            totalScore += entry.UserScore;
        }

        return Mathf.RoundToInt((float)totalScore / dbm.totalNumberOfQuestions);
    }
}