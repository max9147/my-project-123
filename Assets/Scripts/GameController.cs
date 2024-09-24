using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _gameScreen;
    [SerializeField] private Image _globalFade;
    [SerializeField] private MainMenuController _mainMenuController;

    public void InitializeGame()
    {
        _gameScreen.SetActive(true);
    }

    public void PressHomeButton()
    {
        StartCoroutine(StoppingGame());
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