using TMPro;
using UnityEngine;

public class PanelCropStat : MonoBehaviour
{
    [SerializeField] private PlantDataSO _plantDataSO;

    [SerializeField] private TextMeshProUGUI _newCount;
    [SerializeField] private TextMeshProUGUI _halfDoneCount;
    [SerializeField] private TextMeshProUGUI _doneCount;

    public PlantDataSO GetPlantDataSO() { return _plantDataSO; }

    public void UpdatePanel(CropStat cropStat)
    {
        _newCount.text = cropStat.NewCount.ToString();
        _halfDoneCount.text = cropStat.HalfDoneCount.ToString();
        _doneCount.text = cropStat.DoneCount.ToString();
    }
}
