using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectivePanel : MonoBehaviour
{
    // Singleton
    public static ObjectivePanel Instance;
    // Events
    public event System.Action OnAllObjectivesComplete;

    // Serialized Variables
    [SerializeField] private ObjectivePrefabContainer[] objectivePrefabs;
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private ObjectiveData[] objectiveData;
    [SerializeField] private bool testOnStart = false;
    [SerializeField] private bool debug = false;

    // Private Variables
    private RectTransform rectTransform;
    private List<Objective> objectives;

    public void RaiseAllObjectivesComplete()
    {
        if (debug) Debug.Log("All objectives complete");
        OnAllObjectivesComplete?.Invoke();
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
            Instance = this;
    }

    private void Start()
    {
        if (testOnStart) 
            StartLevelObjecives(); // TESTING CALL; DELETE IN FINAL GAME
    }

    /// <summary>
    /// Trigger to start the objectives in the level
    /// </summary>
    public void StartLevelObjecives()
    {
        rectTransform = GetComponent<RectTransform>();

        CreateObjectives();
        if(movePanelCoro != null) StopCoroutine(movePanelCoro);
        movePanelCoro = StartCoroutine(MovePanel(true, delay: 3f));
    }

    private void CreateObjectives()
	{
        objectives = new List<Objective>();

        // Increase height of the panel depending on the number of objectives
        float sizeY = 60f;
        foreach(ObjectiveData data in objectiveData)
		{
            sizeY += data.type == ObjectiveType.KILLS_TIMED ? 80f : 60f;
		}
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sizeY);

        // Instantiate copies of the objective prefab and set their fields
        for(int i = 0; i < objectiveData.Length; i++)
		{
            foreach(ObjectivePrefabContainer prefabContainer in objectivePrefabs)
			{
                if(prefabContainer.type == objectiveData[i].type)
				{
                    GameObject objectiveGO = Instantiate(prefabContainer.prefab, objectiveContainer);
                    objectiveGO.SetActive(false);
                    objectiveGO.transform.localPosition = new Vector3(0f, -60f * i, 0f);

                    Objective objectiveComp = objectiveGO.GetComponent<Objective>();
                    objectiveComp.data = objectiveData[i];
                    objectiveComp.OnObjectiveComplete += OnObjectiveComplete;
                    objectives.Add(objectiveComp);

                    objectiveGO.SetActive(true);
                    break;
                }
			}
        }
	}

	private void OnObjectiveComplete(Objective obj)
	{
		objectives.Remove(obj);
        obj.OnObjectiveComplete -= OnObjectiveComplete;
        if(objectives.Count == 0)
		{
            RaiseAllObjectivesComplete();
        }
	}

    private Vector3 lowPos = new Vector3(-10f, -10f);
    private Vector3 highPos = new Vector3(-10f, 170f);
    float posOffset = 10f;
    private Coroutine movePanelCoro;

    private IEnumerator MovePanel(bool up, float time = 0.5f, float delay = 0f)
	{
        float currentY = rectTransform.anchoredPosition.y;
        float percentY = (currentY + posOffset) / (highPos.y + posOffset);
        if(!up) percentY = 1f - percentY;
        float t = percentY * time;

        if (debug) Debug.Log("ObjectivePanel::MovePanel" +
            "\nt is " + t);
        
        yield return new WaitForSecondsRealtime(delay);

		while(t < 1f)
        {
            rectTransform.anchoredPosition = Vector3.Lerp(up ? lowPos : highPos, up ? highPos : lowPos, t / time);
            t += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    private bool prevPausedGame;
    private void Update()
	{
        if(PauseMenu.pausedGame != prevPausedGame)
		{
            if(movePanelCoro != null) StopCoroutine(movePanelCoro);
            movePanelCoro = StartCoroutine(MovePanel(!PauseMenu.pausedGame, 0.2f));
            prevPausedGame = PauseMenu.pausedGame;
		}
	}
}

public enum ObjectiveType
{
    KILLS,
    KILLS_SPECIAL,
    KILLS_TIMED,
    SURVIVE
}

[Serializable]
public struct ObjectiveData
{
    public string title;
    public ObjectiveType type;
    public EnemyType enemyType; // Only used for the Kills Special objective type
    public int kills; // Only used for the Kills/Kills Special/Kills Timed objective types
    public int time; // Only used for the Survive and Kills Timed objective type
}

[Serializable]
public struct ObjectivePrefabContainer
{
    public ObjectiveType type;
    public GameObject prefab;
}