using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistance
{
    /// <summary>
    /// Load data from file
    /// </summary>
    void LoadData();

    /// <summary>
    /// Save Data to file
    void SaveData();
}
