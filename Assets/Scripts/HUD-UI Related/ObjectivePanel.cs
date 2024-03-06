using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePanel : MonoBehaviour
{
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private Transform objectiveContainer;
    private ObjectiveData[] objectiveData;

    void Start()
    {
        // Test code
        // In the future, pull objective data from the level data
        objectiveData = new ObjectiveData[]
        {
            new ObjectiveData()
            {
                title = "Objective 1",
                type = ObjectiveType.KILLS,
                minValue = 0,
                maxValue = 25
			},
            new ObjectiveData()
            {
                title = "Objective 2",
                type = ObjectiveType.KILLS,
                minValue = 0,
                maxValue = 10
            }
        };
        CreateObjectives();
    }

    private void CreateObjectives()
	{
        // Increase height of the panel depending on the number of objectives
        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, 60f * (objectiveData.Length + 1));

        // Instantiate copies of the objective prefab and set their fields
        for(int i = 0; i < objectiveData.Length; i++)
		{
            GameObject objectiveGO = Instantiate(objectivePrefab, objectiveContainer);
            objectiveGO.transform.localPosition = new Vector3(0f, -60f * i, 0f);
            objectiveGO.GetComponent<Objective>().SetupData(objectiveData[i]);
		}
	}
}

public enum ObjectiveType
{
    KILLS
}

public struct ObjectiveData
{
    public string title;
    public ObjectiveType type;
    public int minValue;
    public int maxValue;
}