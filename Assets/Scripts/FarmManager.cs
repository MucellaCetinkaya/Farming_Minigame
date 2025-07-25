using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    [SerializeField] private int _row = 4;
    [SerializeField] private int _column = 3;
    [SerializeField] private float _cellDistance = 1.2f;

    [SerializeField] private GameObject _cell; // Prefab of cell
    [SerializeField] private GameObject _highlightPrefab; // Prefab of highlight mesh

    [SerializeField] private Color _cellDryColor;
    [SerializeField] private Color _cellWetColor;
    [SerializeField] private Color _highlightColor;
    
    [SerializeField] private float _cellSpawnYOffset = 0.02f; // Offset to avoid clipping;
    [SerializeField] private float _cellWetDuration = 10f; // How long a cell remains wet when watered;

    public List<FarmCell> Cells = new List<FarmCell>();

    private FarmCell _currentlySelectedCell = null;
    private GameObject _currentHighlight = null;

    private FarmInteractionState _interactionState = FarmInteractionState.Plant;

    public static FarmManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Update()
    {

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

                FarmCell currentCell = currentCellGameObject.GetComponent<FarmCell>();
                currentCell.SetGridPosition(new Vector2Int(column, row));

                currentCell.SetCellColors(_cellDryColor, _cellWetColor, _highlightColor);

                if (currentCell != null)
                {
                    Cells.Add(currentCell);
                }
            }
        }

        _currentHighlight = Instantiate(_highlightPrefab, transform);
        _currentHighlight.SetActive(false);
    }

    public void WaterCell(Vector2Int gridPosition)
    {
        int index = GetIndex(gridPosition);
        if(index < 0)
        {
            return;
        }
        StartCoroutine(WaterCellCoroutine(index));
    }

    public void WaterCurrentlySelectedCell()
    {
        if (_currentlySelectedCell != null)
        {
            Vector2Int gridPosition = _currentlySelectedCell.GetGridPosition();
            WaterCell(gridPosition);
        }
    }

    private IEnumerator WaterCellCoroutine(int index)
    {
        float timer = 0f;
        Cells[index].WaterCell();

        while( timer < _cellWetDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        Cells[index].DryCell();
    }

    public void PlantCrop(PlantDataSO plantDataSO)
    {
        if (_currentlySelectedCell != null)
        {
            if(GameManager.Instance.GetMoney() < plantDataSO.Cost)
            {
                return;
            } else
            {
                GameManager.Instance.UpdateMoney(-plantDataSO.Cost);
            }


            Vector2Int gridPosition = _currentlySelectedCell.GetGridPosition();
            GameObject plantGO = new GameObject();
            plantGO.AddComponent<Plant>();
            Plant plant = plantGO.GetComponent<Plant>();
            plant.SetPlantData(plantDataSO);
            //plant.SetFarmCell(_currentlySelectedCell);
            _currentlySelectedCell.PlantCrop(plant);

            //CropStat currentCropStat = GameManager.Instance.GetCropStat(plantDataSO);
            //currentCropStat.NewCount += 1;

        }
    }

    public void HarvestCrop()
    {
        if (_currentlySelectedCell != null)
        {
            _currentlySelectedCell.Harvest();
        }
    }

    public void OnCellClicked(FarmCell cell)
    {
        //Debug.Log($"FarmManager received click on cell: {cell.GetGridPosition()}");
        if(_currentlySelectedCell != null)
        {
            _currentlySelectedCell.SetSelected(false);
        }
        _currentlySelectedCell = cell;
        _currentlySelectedCell?.SetSelected(true);

        _currentHighlight.SetActive(true);
        Vector3 cellPosition = cell.transform.position;
        _currentHighlight.transform.position = new Vector3(cellPosition.x, cellPosition.y + _cellSpawnYOffset, cellPosition.z);
    }

    public void ClearSelection()
    {
        if (_currentlySelectedCell != null)
        {
            _currentlySelectedCell.SetSelected(false);
        }
        _currentHighlight.SetActive(false);
    }

    public void SetFarmInteractionState(FarmInteractionState state)
    {
        _interactionState = state;
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

public enum FarmInteractionState
{
    Plant,
    Water,
    Harvest
}
