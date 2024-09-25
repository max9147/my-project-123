using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabController : MonoBehaviour
{
    [SerializeField] private AudioSource _whooshSound;
    [SerializeField] private Image _cardFrontImage;
    [SerializeField] private Image _cardBackImage;
    [SerializeField] private RectTransform _cardGraphics;

    private GameController _gameController;

    private bool _cardIsLocked;
    private int _cardID;

    private void Awake()
    {
        _cardIsLocked = true;
    }

    /// <summary>
    /// Animate card flying to its spot on the grid
    /// </summary>
    public void AnimateSpawn()
    {
        _cardGraphics.anchoredPosition = new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), -Screen.height);
        _cardGraphics.gameObject.SetActive(true);
        _cardGraphics.DOAnchorPos(Vector2.zero, 0.5f).SetEase(Ease.InOutSine);
    }

    public void CloseCard(float _setDelay)
    {
        StartCoroutine(FlippingCard(false, _setDelay));
    }

    public void HideCard()
    {
        if (_cardGraphics.anchoredPosition != Vector2.zero)
            return;

        StartCoroutine(HidingCard());
    }

    public void InitialShow(float _showTime)
    {
        StartCoroutine(InitialShowing(_showTime));
    }

    public void PressCard()
    {
        if (_cardIsLocked)
            return;

        _whooshSound.Play();

        _gameController.SelectCard(_cardID, this);

        StartCoroutine(FlippingCard(true));
    }

    public void SetupCard(int _setID, Sprite _setSprite, GameController _setGameController)
    {
        _cardID = _setID;
        _cardFrontImage.sprite = _setSprite;
        _gameController = _setGameController;
    }

    /// <summary>
    /// Playing flipping animation and changing displayed side
    /// </summary>
    /// <param name="_toFront">Is card being flipped to front side</param>
    /// <param name="_delay">Optional animation delay</param>
    /// <returns></returns>
    private IEnumerator FlippingCard(bool _toFront, float _delay = 0f)
    {
        yield return new WaitForSeconds(_delay);

        if (_toFront)
            _cardIsLocked = true;

        _cardGraphics.DOScaleX(0f, 0.2f).SetEase(Ease.InSine);

        yield return new WaitForSeconds(0.2f);

        _cardBackImage.gameObject.SetActive(!_toFront);
        _cardFrontImage.gameObject.SetActive(_toFront);

        _cardGraphics.DOScaleX(1f, 0.2f).SetEase(Ease.OutSine);

        yield return new WaitForSeconds(0.2f);

        if (!_toFront)
            _cardIsLocked = false;
    }

    /// <summary>
    /// Animate card jumping away from the screen
    /// </summary>
    /// <returns></returns>
    private IEnumerator HidingCard()
    {
        yield return new WaitForSeconds(0.5f);

        _cardGraphics.GetComponent<Image>().raycastTarget = false;

        _cardGraphics.DOJumpAnchorPos(new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), -Screen.height), Screen.height / 2f, 1, 1f);

        yield return new WaitForSeconds(1f);

        _cardGraphics.gameObject.SetActive(false);
    }

    /// <summary>
    /// Animates initial memorizing sequence at the beginning of every level
    /// </summary>
    /// <param name="_showTime"></param>
    /// <returns></returns>
    private IEnumerator InitialShowing(float _showTime)
    {
        StartCoroutine(FlippingCard(true));

        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(_showTime - 0.6f);

        StartCoroutine(FlippingCard(false));
    }
}