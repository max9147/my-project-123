using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private Image _globalFade;
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private TextMeshProUGUI _levelText;

    private CardLayoutCreator _cardLayoutCreator;

    private int _currentLevel;

    private void Awake()
    {
        _cardLayoutCreator = GetComponent<CardLayoutCreator>();
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

    private void AdvanceLevel()
    {
        _currentLevel++;

        StartCoroutine(StartingLevel());
    }

    private IEnumerator StartingLevel()
    {
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