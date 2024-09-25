using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private Image _globalFade;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private Sprite _heartSprite;
    [SerializeField] private Sprite _skullSprite;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private TextMeshProUGUI _gameOverText;

    private CardLayoutCreator _cardLayoutCreator;
    private CardPrefabController _lastCardController;
    private ScoringSystem _scoringSystem;

    private int _currentHealth;
    private int _currentLevel;
    private int _currentPairCount;
    private int _currentPairSolved;
    private int _lastCardID;

    private void Awake()
    {
        _cardLayoutCreator = GetComponent<CardLayoutCreator>();
        _scoringSystem = GetComponent<ScoringSystem>();

        _currentHealth = 0;
        _currentPairCount = 0;
        _currentPairSolved = 0;
        _lastCardID = -1;
    }

    public void InitializeGame()
    {
        _gameScreen.SetActive(true);

        _currentHealth = (int)(3 - DataContainer.Instance.CurrentDifficulty) * 2;

        foreach (var _heart in _hearts)
        {
            _heart.gameObject.SetActive(false);
            _heart.sprite = _heartSprite;
        }

        for (int i = 0; i < _currentHealth; i++)
            _hearts[i].gameObject.SetActive(true);

        _currentLevel = 0;

        AdvanceLevel();
    }

    public void PressHomeButton()
    {
        StartCoroutine(StoppingGame());
    }

    public void SelectCard(int _selectedCardID, CardPrefabController _selectedCardController)
    {
        if (_lastCardID == -1)
        {
            _lastCardID = _selectedCardID;
            _lastCardController = _selectedCardController;
        }
        else
        {
            _scoringSystem.CountTurn(_selectedCardID == _lastCardID);

            if (_selectedCardID == _lastCardID)
            {
                _lastCardController.HideCard();
                _selectedCardController.HideCard();

                _currentPairSolved++;

                if (_currentPairSolved == _currentPairCount)
                    Invoke(nameof(AdvanceLevel), 2f);
            }
            else
            {
                _lastCardController.CloseCard(1f);
                _selectedCardController.CloseCard(1f);

                _currentHealth--;
                _hearts[_currentHealth].sprite = _skullSprite;

                if (_currentHealth == 0)
                {
                    DataContainer.Instance.SubmitRecord(_scoringSystem.CurrentScore);

                    StartCoroutine(PlayingGameOver());
                }
            }

            _lastCardID = -1;
        }
    }

    public void SetPairCount(int _setPairCount)
    {
        _currentPairCount = _setPairCount;
    }

    private void AdvanceLevel()
    {
        _currentLevel++;

        StartCoroutine(StartingLevel());
    }

    private IEnumerator PlayingGameOver()
    {
        _cardLayoutCreator.LockCards();

        _gameOverText.text = $"Game over!\nFinal score: {_scoringSystem.CurrentScore}";
        _gameOverText.color = new Color(1f, 1f, 1f, 0f);
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.DOColor(Color.white, 0.5f);

        yield return new WaitForSeconds(2.5f);

        _gameOverText.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f);

        yield return new WaitForSeconds(0.5f);

        _gameOverText.gameObject.SetActive(false);

        StartCoroutine(StoppingGame());
    }

    private IEnumerator StartingLevel()
    {
        _currentPairSolved = 0;

        _cardLayoutCreator.CalculateLayout(_currentLevel);

        _levelText.text = $"Level {_currentLevel}";
        _levelText.color = new Color(1f, 1f, 1f, 0f);
        _levelText.gameObject.SetActive(true);
        _levelText.DOColor(Color.white, 0.5f);

        yield return new WaitForSeconds(1.5f);

        _levelText.DOColor(new Color(1f, 1f, 1f, 0f), 0.5f);

        yield return new WaitForSeconds(0.5f);

        _levelText.gameObject.SetActive(false);
    }

    private IEnumerator StoppingGame()
    {
        _globalFade.color = Color.clear;
        _globalFade.gameObject.SetActive(true);
        _globalFade.DOColor(Color.black, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _scoringSystem.ResetScoring();
        _gameScreen.SetActive(false);
        _mainMenuController.InitializeMenu();

        _globalFade.DOColor(Color.clear, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _globalFade.gameObject.SetActive(false);
    }
}