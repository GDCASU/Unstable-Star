using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Boss : CombatEntity
{
    public abstract void BeginFight();

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
