using UnityEngine;

public class FarmCell : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridPosition; // (column, row)
    [SerializeField] private Plant _currentPlant;
    [SerializeField] private bool _isOccupied;

    private Vector3 SpawnOffset = new Vector3(0, 0.01f, 0); // Small offset to avoid crop mesh clipping
    private Renderer _renderer;
    private Color _dryColor;
    private Color _wetColor;

    //public FarmCell(Vector2Int gridPosition)
    //{
    //    GridPosition = gridPosition;
    //}
    
    private void Start()
    {
        _isOccupied = false;
        _currentPlant = null;
        _renderer = GetComponent<Renderer>();
    }

    public bool PlantCrop(Plant plant)
    {
        if(_isOccupied) return false;

        _currentPlant = plant;
        _currentPlant.transform.position = transform.position + SpawnOffset;
        _currentPlant.transform.SetParent(transform);

        _isOccupied = true;
        return true;
    }

    public void WaterCell()
    {
        if (_isOccupied && _currentPlant != null)
        {
            _currentPlant.SetWateredState(true);
            _renderer.material.color =_wetColor;
        }
    }

    public void SetCellColors(Color dryColor, Color wetColor)
    {
        _dryColor = dryColor;
        _wetColor = wetColor;
    }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        _gridPosition = gridPosition;
    }
}
