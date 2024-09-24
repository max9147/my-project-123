using UnityEngine;
using UnityEngine.UI;

public class CardLayoutCreator : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private RectTransform _layoutContainer;

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

        for (int i = 0; i < _layoutX * _layoutY; i++)
            Instantiate(_cardPrefab, _layoutContainer);
    }
}