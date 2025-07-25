using UnityEngine;

public class Plant : MonoBehaviour
{
    private PlantDataSO _plantDataSO;
    private FarmCell _farmCell;

    public PlantState State { get; private set; }

    private float _stateTimer = 0;
    private GameObject _currentModel;
    private bool _isWatered = false;

    public Plant(PlantDataSO plantDataSO)
    {
        _plantDataSO = plantDataSO;
    }

    private void Start()
    {
        State = PlantState.New;
        GameManager.Instance.AddNewCrop(_plantDataSO);
        UIManager.Instance.UpdateCropStats();

        UpdateVisual();
        _stateTimer = 0;
        _isWatered = false;
    }

    private void Update()
    {
        _stateTimer += Time.deltaTime;

        if(State == PlantState.New && _stateTimer >= _plantDataSO.NewToHalfDoneDuration)
        {
            SetState(PlantState.HalfDone);
        }
        if(State == PlantState.HalfDone && _stateTimer >= _plantDataSO.HalfDoneToDoneDuration && _isWatered)
        {
            SetState(PlantState.Done);
        }
    }

    public void SetWateredState(bool isWatered)
    {
        _isWatered = isWatered;
    }

    private void SetState(PlantState state)
    {
        if(state == State)
        {
            return;
        }

        State = state;
        _stateTimer = 0;
        UpdateVisual();
        
        if(state== PlantState.HalfDone)
        {
            GameManager.Instance.AddHalfDoneCrop(_plantDataSO);
        } else if(state== PlantState.Done)
        {
            GameManager.Instance.AddDoneCrop(_plantDataSO);
        }

        UIManager.Instance.UpdateCropStats();
        if(_farmCell != null)
        {
            _farmCell.PlayParticleEffect();
        }
    }

    private void UpdateVisual()
    {
        if(_currentModel != null)
        {
            Destroy(_currentModel);
        }

        GameObject newModel = null;
        switch (State)
        {
            case PlantState.New:
                newModel = _plantDataSO.NewStagePrefab;
                break;
            case PlantState.HalfDone:
                newModel = _plantDataSO.HalfDoneStagePrefab; 
                break;
            case PlantState.Done:
                newModel = _plantDataSO.DoneStagePrefab;
                break;
        }

        if(newModel != null)
        {
            _currentModel = Instantiate(newModel, transform.position, transform.rotation);
            _currentModel.transform.SetParent(transform);
        }
    }
    
    public void SetPlantData(PlantDataSO plantData)
    {
        _plantDataSO = plantData;
    }

    public PlantDataSO GetPlantData()
    {
        return _plantDataSO;
    }

    public float GetStateTimerNormalized()
    {
        float stateDuration = 0.0f;

        switch (State)
        {
            case PlantState.New:
                stateDuration = _plantDataSO.NewToHalfDoneDuration;
                break;
             case PlantState.HalfDone:
                stateDuration = _plantDataSO.HalfDoneToDoneDuration;
                break;
             case PlantState.Done:
                stateDuration = _plantDataSO.DoneToDeathDuration;
                break;
        }

        float normalizedVal = _stateTimer/ stateDuration;
        
        return Mathf.Clamp01(normalizedVal);
    }

    public void SetFarmCell(FarmCell farmCell)
    {
            _farmCell = farmCell;
    }

    public bool CanHarvest()
    {
        bool result = false;

        if(_currentModel != null && 
            State == PlantState.Done) {
        result = true;
        }

        return result;
    }

    public void HarvestPlant()
    {
        if (_currentModel != null)
        {
            Destroy(_currentModel);
            GameManager.Instance.UpdateMoney(_plantDataSO.Value);
        }
    }
}

public enum PlantState
{
    New,
    HalfDone,
    Done
}
