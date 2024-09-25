using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MenuContainer
{
    Settings,
    Leaderboard,
}

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameController _gameController;
    [SerializeField] private GameObject _mainMenuScreen;
    [SerializeField] private Image _menuFade;
    [SerializeField] private Image _globalFade;
    [SerializeField] private LeaderboardEntryController _leaderboardEntryPrefab;
    [SerializeField] private RectTransform _settingsContainer;
    [SerializeField] private RectTransform _leaderboardContainer;
    [SerializeField] private RectTransform _leaderboardContent;
    [SerializeField] private Slider _difficultySlider;
    [SerializeField] private TextMeshProUGUI _toggleSoundButtonText;

    private bool _isChangingScreen;

    private void Awake()
    {
        _isChangingScreen = false;
    }

    private void Start()
    {
        AudioListener.volume = DataContainer.Instance.CurrentSoundIsOn ? 1f : 0f;
    }

    public void InitializeMenu()
    {
        _mainMenuScreen.SetActive(true);
    }

    public void PressClearDataButton()
    {
        DataContainer.Instance.ClearData();
    }

    public void PressCloseLeaderboardButton()
    {
        if (_isChangingScreen)
            return;

        StartCoroutine(ClosingContainer(MenuContainer.Leaderboard));
    }

    public void PressCloseSettingsButton()
    {
        if (_isChangingScreen)
            return;

        if ((Difficulty)_difficultySlider.value != DataContainer.Instance.CurrentDifficulty)
            DataContainer.Instance.ChangeDifficulty((Difficulty)_difficultySlider.value);

        StartCoroutine(ClosingContainer(MenuContainer.Settings));
    }

    public void PressOpenLeaderboardButton()
    {
        if (_isChangingScreen)
            return;

        foreach (Transform _child in _leaderboardContent.transform)
            Destroy(_child.gameObject);

        _leaderboardContent.sizeDelta = new Vector2(_leaderboardContent.sizeDelta.x, DataContainer.Instance.CurrentRecords.Count * 100f + (DataContainer.Instance.CurrentRecords.Count - 1) * 20f);

        for (int i = 0; i < DataContainer.Instance.CurrentRecords.Count; i++)
            Instantiate(_leaderboardEntryPrefab, _leaderboardContent).SetupLeaderboardEntry(i + 1, DataContainer.Instance.CurrentRecords[i].Score, DataContainer.Instance.CurrentRecords[i].Difficulty);

        StartCoroutine(OpeningContainer(MenuContainer.Leaderboard));
    }

    public void PressOpenSettingsButton()
    {
        if (_isChangingScreen)
            return;

        _difficultySlider.value = (int)DataContainer.Instance.CurrentDifficulty;
        _toggleSoundButtonText.text = DataContainer.Instance.CurrentSoundIsOn ? "Mute sound" : "Unmute sound";

        StartCoroutine(OpeningContainer(MenuContainer.Settings));
    }

    public void PressPlayButton()
    {
        StartCoroutine(StartingGame());
    }

    public void PressQuit()
    {
        Application.Quit();
    }

    public void PressToggleSoundButton()
    {
        bool _turnedOn = DataContainer.Instance.ToggleSound();

        _toggleSoundButtonText.text = _turnedOn ? "Mute sound" : "Unmute sound";
        AudioListener.volume = _turnedOn ? 1f : 0f;
    }

    private IEnumerator OpeningContainer(MenuContainer _setMenuContainer)
    {
        _isChangingScreen = true;

        switch (_setMenuContainer)
        {
            case MenuContainer.Settings:
                _settingsContainer.anchoredPosition = new Vector2(-Screen.width / 2f - _settingsContainer.rect.width, 0f);
                _settingsContainer.gameObject.SetActive(true);
                _settingsContainer.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutSine);
                break;
            case MenuContainer.Leaderboard:
                _leaderboardContainer.anchoredPosition = new Vector2(-Screen.width / 2f - _leaderboardContainer.rect.width, 0f);
                _leaderboardContainer.gameObject.SetActive(true);
                _leaderboardContainer.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutSine);
                break;
            default:
                break;
        }

        _menuFade.color = Color.clear;
        _menuFade.gameObject.SetActive(true);
        _menuFade.DOColor(new Color(0f, 0f, 0f, 0.8f), 0.5f);

        yield return new WaitForSeconds(0.5f);

        _isChangingScreen = false;
    }

    private IEnumerator ClosingContainer(MenuContainer _setMenuContainer)
    {
        _isChangingScreen = true;

        switch (_setMenuContainer)
        {
            case MenuContainer.Settings:
                _settingsContainer.DOAnchorPos(new Vector2(Screen.width / 2f + _settingsContainer.rect.width, 0f), 0.5f).SetEase(Ease.InOutSine);
                break;
            case MenuContainer.Leaderboard:
                _leaderboardContainer.DOAnchorPos(new Vector2(Screen.width / 2f + _settingsContainer.rect.width, 0f), 0.5f).SetEase(Ease.InOutSine);
                break;
            default:
                break;
        }

        _menuFade.DOColor(Color.clear, 0.5f);

        yield return new WaitForSeconds(0.5f);

        switch (_setMenuContainer)
        {
            case MenuContainer.Settings:
                _settingsContainer.gameObject.SetActive(false);
                break;
            case MenuContainer.Leaderboard:
                _leaderboardContainer.gameObject.SetActive(false);
                break;
            default:
                break;
        }

        _menuFade.gameObject.SetActive(false);

        _isChangingScreen = false;
    }

    private IEnumerator StartingGame()
    {
        _globalFade.color = Color.clear;
        _globalFade.gameObject.SetActive(true);
        _globalFade.DOColor(Color.black, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _mainMenuScreen.SetActive(false);
        _gameController.InitializeGame();

        _globalFade.DOColor(Color.clear, 0.5f);

        yield return new WaitForSeconds(0.5f);

        _globalFade.gameObject.SetActive(false);
    }
}