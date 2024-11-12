using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveInspector : EditorWindow
{
    private VisualElement m_LeftPane;
    private VisualElement m_RightPane;
    
    [MenuItem("Window/UI Toolkit/SaveInspector")]
    public static void ShowExample()
    {
        SaveInspector wnd = GetWindow<SaveInspector>();
        wnd.titleContent = new GUIContent("SaveInspector");
    }

    public void CreateGUI()
    {
        // Get all data
        var allSaveFiles = new List<string>();

        var info = new DirectoryInfo(Application.persistentDataPath);
        var fileInfo = info.GetFiles();
        foreach (var file in fileInfo)
        {
            allSaveFiles.Add(file.Name);
        }

        // Create structure
        VisualElement root = rootVisualElement;
        root.Clear();

        TwoPaneSplitView splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);
        root.Add(splitView);

        m_LeftPane = new VisualElement();
        m_LeftPane.Clear();
        splitView.Add(m_LeftPane);

        m_RightPane = new VisualElement();
        m_RightPane.Clear();
        splitView.Add(m_RightPane);

        if(!EditorApplication.isPlaying)
        {
            var refreashButton = new Button(() => 
                { 
                    CreateGUI();
                }) 
                { 
                    text = "Refreash" 
                };

            var newButton = new Button(() => 
                { 
                    SaveManager.TryToMakeNew(); 
                    CreateGUI();
                }) 
                { 
                    text = "New" 
                };

            m_LeftPane.Add(refreashButton);    
            m_LeftPane.Add(newButton);
        }

        

        ListView leftList = new ListView();
        m_LeftPane.Add(leftList);

        leftList.makeItem = () => new Label();
        leftList.bindItem = (item, index) => { (item as Label).text = allSaveFiles[index]; };
        leftList.itemsSource = allSaveFiles;

        // Add interaction

        // React to the user's selection
        leftList.selectedIndicesChanged += OnSaveSelectionChange;

        
        
        

    }

    private void OnSaveSelectionChange(IEnumerable<int> selectedIndices)
    {
        // Clear all previous content from the pane.
        m_RightPane.Clear();

        // Get the selected sprite and display it.
        var enumerator = selectedIndices.GetEnumerator();
        if (enumerator.MoveNext())
        {
            var index = enumerator.Current;

            if(!EditorApplication.isPlaying)
            {
                // Get save file
                var deleteButton = new Button(() => 
                { 
                    SaveManager.TryDelete(index); 
                    CreateGUI();
                }) 
                { 
                    text = "Delete" 
                };

                var selectButton = new Button(() => 
                { 
                    // TODO: select as active save
                    CreateGUI();
                }) 
                { 
                    text = "Select" 
                };
                
                m_RightPane.Add(selectButton);
                m_RightPane.Add(deleteButton);
            }
        }
    }
}
