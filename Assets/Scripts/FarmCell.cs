using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class FarmCell : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridPosition; // (column, row)
    [SerializeField] private Plant _currentPlant;
    [SerializeField] private bool _isOccupied;
    [SerializeField] private PlantProgressIcon _progressIcon;
    [SerializeField] private ParticleSystem _particleSystem;

    private Vector3 SpawnOffset = new Vector3(0, 0.01f, 0); // Small offset to avoid crop mesh clipping
    private Renderer _renderer;
    private Color _dryColor;
    private Color _wetColor;
    private Color _highlightColor;

    private bool _isSelected = false;

    private void Start()
    {
        _isOccupied = false;
        _currentPlant = null;
        _renderer = GetComponent<Renderer>();
        _progressIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(_isOccupied && _currentPlant != null)
        {
            _progressIcon.SetMeterValue(_currentPlant.GetStateTimerNormalized());
        }
    }

    public bool PlantCrop(Plant plant)
    {
        if(_isOccupied) return false;

        _currentPlant = plant;
        _currentPlant.transform.position = transform.position + SpawnOffset;
        _currentPlant.transform.SetParent(transform);

        _isOccupied = true;

        _progressIcon.gameObject.SetActive(true);
        _progressIcon.SetPlantState(PlantState.New);
        _progressIcon.SetMeterValue(0f);

        return true;
    }

    public void WaterCell()
    {
        _renderer.material.color = _wetColor;
        if (_isOccupied && _currentPlant != null)
        {
            _currentPlant.SetWateredState(true);
        }
    }

    public void DryCell()
    {
        _renderer.material.color = _dryColor;
        if (_isOccupied && _currentPlant != null)
        {
            _currentPlant.SetWateredState(false);
        }
    }

    public bool CanHarvest()
    {
        bool result = false;

        if(_currentPlant != null && _currentPlant.CanHarvest()) {
            result = true;
        }

        return result;
    }

    public void Harvest()
    {
        if(CanHarvest())
        {
            GameManager.Instance.RemoveDoneCrop(_currentPlant.GetPlantData());
            UIManager.Instance.UpdateCropStats();


            _currentPlant.HarvestPlant();
            Destroy(_currentPlant);
            _isOccupied = false;
            _progressIcon.gameObject.SetActive(false);
        }
    }

    public void SetCellColors(Color dryColor, Color wetColor, Color highlightColor)
    {
        _dryColor = dryColor;
        _wetColor = wetColor;
        _highlightColor = highlightColor;
    }

    public void SetGridPosition(Vector2Int gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public Vector2Int GetGridPosition()
    {
        return _gridPosition;
    }
    
    public void HighlightCell()
    {
        _renderer.material.color = _highlightColor;
    }

    public void OnClicked()
    {
        FarmManager.Instance.OnCellClicked(this);
    }

    public void SetSelected(bool selected)
    {
        _isSelected = selected;
        

        if(selected)
        {
            Debug.Log($"Cell {_gridPosition} is selected");
            //HighlightCell();
        } else
        {
            Debug.Log($"Cell {_gridPosition} is unselected");
            //DryCell();
        }
    }

    public void SetProgressIconState(PlantState plantState)
    {
        if(_progressIcon != null)
        {
            _progressIcon.SetPlantState(plantState);
        }
    }

    public void PlayParticleEffect()
    {
        _particleSystem.Play();
    }

    public bool IsSelected()
    {
        return _isSelected;
    }

    public bool IsOccupied()
    {
        return _isOccupied;
    }
}
