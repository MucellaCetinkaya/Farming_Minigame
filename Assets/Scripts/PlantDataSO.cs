using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "ScriptableObjects/PlantDataScriptableObject", order = 1)]
public class PlantDataSO : ScriptableObject
{
    public string PlantName;
    public float Cost;
    public float Value;
    
    public float NewToHalfDoneDuration;
    public float HalfDoneToDoneDuration;
    public float DoneToDeathDuration;

    public GameObject NewStagePrefab;
    public GameObject HalfDoneStagePrefab;
    public GameObject DoneStagePrefab;
}
