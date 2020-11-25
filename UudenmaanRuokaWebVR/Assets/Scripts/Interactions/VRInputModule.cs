using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using WebXR;

public class VRInputModule : BaseInputModule
{
    public Camera cam;

    GameObject currentObject = null;
    PointerEventData data = null;
    public static bool pointingUI = false;

    bool pointerPress = false;
    bool pointerRelease = false;

    protected override void Awake()
    {
        base.Awake();

        data = new PointerEventData(eventSystem);
        PhysicsRaycaster.onPointerUp += PointerUp;
        PhysicsRaycaster.onPointerDown += PointerDown;
    }

    protected override void OnDisable()
    {
        PhysicsRaycaster.onPointerUp -= PointerUp;
        PhysicsRaycaster.onPointerDown -= PointerDown;
    }

    public void PointerDown()
    {
        pointerPress = true;
    }

    public void PointerUp()
    {
        pointerRelease = true;
    }

    public override void Process()
    {
        data.Reset();
        data.position = new Vector2(cam.pixelWidth / 2, cam.pixelHeight / 2);

        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentObject = data.pointerCurrentRaycast.gameObject;

        m_RaycastResultCache.Clear();

        if (!eventSystem.currentSelectedGameObject)
            HandlePointerExitAndEnter(data, data.pointerCurrentRaycast.gameObject);

        if (pointerPress)
        {
            ProcessPress(data);
            pointerPress = false;
        }

        if (pointerRelease)
        {
            ProcessRelease(data);
            pointerRelease = false;
        }
    }

    public PointerEventData GetData()
    {
        return data;
    }

    private void ProcessPress(PointerEventData data)
    {
        //Debug.Log("Process Press");
        data.pointerPressRaycast = data.pointerCurrentRaycast;

        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentObject, data, ExecuteEvents.pointerDownHandler);

        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        data.pressPosition = data.position;
        data.pointerPress = newPointerPress;
        data.rawPointerPress = currentObject;
    }

    private void ProcessRelease(PointerEventData data)
    {
        //Debug.Log("Process Release");
        ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerUpHandler);

        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentObject);

        if (data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(data.pointerPress, data, ExecuteEvents.pointerClickHandler);
        }

        eventSystem.SetSelectedGameObject(null);

        data.pressPosition = Vector2.zero;
        data.pointerPress = null;
        data.rawPointerPress = null;
    }
}
