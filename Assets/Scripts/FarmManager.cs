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

    public Vector2Int TestWaterCell = new Vector2Int(0,0);

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log("Key pressed");
            WaterCell(TestWaterCell);
        }
    }

    private void Initialize()
    {
        float totalWidth = _column * _cellDistance;
        float totalHeight = _row * _cellDistance;

        for (int row = 0; row < _row; row++)
        {
            for (int column = 0; column < _column; column++)
            {
                float x = (column * _cellDistance) - (totalWidth / 2f) + (_cellDistance / 2f);
                float z = (row * _cellDistance) - (totalHeight / 2f) + (_cellDistance / 2f);

                Vector3 cellPosition = new Vector3(
                    transform.position.x + x,
                    transform.position.y + _cellSpawnYOffset,
                    transform.position.z + z
                );

                GameObject currentCellGameObject = Instantiate(_cell, cellPosition, transform.rotation, transform);

                FarmCell currentCell = currentCellGameObject.AddComponent<FarmCell>();
                currentCell.SetGridPosition(new Vector2Int(column, row)); // column is X, row is Y

                currentCell.SetCellColors(_cellDryColor, _cellWetColor);

                if (currentCell != null)
                {
                    Cells.Add(currentCell);
                }
            }
        }
    }

    public void WaterCell(Vector2Int gridPosition)
    {
        int index = GetIndex(gridPosition);
        if(index < 0)
        {
            return;
        }

        Cells[GetIndex(gridPosition)].WaterCell();
    }

    private int GetIndex(Vector2Int gridPosition)
    {
        int x = gridPosition.x;
        int y = gridPosition.y;

        if (x < 0 || y < 0 || x >= _column || y >= _row)
        {
            Debug.Log("Grid Position out of bounds.");
            return -1;
        }
        return x + y * _column;
    }
}
