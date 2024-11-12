using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class SaveEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    private Button newButton;
    private Button refreshButton;
    private ListView allSavesListView;
    private ListView saveDetailsListView;

    [MenuItem("Window/UI Toolkit/SaveEditor")]
    public static void ShowExample()
    {
        SaveEditor wnd = GetWindow<SaveEditor>();
        wnd.titleContent = new GUIContent("SaveEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        VisualElement labelFromUXML = m_VisualTreeAsset.Instantiate();
        root.Add(labelFromUXML);

        newButton = root.Query<Button>(name: "NewButton").First();
        refreshButton = root.Query<Button>(name: "RefreshButton").First();
        allSavesListView = root.Query<ListView>(name: "AllSavesList").First();
        saveDetailsListView = root.Query<ListView>(name: "SaveDetails").First();

        newButton.clicked += OnNewButtonClicked;
        refreshButton.clicked += OnRefreshButtonClicked;
    }

    public void OnGUI()
    {
        // Debug.Log("OnGUI()");
    }

    private void OnNewButtonClicked()
    {
        SaveManager.TryToMakeNew();
    }

    private void OnRefreshButtonClicked()
    {
        // TODO
    }
}
