using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(requiredComponent: typeof(ARRaycastManager),
    requiredComponent2 : typeof(ARPlaneManager))]
public class PlaceObject : MonoBehaviour
{
    [SerializeField]
    private GameObject prefab;

    private GameObject obj = null;

    private ARRaycastManager aRRaycastManager;
    private ARPlaneManager aRPlaneManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool showingObject = false;

    private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void FingerDown(EnhancedTouch.Finger finger) {
        if (finger.index != 0) return;
        if (aRRaycastManager.Raycast(finger.currentTouch.screenPosition,
         hits,TrackableType.PlaneWithinPolygon)) {
            foreach (ARRaycastHit hit in hits) {
                if (showingObject) {
                    Destroy( obj );
                }
                else
                {
                    Pose pose = hit.pose;
                    obj = Instantiate( original: prefab, position: pose.position,
                    rotation: pose.rotation );
                    float myScale = 0.25f;
                    Vector3 scale = new Vector3( myScale, myScale, myScale );
                    obj.transform.localScale = scale;
                    Quaternion rotation = Quaternion.Euler( 0f, 180f, 0);
                    obj.transform.localRotation = rotation ;
                }
                showingObject = !showingObject;
            }
        }
    }

}
