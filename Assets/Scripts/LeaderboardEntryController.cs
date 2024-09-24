using TMPro;
using UnityEngine;

public class LeaderboardEntryController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _postionText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _difficultyText;

    public void SetupLeaderboardEntry(int _setPosition, int _setScore, Difficulty _setDifficulty)
    {
        _postionText.text = _setPosition.ToString();
        _scoreText.text = _setScore.ToString();
        _difficultyText.text = _setDifficulty.ToString();
    }
}