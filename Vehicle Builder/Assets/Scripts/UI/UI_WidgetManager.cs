using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_WidgetManager : GenericSingleton<UI_WidgetManager>
{
    public Canvas canvas;
    public List<WidgetPrefab> allWidgetPrefabs;
    
    private Dictionary<string, GameObject> AllWidgets_Dict;
    private Dictionary<string, GameObject> allLoadedWidgets_Dict;

    public void Awake()
    {
        AllWidgets_Dict = new Dictionary<string, GameObject>();
        allLoadedWidgets_Dict = new Dictionary<string, GameObject>();
        
        foreach (WidgetPrefab item in allWidgetPrefabs)
        {
            AllWidgets_Dict.TryAdd(item.key, item.prefab);
        }
    }


    public bool TryLoadWidget(string keyToLoadFrom, string keyToLoadTo)
    {
        if(!AllWidgets_Dict.ContainsKey(keyToLoadFrom))
        {
            Debug.LogWarning("UI_WidgetManager tried to load from a key (" + keyToLoadFrom + ") that does not exist.  Double check the spelling of all keys involved");
            return false;
        }

        GameObject prefab = AllWidgets_Dict[keyToLoadFrom];

        GameObject loaded = Instantiate(prefab);

        if(loaded == null)
        {
            Debug.LogWarning("UI_WidgetManager loaded a widget from the key (" + keyToLoadFrom + ") but the loaded prefab was null.  Double check that there is a prefab to load.  Destorying this prefab");
            Destroy(loaded);
            return false;
        }

        loaded.GetComponent<Widget>().SetID(keyToLoadTo);

        if(!allLoadedWidgets_Dict.TryAdd(keyToLoadTo, loaded))
        {
            Debug.LogWarning("UI_WidgetManager tried to load a new widget with a key (" + keyToLoadTo + ") that already exists.  Please use a different key instead.");
            Destroy(loaded);
            return false;
        }

        loaded.transform.SetParent(canvas.transform, false);

        Widget widget = loaded.GetComponent<Widget>();
        widget.SetUp();


        return true;
    }

    public bool TryLoadWidgetWithArgs(string keyToLoadFrom, string keyToLoadTo, List<string> args)
    {
        if(!AllWidgets_Dict.ContainsKey(keyToLoadFrom))
        {
            Debug.LogWarning("UI_WidgetManager tried to load from a key (" + keyToLoadFrom + ") that does not exist.  Double check the spelling of all keys involved");
            return false;
        }

        GameObject prefab = AllWidgets_Dict[keyToLoadFrom];

        GameObject loaded = Instantiate(prefab);
        loaded.GetComponent<Widget>().SetID(keyToLoadTo);

        if(!allLoadedWidgets_Dict.TryAdd(keyToLoadTo, loaded))
        {
            Debug.LogWarning("UI_WidgetManager tried to load a new widget with a key (" + keyToLoadTo + ") that already exists.  Please use a different key instead.");
            Destroy(loaded);
            return false;
        }

        if(args.Count == 0) Debug.LogWarning("UI-WidgetManager is running TryLoadWidgetWithArgs with zero arguments.  Try to use TryLoadWidget instead");

        loaded.transform.SetParent(canvas.transform, false);


        Widget widget = loaded.GetComponent<Widget>();
        widget.SetUp();
        widget.SendArguments(args);


        return true;
    }

    public bool TryUnloadWidget(string keyToUnload)
    {
        if(!allLoadedWidgets_Dict.ContainsKey(keyToUnload))
        {
            Debug.LogWarning("UI_WidgetManager tried to unload from a key (" + keyToUnload + ") that does not exist.  Double check the spelling of all keys involved");
            return false;
        }

        GameObject loaded = allLoadedWidgets_Dict[keyToUnload];

        Widget widget = loaded.GetComponent<Widget>();
        widget.Teardown();

        Destroy(loaded);

        allLoadedWidgets_Dict.Remove(keyToUnload);

        return true;
    }
}

[System.Serializable]
public class WidgetPrefab
{
    public string key;
    public GameObject prefab;
}
