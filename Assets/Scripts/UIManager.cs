using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _plantPanel;
    [SerializeField] private GameObject _waterPanel;
    [SerializeField] private GameObject _harvestPanel;

    private GameObject _activePanel;
    private PlantDataSO _currentPlantDataSO;

    public static UIManager Instance { get; private set; }

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
        _plantPanel.SetActive(true);
        _waterPanel.SetActive(false);
        _harvestPanel.SetActive(false);
        _activePanel = _plantPanel;
    }

    public void SetFarmInteractionState(FarmInteractionState state)
    {
        FarmManager.Instance.SetFarmInteractionState(state);
        _activePanel.SetActive(false);

        switch (state)
        {
            case FarmInteractionState.Plant: 
                _plantPanel.SetActive(true);
                _activePanel = _plantPanel;
                break;
            case FarmInteractionState.Water:
                _waterPanel.SetActive(true);
                _activePanel = _waterPanel;
                break;
            case FarmInteractionState.Harvest: 
                _harvestPanel.SetActive(true);
                _activePanel = _harvestPanel;
                break;
        }
    }

    public void WaterCell()
    {
        FarmManager.Instance.WaterCurrentlySelectedCell();
    }

    public void SelectCropTypeToBuy(PlantDataSO plantDataSO) {
        _currentPlantDataSO = plantDataSO;
    }

    public void BuyAndPlantCrop()
    {
        if (_currentPlantDataSO != null)
        {
            FarmManager.Instance.PlantCrop(_currentPlantDataSO);
        }
    }

    public void SetFarmInteractionStatePlant()
    {
        SetFarmInteractionState(FarmInteractionState.Plant);
    }
    public void SetFarmInteractionStateWater()
    {
        SetFarmInteractionState(FarmInteractionState.Water);
    }
    public void SetFarmInteractionStateHarvest()
    {
        SetFarmInteractionState(FarmInteractionState.Harvest);
    }
}
