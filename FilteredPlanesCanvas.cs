using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilteredPlanesCanvas : MonoBehaviour
{
    [SerializeField]
    private Toggle verticalPlaneToggle;
    [SerializeField]
    private Toggle horizontalPlaneToggle;
    [SerializeField]
    private Toggle bigPlaneToggle;
    [SerializeField]
    private Button startButton;
    
   
    private ARFilteredPlanes aRFilteredPlanes;
    public bool VerticalPlaneToggle 
    {
        get => verticalPlaneToggle.isOn;
        set
        {
            verticalPlaneToggle.isOn = value;
            CheckIfAllAreTrue();
        }
    }

    public bool HorizontalPlaneToggle
    {
        get => horizontalPlaneToggle.isOn;
        set
        {
            horizontalPlaneToggle.isOn = value;
            CheckIfAllAreTrue();
        }
    }

    public bool BigPlaneToggle
    {
        get => bigPlaneToggle.isOn;
        set
        {
            bigPlaneToggle.isOn = value;
            CheckIfAllAreTrue();
        }
    }

    private void OnEnable()
    {
        aRFilteredPlanes = FindObjectOfType<ARFilteredPlanes>();

        aRFilteredPlanes.OnVerticalPlaneFound += () => VerticalPlaneToggle = true;
        aRFilteredPlanes.OnHorizontalPlaneFound += () => HorizontalPlaneToggle = true;
        aRFilteredPlanes.OnBigPlaneFound += () => BigPlaneToggle = true;

    }

    private void OnDisable()
    {
        aRFilteredPlanes.OnVerticalPlaneFound -= () => VerticalPlaneToggle = true;
        aRFilteredPlanes.OnHorizontalPlaneFound -= () => HorizontalPlaneToggle = true;
        aRFilteredPlanes.OnBigPlaneFound -= () => BigPlaneToggle = true;
    }

    private void CheckIfAllAreTrue()
    {
        if (VerticalPlaneToggle && HorizontalPlaneToggle && BigPlaneToggle)
        {
            startButton.interactable = true;
             
        }   
    }

}
