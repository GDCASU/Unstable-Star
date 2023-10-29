using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlayerHealth : MonoBehaviour
{
    public int health { get; private set; }
    public int maxHealth { get; private set; }
    public int shield { get; private set; }
    public int maxShield { get; private set; }

    public delegate void OnHealthChangedDelegate();
    public event OnHealthChangedDelegate OnHealthChanged;
    public delegate void OnDeathDelegate();
    public event OnDeathDelegate OnDeath;
}
