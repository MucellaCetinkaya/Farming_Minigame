using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlantDataSO> _cropTypes;
    [SerializeField] private int _money = 50;
    [SerializeField] private int _maxMoney = 10000;

    public List<CropStat> CropStats;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        CropStats = new List<CropStat>();
    }

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        UpdateMoney(0);

        foreach (var cropType in _cropTypes)
        {
            CropStat cropStat = new CropStat();
            cropStat.PlantData = cropType;
            cropStat.NewCount = 0;
            cropStat.HalfDoneCount = 0;
            cropStat.DoneCount = 0;
            CropStats.Add(cropStat);
        }
    }

    public void UpdateMoney(int money)
    {
        _money += money;
        _money = Mathf.Clamp(_money, 0, _maxMoney);
        UIManager.Instance.UpdateMoney(_money);
    }

    public int GetMoney() {  return _money; }

    public List<PlantDataSO> GetCropTypes() {  return _cropTypes; }

    public CropStat GetCropStat(PlantDataSO plantData)
    {
        CropStat result = CropStats[0];

        foreach(var stat in CropStats)
        {
            if(stat.PlantData == plantData) result = stat;
        }

        return result;
    }

    public void AddNewCrop(PlantDataSO plantData)
    {
        for (int i=0; i< CropStats.Count;i++)
        {
            if(CropStats[i].PlantData == plantData)
            {
                CropStat updatedStat = CropStats[i];
                updatedStat.NewCount += 1;
                CropStats[i] = updatedStat;
                break;
            }
        }
    }

    public void AddHalfDoneCrop(PlantDataSO plantData)
    {
        for (int i = 0; i < CropStats.Count; i++)
        {
            if (CropStats[i].PlantData == plantData)
            {
                CropStat updatedStat = CropStats[i];
                updatedStat.NewCount -= 1;
                updatedStat.HalfDoneCount += 1;
                CropStats[i] = updatedStat;
                break;
            }
        }
    }

    public void AddDoneCrop(PlantDataSO plantData)
    {
        for (int i = 0; i < CropStats.Count; i++)
        {
            if (CropStats[i].PlantData == plantData)
            {
                CropStat updatedStat = CropStats[i];
                updatedStat.HalfDoneCount -= 1;
                updatedStat.DoneCount += 1;
                CropStats[i] = updatedStat;
                break;
            }
        }
    }

    public void RemoveDoneCrop(PlantDataSO plantData)
    {
        for (int i = 0; i < CropStats.Count; i++)
        {
            if (CropStats[i].PlantData == plantData)
            {
                CropStat updatedStat = CropStats[i];
                updatedStat.DoneCount -= 1;
                CropStats[i] = updatedStat;
                break;
            }
        }
    }
}

public struct CropStat
{
    public PlantDataSO PlantData;
    public int NewCount;
    public int HalfDoneCount;
    public int DoneCount;
}
