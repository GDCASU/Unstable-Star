using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectivePanel : MonoBehaviour
{
    [SerializeField] private GameObject objectivePrefab;
    [SerializeField] private Transform objectiveContainer;
    private ObjectiveData[] objectiveData;
    private Objective[] objectives;

    void Start()
    {
        // Test code
        // In the future, pull objective data from the level data
        objectiveData = new ObjectiveData[]
        {
            new ObjectiveData()
            {
                title = "Kill enemies",
                type = ObjectiveType.KILLS,
                kills = 25
            },
            new ObjectiveData()
            {
                title = "Kill basic enemies",
                type = ObjectiveType.KILLS_SPECIAL,
                enemyType = EnemyType.BASIC,
                kills = 10
            },
            new ObjectiveData()
            {
                title = "Survive the onslaught",
                type = ObjectiveType.SURVIVE,
                time = 30
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
            objectiveGO.SetActive(false);
            objectiveGO.transform.localPosition = new Vector3(0f, -60f * i, 0f);
            objectiveGO.AddComponent<Objective>().data = objectiveData[i];
            objectiveGO.SetActive(true);
		}
	}
}

public enum ObjectiveType
{
    KILLS,
    KILLS_SPECIAL,
    SURVIVE
}

public struct ObjectiveData
{
    public string title;
    public ObjectiveType type;
    public EnemyType enemyType; // Only used for the Kills Special objective type
    public int kills; // Only used for the Kills/Kills Special objective types
    public int time; // Only used for the Survive objective type
}