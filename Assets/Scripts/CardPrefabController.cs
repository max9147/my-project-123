using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardPrefabController : MonoBehaviour
{
    [SerializeField] private Image _cardFrontImage;
    [SerializeField] private Image _cardBackImage;
    [SerializeField] private RectTransform _cardGraphics;

    private int _cardID;

    public void AnimateSpawn()
    {
        _cardGraphics.anchoredPosition = new Vector2(Random.Range(-Screen.width / 2, Screen.width / 2), -Screen.height);
        _cardGraphics.gameObject.SetActive(true);
        _cardGraphics.DOAnchorPos(Vector2.zero, 0.5f);
    }

    public void SetupCard(int _setID, Sprite _setSprite)
    {
        _cardID = _setID;
        _cardFrontImage.sprite = _setSprite;
    }
}