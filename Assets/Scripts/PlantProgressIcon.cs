using UnityEngine;

public class PlantProgressIcon : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;
    [SerializeField] private Material _material;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        SetPlantState(PlantState.New);
    }

    public void SetPlantState(PlantState plantState)
    {
        if(_material == null)
        {
            Debug.Log("No material found");
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
            return;
        }
        _material.SetFloat("_Plant_State", (float)plantState);
    }

    public void SetMeterValue(float meterValue)
    {
        if (_material == null)
        {
            Debug.Log("No material found");
            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
            return;
        }
        _material.SetFloat("_Meter_Value", Mathf.Clamp01(meterValue));
    }
}
