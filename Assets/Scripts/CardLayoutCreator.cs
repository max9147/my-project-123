using UnityEngine;
using UnityEngine.UI;

/*public enum CardLayout
{
    2x2,
    3x2,
    4x2,
    4x3,
    4x4,
    5x4,
    6x4,
    6x5,
    6x6,
}*/

public class CardLayoutCreator : MonoBehaviour
{
    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private RectTransform _layoutContainer;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            CalculateLayout(1);
    }

    public void CalculateLayout(int _level)
    {
        int _layoutX = 2;
        int _layoutY = 2;

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

        for (int i = 0; i < _layoutX * _layoutY; i++)
            Instantiate(_cardPrefab, _layoutContainer);
    }
}