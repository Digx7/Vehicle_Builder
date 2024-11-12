using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

public class SaveManager
{
    // Fields
    
    private const string FILE_NAME = "save";
    private const string FILE_EXTENSION = ".data";
    private static string activeName;
    private static int _activeFileNumber;
    private static Save _loadedSave;

    // Properties
    public static Save LoadedSave
    {
        get
        {
            return _loadedSave;
        }
    }

    public static int ActiveFileNumber
    {
        get
        {
            if(_activeFileNumber == null)
            {
                _activeFileNumber = 0;
            }
            return _activeFileNumber;
        }

        set
        {
            _activeFileNumber = value;
            activeName = FILE_NAME + _activeFileNumber + FILE_EXTENSION;
        }
    }

    // Methods

    public static int NumberOfSaveFiles()
    {
        return GetAllSaveFiles().Length;
    }

    public static void TrySave()
    {
        Save(Path());
    }
    public static void TrySave(int index)
    {
        Save(Path_AtIndex(index));
    }

    public static void TryLoad()
    {
        TryLoad(Path());
    }
    public static void TryLoad(int index)
    {
        TryLoad(Path_AtIndex(index));
    }

    public static void TryToMakeNew()
    {
        MakeNewSave();
    }

    public static void TryDelete()
    {
        Delete(Path());
    }
    public static void TryDelete(int index)
    {
        Delete(Path_AtIndex(index));
    }


    private static FileInfo[] GetAllSaveFiles()
    {
        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        return fileInfo;
    }

    private static string Path()
    {
        if(activeName == null)
        {
            ActiveFileNumber = 0; 
        }
        return Application.persistentDataPath + "/" + activeName;
    }

    private static string Path_AtIndex(int index)
    {
        var fileInfo = GetAllSaveFiles();

        return fileInfo[index].FullName;
    }
    
    private static string Path_ForNewFile()
    {
        // Will look through all existing files and find the smallest non used file number, will then return a file with said number
        
        // Gets all the saved files
        var fileInfo = GetAllSaveFiles();
        
        // This array will be filled with all the already used file numbers
        int[] usedNums = new int[NumberOfSaveFiles()];

        // Fills usedNums with all the used file numbers
        for (int i = 0; i < NumberOfSaveFiles(); i++)
        {
            // Extract the used file number from its name
            string name = fileInfo[i].Name;
            name = name.Replace(FILE_NAME, "");
            name = name.Replace(FILE_EXTENSION, "");
            int usedNum = Convert.ToInt32(name);

            // Add usedNum to the usedNums array
            usedNums[i] = usedNum;
        }

        Array.Sort(usedNums);

        int res = 0;
        for (int i = 0; i < NumberOfSaveFiles(); i++)
        {
            if(usedNums[i] == res) res++;
            if(usedNums[i] > res) break;
        }

        return Application.persistentDataPath + "/" + FILE_NAME + res + FILE_EXTENSION;
    }
    
    private static void Save(string path)
    {
        if(_loadedSave == null) _loadedSave = new Save();
        
        BinaryFormatter formatter = new BinaryFormatter();

        DataContractSerializer serializer = new DataContractSerializer(_loadedSave.GetType());

        FileStream stream = new FileStream(path, FileMode.Create);

        serializer.WriteObject(stream, _loadedSave);
        stream.Close();
    }

    private static Save Load(string path)
    {
        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            DataContractSerializer serializer = new DataContractSerializer(_loadedSave.GetType());

            FileStream stream = new FileStream(path, FileMode.Open);

            _loadedSave = serializer.ReadObject(stream) as Save;
            stream.Close();

            return _loadedSave;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    private static Save TryLoad(string path)
    {
        if(File.Exists(path))
        {
            return Load(path);
        }
        else
        {
            MakeEmptySave(path);
            return Load(path);
        }
    }


    private static void Delete(string path)
    {
        File.Delete(path);
    }

    private static void MakeNewSave()
    {
        // Path_ForNewFile() is expensive so I only wanna run it once
        string path = Path_ForNewFile();
        
        MakeEmptySave(path);
        Save(path);
    }

    // Run when making a new save, fills it with default values we need
    private static void MakeEmptySave(string path)
    {
        _loadedSave = new Save();
        
        GameObject playerSpawnPoint = GameObject.Find("PlayerSpawnPoint");
        Vector3 playerStartingPosition;

        if(playerSpawnPoint != null) playerStartingPosition = playerSpawnPoint.transform.position;
        else playerStartingPosition = new Vector3 (-38.84f, 3.22f, -6.5f);
        
        _loadedSave.TryAdd<Vector3>("PlayerPosition", playerStartingPosition);
    }

}
