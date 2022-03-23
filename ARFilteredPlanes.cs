using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARFilteredPlanes : MonoBehaviour
{
    [SerializeField]
    private Vector2 dimensionsForBigPlane;

    public event Action OnVerticalPlaneFound;
    public event Action OnHorizontalPlaneFound;
    public event Action OnBigPlaneFound;
    private ARPlaneManager arPlaneManager;
    private List<ARPlane> arPlanes;

    private void OnEnable()
    {
        arPlanes = new List<ARPlane>();
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
        arPlaneManager.planesChanged += OnPlanesChanged;
    }

    private void OnDisable()
    {
        arPlaneManager.planesChanged -= OnPlanesChanged;
    }

    private void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        if (args.added != null && args.added.Count > 0)
            arPlanes.AddRange(args.added);

        foreach(ARPlane plane in arPlanes.Where(plane => plane.extents.x * plane.extents.y >= 0.1f))
        {
            if (plane.alignment.IsVertical())
            {
                OnVerticalPlaneFound.Invoke();
                
            }
            else
            {
                OnHorizontalPlaneFound.Invoke();
            }
            if (plane.extents.x * plane.extents.y >= dimensionsForBigPlane.x * dimensionsForBigPlane.y)
            {
                OnBigPlaneFound.Invoke();
            }
        }
    }
}
