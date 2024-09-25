using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLayoutCreator : MonoBehaviour
{
    [SerializeField] private CardPrefabController _cardPrefabController;
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private RectTransform _layoutContainer;
    [SerializeField] private Slider _initialShowSlider;
    [SerializeField] private Sprite[] _cardSprites;

    private GameController _gameController;
    private List<CardPrefabController> _spawnedCards;

    private void Awake()
    {
        _gameController = GetComponent<GameController>();
        _spawnedCards = new List<CardPrefabController>();
    }

    public void CalculateLayout(int _level)
    {
        int _layoutX = 2;
        int _layoutY = 2;

        int _difficultyScore = Mathf.FloorToInt(_level * 0.3f * ((int)DataContainer.Instance.CurrentDifficulty / 2f + 1f));

        for (int i = 0; i < _difficultyScore; i++)
        {
            if (_layoutY < _layoutX && _layoutX * (_layoutY + 1) % 2 == 0)
                _layoutY++;
            else
                _layoutX++;
        }

        _layoutX = Mathf.Clamp(_layoutX, 2, 6);
        _layoutY = Mathf.Clamp(_layoutY, 2, 6);

        Constructlayout(_layoutX, _layoutY);
    }

    public void LockCards()
    {
        foreach (var _card in _spawnedCards)
            _card.LockCard();
    }

    private void Constructlayout(int _layoutX, int _layoutY)
    {
        _gridLayout.constraintCount = _layoutX;

        float _currentCellSize = 0f;

        if (_layoutX >= _layoutY)
            _currentCellSize = (_layoutContainer.sizeDelta.x - _layoutX * _gridLayout.spacing.x) / _layoutX;
        else
            _currentCellSize = (_layoutContainer.sizeDelta.y - _layoutY * _gridLayout.spacing.y) / _layoutY;

        _gridLayout.cellSize = new Vector2(_currentCellSize, _currentCellSize);

        foreach (Transform _child in _layoutContainer.transform)
            Destroy(_child.gameObject);

        foreach (var _spawnedCard in _spawnedCards)
            Destroy(_spawnedCard.gameObject);

        _spawnedCards.Clear();

        List<int> _availableCards = new List<int>();

        for (int i = 0; i < _layoutX * _layoutY / 2; i++)
        {
            _availableCards.Add(i);
            _availableCards.Add(i);
        }

        for (int i = 0; i < _layoutX * _layoutY; i++)
        {
            CardPrefabController _currentCard = Instantiate(_cardPrefabController, _layoutContainer);
            _spawnedCards.Add(_currentCard);

            int _randomAvailableIndex = Random.Range(0, _availableCards.Count);

            _currentCard.SetupCard(_availableCards[_randomAvailableIndex], _cardSprites[_availableCards[_randomAvailableIndex]], _gameController);

            _availableCards.RemoveAt(_randomAvailableIndex);
        }

        _gameController.SetPairCount(_layoutX * _layoutY / 2);

        StartCoroutine(AnimateCardsSpawn());
    }

    private IEnumerator AnimateCardsSpawn()
    {
        _initialShowSlider.DOKill();
        _initialShowSlider.value = 0f;

        for (int i = 0; i < _spawnedCards.Count; i++)
        {
            _spawnedCards[i].AnimateSpawn();

            yield return new WaitForSeconds(2f / _spawnedCards.Count);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (var _spawnedCard in _spawnedCards)
        {
            if (_spawnedCard.isActiveAndEnabled)
                _spawnedCard.InitialShow(5 - (int)DataContainer.Instance.CurrentDifficulty);
        }

        _initialShowSlider.value = 1f;

        _initialShowSlider.DOValue(0f, 5 - (int)DataContainer.Instance.CurrentDifficulty).SetEase(Ease.Linear);
    }
}