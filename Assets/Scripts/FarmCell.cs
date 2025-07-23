using UnityEngine;

public class FarmCell : MonoBehaviour
{
    public Vector2Int GridPosition {  get; private set; }
    public Plant CurrentPlant { get; private set; }
    public bool IsOccupied { get; private set; }

    private Vector3 SpawnOffset = new Vector3(0, 0.02f, 0); // Small offset to avoid clipping
    private Renderer _renderer;
    private Color _dryColor;
    private Color _wetColor;

    public FarmCell(Vector2Int gridPosition)
    {
        GridPosition = gridPosition;
    }
    
    private void Start()
    {
        IsOccupied = false;
        CurrentPlant = null;
        _renderer = GetComponent<Renderer>();
    }

    public bool PlantCrop(Plant plant)
    {
        if(IsOccupied) return false;

        CurrentPlant = plant;
        CurrentPlant.transform.position = transform.position + SpawnOffset;
        CurrentPlant.transform.SetParent(transform);

        IsOccupied = true;
        return true;
    }

    public void WaterCell()
    {
        if (IsOccupied && CurrentPlant != null)
        {
            CurrentPlant.SetWateredState(true);
            _renderer.material.color =_wetColor;
        }
    }
}
