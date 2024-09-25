using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabController : MonoBehaviour
{
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
        StartCoroutine(HidingCard());
    }

    public void InitialShow(float _showTime)
    {
        StartCoroutine(InitialShowing(_showTime));
    }

    public void LockCard()
    {
        _cardIsLocked = true;
    }

    public void PressCard()
    {
        if (_cardIsLocked)
            return;

        _gameController.SelectCard(_cardID, this);

        StartCoroutine(FlippingCard(true));
    }

    public void SetupCard(int _setID, Sprite _setSprite, GameController _setGameController)
    {
        _cardID = _setID;
        _cardFrontImage.sprite = _setSprite;
        _gameController = _setGameController;
    }

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

    private IEnumerator HidingCard()
    {
        yield return new WaitForSeconds(0.5f);

        _cardGraphics.GetComponent<Image>().raycastTarget = false;

        _cardGraphics.DOJumpAnchorPos(new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), -Screen.height), Screen.height / 2f, 1, 1f);
    }

    private IEnumerator InitialShowing(float _showTime)
    {
        StartCoroutine(FlippingCard(true));

        yield return new WaitForSeconds(0.2f);

        yield return new WaitForSeconds(_showTime - 0.6f);

        StartCoroutine(FlippingCard(false));
    }
}