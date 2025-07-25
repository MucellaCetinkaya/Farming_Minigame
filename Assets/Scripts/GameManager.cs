using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<PlantDataSO> _cropTypes;
    [SerializeField] private int _money = 50;
    [SerializeField] private int _maxMoney = 10000;

    public static GameManager Instance { get; private set; }

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

    public void Initialize()
    {
        UpdateMoney(0);
    }

    public void UpdateMoney(int money)
    {
        _money += money;
        _money = Mathf.Clamp(_money, 0, _maxMoney);
        UIManager.Instance.UpdateMoney(_money);
    }

    public int GetMoney() {  return _money; }

    public List<PlantDataSO> GetCropTypes() {  return _cropTypes; }
}
