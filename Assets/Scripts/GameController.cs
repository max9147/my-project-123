using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private Image _errorFlash;
    [SerializeField] private Image _globalFade;
    [SerializeField] private Image[] _hearts;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private RectTransform _heartsContainer;
    [SerializeField] private RectTransform _homeButton;
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

    /// <summary>
    /// Sets player healts according to difficulty and starts the gameplay loop
    /// </summary>
    public void InitializeGame()
    {
        _gameScreen.SetActive(true);

        _currentHealth = (int)(3 - DataContainer.Instance.CurrentDifficulty) * 2;

        _heartsContainer.sizeDelta = new Vector2(_heartsContainer.sizeDelta.x, _currentHealth * 80f + (_currentHealth - 1) * 10f);

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
        StopAllCoroutines();
        _cardLayoutCreator.StopAllCoroutines();

        StartCoroutine(StoppingGame());
    }

    /// <summary>
    /// Processing player's move
    /// </summary>
    /// <param name="_selectedCardID">Card id</param>
    /// <param name="_selectedCardController">Card controller</param>
    public void SelectCard(int _selectedCardID, CardPrefabController _selectedCardController)
    {
        _scoringSystem.IncreaseTurns();

        if (_lastCardID == -1)
        {
            //  First card selected, saving it for comparison

            _lastCardID = _selectedCardID;
            _lastCardController = _selectedCardController;
        }
        else
        {
            // Second card selected. If same as first one add score and remove cards, else remove health and return cards

            _scoringSystem.CountTurn(_selectedCardID == _lastCardID);

            if (_selectedCardID == _lastCardID)
            {
                SoundManager.Instance.PlayCorrectSound();

                _lastCardController.HideCard();
                _selectedCardController.HideCard();

                _currentPairSolved++;

                //  If no cards left, level is completed, advance to next

                if (_currentPairSolved == _currentPairCount)
                    Invoke(nameof(AdvanceLevel), 2f);
            }
            else
            {
                SoundManager.Instance.PlayWrongSound();

                _lastCardController.CloseCard(1f);
                _selectedCardController.CloseCard(1f);

                StartCoroutine(FlashError());

                _currentHealth--;
                _hearts[_currentHealth].sprite = _skullSprite;

                // If no health left, stop game

                if (_currentHealth == 0)
                {
                    DataContainer.Instance.SubmitRecord(_scoringSystem.CurrentScore);

                    PlayGameOver();
                }
            }

            _lastCardID = -1;
        }
    }

    /// <summary>
    /// Sets how many pairs of cards are on current level
    /// </summary>
    /// <param name="_setPairCount"></param>
    public void SetPairCount(int _setPairCount)
    {
        _currentPairCount = _setPairCount;
    }

    private void AdvanceLevel()
    {
        _currentLevel++;

        StartCoroutine(StartingLevel());
    }

    /// <summary>
    /// Stops current game
    /// </summary>
    private void PlayGameOver()
    {
        SoundManager.Instance.PlayFailSound();

        _cardLayoutCreator.HideCards();

        _gameOverText.text = $"Game over!\nFinal score: {_scoringSystem.CurrentScore}";
        _gameOverText.color = new Color(1f, 1f, 1f, 0f);
        _gameOverText.gameObject.SetActive(true);
        _gameOverText.DOColor(Color.white, 0.5f);

        _homeButton.DOPunchScale(Vector3.one * 0.2f, 2f, 0, 1f).SetLoops(-1).SetEase(Ease.InOutSine);
    }

    /// <summary>
    /// Shows red flash on mismatch
    /// </summary>
    /// <returns></returns>
    private IEnumerator FlashError()
    {
        _errorFlash.DOKill();
        _errorFlash.color = new Color(1f, 0f, 0f, 0f);

        _errorFlash.DOColor(new Color(1f, 0f, 0f, 0.5f), 0.25f);

        yield return new WaitForSeconds(0.25f);

        _errorFlash.DOColor(new Color(1f, 0f, 0f, 0f), 0.25f);
    }

    /// <summary>
    /// Launches level layout initialization and shows current level number
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Return to menu with fade in-out animation
    /// </summary>
    /// <returns></returns>
    private IEnumerator StoppingGame()
    {
        _globalFade.color = Color.clear;
        _globalFade.gameObject.SetActive(true);
        _globalFade.DOColor(Color.black, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _gameOverText.color = new Color(1f, 1f, 1f, 0f);
        _gameOverText.gameObject.SetActive(false);

        _homeButton.DOKill();
        _homeButton.localScale = Vector3.one;

        _scoringSystem.ResetScoring();
        _gameScreen.SetActive(false);
        _mainMenuController.InitializeMenu();

        _globalFade.DOColor(Color.clear, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _globalFade.gameObject.SetActive(false);
    }
}