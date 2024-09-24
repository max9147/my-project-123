using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private Image _globalFade;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private TextMeshProUGUI _levelText;

    private CardLayoutCreator _cardLayoutCreator;
    private CardPrefabController _lastCardController;

    private int _currentLevel;
    private int _currentPairCount;
    private int _currentPairSolved;
    private int _lastCardID;

    private void Awake()
    {
        _cardLayoutCreator = GetComponent<CardLayoutCreator>();

        _currentPairCount = 0;
        _currentPairSolved = 0;
        _lastCardID = -1;
    }

    public void InitializeGame()
    {
        _gameScreen.SetActive(true);

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

        _gameScreen.SetActive(false);
        _mainMenuController.InitializeMenu();

        _globalFade.DOColor(Color.clear, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _globalFade.gameObject.SetActive(false);
    }
}