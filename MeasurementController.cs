
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARSubsystems;
 
[RequireComponent(typeof(ARRaycastManager))]
public class MeasurementController : MonoBehaviour
    
{
    private bool isSaved;
    [SerializeField]
    private InputField inputName;

    [SerializeField]
    private GameObject distancesPanel;

    [SerializeField]
    private Transform distancesContainer;

    [SerializeField]
    private GameObject distanceItem;

    private List<string> distancesList;
    private GameObject textToAdd;
    private SaveData myData;

    [SerializeField]
    private GameObject measurementPointPrefab;

    [SerializeField]
    private Button startButton;

    private string guiText;
    int n;
   
    [SerializeField]
    private ARCameraManager arCameraManager;

    private LineRenderer measureLine;
    private ARRaycastManager arRaycastManager;
    private Vector2 touchPosition = default;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private ARPlane plane;
    
    private UnityEngine.XR.ARSubsystems.TrackableId trackableId;
    private List<GameObject> _augmentations = new List<GameObject>();
    [SerializeField]
    GameObject panel;

    private MyButtons buttons;
    private bool isCmClicked;
    private bool isMClicked;
    private bool isSaveClicked;

    private bool isReady;
    private int cmOrm = 1;
    private string cmM;

    void Awake()
    {
        distancesPanel.SetActive(false);
        distancesList = new List<string>();

        arRaycastManager = GetComponent<ARRaycastManager>();
        measureLine = GetComponent<LineRenderer>();
 
        myData = new SaveData();
    }

    private void Start()
    {
        measureLine.enabled = false;
    }

    private void OnEnable()
    {
        if (measurementPointPrefab == null)
        {
            enabled = false;
        }  
    }
 
    public void Update()
    { 
        if (panel.gameObject.activeInHierarchy)
        {
            destroyLines();
            measurementPointPrefab.SetActive(false);           
        }
        else
        {         
            measurementPointPrefab.SetActive(true);
            if (isReady)
            {               
                SetPoints();
            }
        }
    }

    public void CmOnClick()
    {
        isCmClicked = true;
    }
    public void MOnClick()
    {
        isMClicked = true;
    }
    
    //Set points on plane, calculate distance, display distances on scoll view
    private void SetPoints()
    {
        Touch touch = Input.GetTouch(0);
        touchPosition = touch.position;
        if (arRaycastManager.Raycast(touchPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
        {
            if (Input.GetMouseButtonUp(0) && !distancesPanel.activeSelf)
            {
                var Point = Instantiate(measurementPointPrefab, Vector3.zero, Quaternion.identity) as GameObject;
                Pose hitpose = hits[0].pose;
                Point.transform.SetPositionAndRotation(hitpose.position, hitpose.rotation);
                _augmentations.Add(Point);
                n = _augmentations.Count;
                if (_augmentations.Count > 1)
                {
                    for (int i = n - 1; i < _augmentations.ToArray().Length; i++)
                    {
                        measureLine.enabled = true;
                        drawLines();
                        guiText = (Vector3.Distance(_augmentations[i].transform.position, _augmentations[i - 1].transform.position)*cmOrm).ToString("F2") + " " + cmM;
                       
                        textToAdd = (GameObject)Instantiate(distanceItem);
                        if (textToAdd != null)
                        {
                            textToAdd.GetComponent<Text>().text = guiText;
                            distancesList.Add(guiText);                                  
                            textToAdd.transform.SetParent(distancesContainer);                            
                        }                                                            
                        OnGUI();    
                            
                    } 
                }
            }

        }
    }

    void OnGUI()
    {
        GUIStyle localStyle = new GUIStyle();
        localStyle.normal.textColor = Color.white;
        localStyle.fontSize = 70;  
        GUI.Label(new Rect(90 + 20, 200 + 20, Screen.width - 20, 30), guiText, localStyle);
    }

    //Draw Line Renderer
    private void drawLines()
    {
        // Set positions size
        measureLine.positionCount = _augmentations.ToArray().Length;
        // Set all line psitions
        for (int i = 0; i < _augmentations.ToArray().Length; i++)
        {                                  
            measureLine.SetPosition(i, new Vector3(_augmentations[i].transform.position.x, _augmentations[i].transform.position.y, _augmentations[i].transform.position.z));
            
        }
    }

    public void destroyLines()
    {
        measureLine.enabled = false;
    }

    //Reset detected Planes
    private void DeactivePlanes()
    {
        var xrManagerSettings = UnityEngine.XR.Management.XRGeneralSettings.Instance.Manager;
        xrManagerSettings.DeinitializeLoader();
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex); // reload current scene
        xrManagerSettings.InitializeLoaderSync();
    }
    public void ResetScene()
    { 
        //// reset augmentations
        foreach (var augmentation in _augmentations)
        {
            Destroy(augmentation);
        }
        _augmentations.Clear();

        destroyLines();

        DeactivePlanes();
   
        guiText = $"Distance: {0:F2}";
    }

    IEnumerator WaitALittleBit()
    {
        yield return new WaitForSeconds(3f);
        isReady = true;
    }

    public void StartMeasure()
    {
        StartCoroutine(WaitALittleBit());
        panel.SetActive(false);
    }


    //Save the measurements to json-file
    public void SaveMyData()
    {
        if (isSaved == false)
        {
            MeasurementData m = new MeasurementData(inputName.text, distancesList);
            myData.SaveIntoJson2(m,inputName.text);
        }
        isSaved = true;
    }



    public void showDistancesPanel()
    {       
        distancesPanel.SetActive(true);                
    }

    public void hideDistancesPanel()
    {
        distancesPanel.SetActive(false);
    }
 
    public void cmClicked()
    {
        cmOrm = 100;
        cmM = "cm";
    }

    public void mClicked()
    {
        cmOrm = 1;
        cmM = "m";
    }

}

