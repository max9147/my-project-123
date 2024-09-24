using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CardPrefabController : MonoBehaviour
{
    [SerializeField] private Image _cardFrontImage;
    [SerializeField] private Image _cardBackImage;
    [SerializeField] private RectTransform _cardGraphics;

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
        _cardGraphics.DOAnchorPos(Vector2.zero, 0.5f);
    }

    public void InitialShow(float _showTime)
    {
        StartCoroutine(InitialShowing(_showTime));
    }

    public void PressCard()
    {
        if (_cardIsLocked)
            return;

        StartCoroutine(FlippingCard());
    }

    public void SetupCard(int _setID, Sprite _setSprite)
    {
        _cardID = _setID;
        _cardFrontImage.sprite = _setSprite;
    }

    private IEnumerator FlippingCard()
    {
        _cardIsLocked = true;

        _cardGraphics.DOScaleX(0f, 0.2f);

        yield return new WaitForSeconds(0.2f);

        _cardBackImage.gameObject.SetActive(false);
        _cardFrontImage.gameObject.SetActive(true);

        _cardGraphics.DOScaleX(1f, 0.2f);
    }

    private IEnumerator InitialShowing(float _showTime)
    {
        _cardGraphics.DOScaleX(0f, 0.2f);

        yield return new WaitForSeconds(0.2f);

        _cardBackImage.gameObject.SetActive(false);
        _cardFrontImage.gameObject.SetActive(true);

        _cardGraphics.DOScaleX(1f, 0.2f);

        yield return new WaitForSeconds(_showTime - 0.6f);

        _cardGraphics.DOScaleX(0f, 0.2f);

        yield return new WaitForSeconds(0.2f);

        _cardFrontImage.gameObject.SetActive(false);
        _cardBackImage.gameObject.SetActive(true);

        _cardGraphics.DOScaleX(1f, 0.2f);

        yield return new WaitForSeconds(0.2f);

        _cardIsLocked = false;
    }
}