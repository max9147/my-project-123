using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public int CurrentScore => _currentScore;

    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _turnsText;

    private int _currentCombo;
    private int _currentScore;
    private int _currentTurns;

    private void Awake()
    {
        ResetScoring();
    }

    public void CountTurn(bool _isCorrect)
    {
        _currentTurns += 2;
        _turnsText.text = _currentTurns.ToString();

        if (_isCorrect)
        {
            _currentScore += _currentCombo;
            _scoreText.text = _currentScore.ToString();
            _currentCombo++;
            _comboText.text = $"{_currentCombo}x";
        }
        else
        {
            _currentCombo = 1;
            _comboText.text = $"{_currentCombo}x";
        }
    }

    public void ResetScoring()
    {
        _currentCombo = 1;
        _currentScore = 0;
        _currentTurns = 0;

        _comboText.text = $"{_currentCombo}x";
        _scoreText.text = _currentScore.ToString();
        _turnsText.text = _currentTurns.ToString();
    }
}