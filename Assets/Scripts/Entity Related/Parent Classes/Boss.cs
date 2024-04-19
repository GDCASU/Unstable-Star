using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : CombatEntity
{
    protected override void TriggerDeath()
    {
        throw new System.NotImplementedException();
    }

    protected override void WhenPlayerDies()
    {
        throw new System.NotImplementedException();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Cutscene.instance.StartDialogue();
    }
}
