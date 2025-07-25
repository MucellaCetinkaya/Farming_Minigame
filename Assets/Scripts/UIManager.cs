using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _plantPanel;
    [SerializeField] private GameObject _waterPanel;
    [SerializeField] private GameObject _harvestPanel;

    [SerializeField] private TextMeshProUGUI _moneyText;

    [SerializeField] private PlantDataSO _cornDataSO;
    [SerializeField] private PlantDataSO _tomatoDataSO;
    [SerializeField] private TextMeshProUGUI _cornCostText;
    [SerializeField] private TextMeshProUGUI _cornValueText;
    [SerializeField] private TextMeshProUGUI _tomatoCostText;
    [SerializeField] private TextMeshProUGUI _tomatoValueText;

    [SerializeField] private List<PanelCropStat> _cropStatList;

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
        //_cropStatList = new List<PanelCropStat>();
    }

    private void Start()
    {
        _plantPanel.SetActive(true);
        _waterPanel.SetActive(false);
        _harvestPanel.SetActive(false);
        _activePanel = _plantPanel;

        SetCropCostsAndValues();
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

    public void SendGardenerToWaterCell()
    {
        FarmManager.Instance.SendGardenerToWaterCurrentCell();
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

    public void HarvestCrop()
    {
        FarmManager.Instance.HarvestCrop();
    }

    public void UpdateMoney(int amount)
    {
        _moneyText.text = amount.ToString();
    }

    private void SetCropCostsAndValues()
    {
        _cornCostText.text = _cornDataSO.Cost.ToString();
        _cornValueText.text = _cornDataSO.Value.ToString();
        _tomatoCostText.text = _tomatoDataSO.Cost.ToString();
        _tomatoValueText.text = _tomatoDataSO.Value.ToString();
    }

    public void UpdateCropStats()
    {
        foreach(var cropStat in GameManager.Instance.CropStats)
        {
            foreach(var panelCropStat in _cropStatList)
            {
                if(cropStat.PlantData == panelCropStat.GetPlantDataSO())
                {
                    panelCropStat.UpdatePanel(cropStat);
                }
            }
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
