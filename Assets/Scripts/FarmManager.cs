using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private int _row = 4;
    [SerializeField] private int _column = 3;

    [SerializeField] private float _cellDistance = 1.2f;

    [SerializeField] private GameObject _cell; //Prefab of cell

    [SerializeField] private Color _cellDryColor;
    [SerializeField] private Color _cellWetColor;
    [SerializeField] private float _cellSpawnYOffset = 0.02f; // Offset to avoid clipping;

    public List<FarmCell> Cells = new List<FarmCell>();

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        float width = _column * _cellDistance;
        float height = _row * _cellDistance;

        for (int i = 0; i < _column; i++)
        {
            for (int j = 0; j < _row; j++)
            {
                float x = (i * _cellDistance) - (width / 2f) + (_cellDistance / 2f);
                float z = (j * _cellDistance) - (height / 2f) + (_cellDistance / 2f);

                Vector3 cellPosition = new Vector3(
                    transform.position.x + x,
                    transform.position.y + _cellSpawnYOffset,
                    transform.position.z + z
                );

                GameObject currentCellGameObject = Instantiate(_cell, cellPosition, transform.rotation, transform);
                FarmCell currentCell = currentCellGameObject.AddComponent<FarmCell>();
                currentCell.SetGridPosition(new Vector2Int(i, j));
                currentCell.SetCellColors(_cellDryColor, _cellWetColor);

                if (currentCell != null)
                {
                    Cells.Add(currentCell);
                }
            }
        }
    }
}
