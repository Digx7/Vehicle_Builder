using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FreeRoamMode : GameMode
{
    // Run when the gamemode is loaded
    public override void Setup()
    {
        Vector3 pos = SaveManager.LoadedSave.TryGetValue<Vector3>("PlayerPosition");
        
        SpawnPlayerAt(pos);

        base.Setup();
    }
    
    // Run when the gamemode is unloaded
    public override void TearDown()
    {
        DespawnAllCharacters();

        // UI_WidgetManager.Instance.TryUnloadWidget("MiniMap");
        
        base.TearDown();
    }
}
