using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    public int CurrentScore => _currentScore;

    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _turnsText;
    [SerializeField] private RectTransform _addedScorePrefab;
    [SerializeField] private RectTransform _statsContainer;

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
            StartCoroutine(ShowingAddedScore(_currentCombo));

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

    private IEnumerator ShowingAddedScore(int _setScore)
    {
        RectTransform _currentAddedScore = Instantiate(_addedScorePrefab, _statsContainer);
        _currentAddedScore.GetComponent<TextMeshProUGUI>().text = $"+{_setScore}";
        _currentAddedScore.anchoredPosition = _scoreText.GetComponent<RectTransform>().anchoredPosition + new Vector2(100f, 0f);
        _currentAddedScore.DOAnchorPosY(_currentAddedScore.anchoredPosition.y + 100f, 0.5f);

        yield return new WaitForSeconds(0.25f);

        _currentAddedScore.GetComponent<TextMeshProUGUI>().DOColor(new Color(0f, 1f, 0f, 0f), 0.25f);

        yield return new WaitForSeconds(0.25f);

        Destroy(_currentAddedScore.gameObject);
    }
}