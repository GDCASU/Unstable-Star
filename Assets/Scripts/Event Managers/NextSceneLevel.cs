using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextSceneLevel : MonoBehaviour
{
    //singleton reference
    public static NextSceneLevel instance;
    
    // Start is called before the first frame update
    private void Start()
    {
        ObjectivePanel.Instance.OnAllObjectivesComplete += afterObjectiveComplete;
    }

    public void BeginningDialogue() {
        if (AsteroidSpawner.Instance.spawnAsteroid) {
            AsteroidSpawner.Instance.StopSpawn();
        }

        if (!WaveManager.instance.waveSpawnStopped) {
            WaveManager.instance.StopWaveSpawn();
        }
    }

    public void callNextScene() {
        if (ScenesManager.currentLevel == 1) {
            ScenesManager.instance.LoadLevel(ScenesManager.currentLevel);
            ScenesManager.currentLevel += 1;
        }

        if (!ScenesManager.instance.CheckLevel(ScenesManager.currentLevel))
            return;

        ScenesManager.instance.LoadLevel(ScenesManager.currentLevel);
        ScenesManager.currentLevel += 1;

        ObjectivePanel.Instance.StartLevelObjecives();

        WaveManager.instance.StartWaveSpawn();

        AsteroidSpawner.Instance.StartSpawn();        
        
    }

    public void afterObjectiveComplete() {
        WaveManager.instance.StopWaveSpawn();

        AsteroidSpawner.Instance.StopSpawn();

        Cutscene.instance.StartDialogue();
        
    }
    
}
