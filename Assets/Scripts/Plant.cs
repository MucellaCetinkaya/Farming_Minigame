using UnityEngine;

public class Plant : MonoBehaviour
{
    public PlantDataSO PlantDataSO;

    public PlantState State { get; private set; }

    private float _stateTimer = 0;
    private GameObject _currentModel;
    private bool _isWatered = false;

    public Plant(PlantDataSO plantDataSO)
    {
        PlantDataSO = plantDataSO;
    }

    private void Start()
    {
        State = PlantState.New;
        UpdateVisual();
        _stateTimer = 0;
        _isWatered = false;
    }

    private void Update()
    {
        _stateTimer += Time.deltaTime;

        if(State == PlantState.New && _stateTimer >= PlantDataSO.NewToHalfDoneDuration)
        {
            SetState(PlantState.HalfDone);
        }
    }

    public void SetWateredState(bool isWatered)
    {
        _isWatered = isWatered;
    }

    private void SetState(PlantState state)
    {
        State = state;
        _stateTimer = 0;
        UpdateVisual();
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
                newModel = PlantDataSO.NewStagePrefab;
                break;
            case PlantState.HalfDone:
                newModel = PlantDataSO.HalfDoneStagePrefab; 
                break;
            case PlantState.Done:
                newModel = PlantDataSO.DoneStagePrefab;
                break;
        }

        if(newModel != null)
        {
            _currentModel = Instantiate(newModel, transform.position, transform.rotation);
        }
    }
}

public enum PlantState
{
    New,
    HalfDone,
    Done
}
