using System;
using System.Collections;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;



public class ObjectDrag : MonoBehaviour
{

    private bool isTaken = false;
    private static readonly float eps = 0.001f;

    [SerializeField]
    private float rotationCoef = 20.0f;
    [SerializeField]
    private float zoomOnTouch = 3.0f;
    [SerializeField]
    private float angleLimiter = 80.0f;

    private Vector3 mOffset;


    private Vector3 originPos;
    private Quaternion originalRotation;
    private bool originSet = false;

    private bool dragEnabled = true;
    private bool dragStarted = false;
    public void SetDragEnabled(bool value) {
        dragEnabled = value;
    }

    private Camera cam;
    private Collider coll;

    private void Start()
    {
        GameEvents.current.onSetDragableObjectEnableState += SetDragEnabled;

        cam = Camera.main;
        coll = GetComponent<Collider>();
    }

    private void OnDestroy()
    {
        GameEvents.current.onSetDragableObjectEnableState -= SetDragEnabled;
    }


    private void Update()
    {
        if (isTaken )
        {
            return;
        }

#if UNITY_EDITOR
        MouseController();
#elif UNITY_ANDROID
        TouchController();
#else
        MouseController();
#endif

    }

    private void MouseController()
    {
        if (CheckPressingCollider())
        {
            if (Input.GetMouseButtonDown(0))
            {
                ObjectPressDown();
            }
        }
        if (Input.GetMouseButtonUp(0) && dragStarted)
        {
            ObjectPressUp();
        }
        else if (Input.GetMouseButton(0) && dragStarted)
        {
            ObjectPressDrag();
        }
    }

    private void TouchController()
    {
        if (CheckPressingCollider())
        {
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Began))
            {
                ObjectPressDown();
            }
        }
        if ((Input.GetTouch(0).phase.Equals(TouchPhase.Ended) || Input.GetTouch(0).phase.Equals(TouchPhase.Canceled))
            && dragStarted)
        {
            ObjectPressUp();
        }
        else if ((Input.GetTouch(0).phase.Equals(TouchPhase.Moved) || Input.GetTouch(0).phase.Equals(TouchPhase.Stationary))
            && dragStarted)
        {
            ObjectPressDrag();
        }
    }

    private bool CheckPressingCollider()
    {
        Ray ray;
#if UNITY_EDITOR
        ray = cam.ScreenPointToRay(Input.mousePosition);

#elif UNITY_ANDROID
        ray = cam.ScreenPointToRay(Input.touches[0].position);
#else
        ray = cam.ScreenPointToRay(Input.mousePosition);
#endif
        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.Equals(coll))
                return true;
        }
        return false;
    }

    private void ObjectPressDown()
    {
        if (!originSet)
        {
            originPos = gameObject.transform.position;
            originalRotation = gameObject.transform.rotation;
            originSet = true;
        }
        else
        {
            try
            {
                LeanTween.pause(gameObject);
                LeanTween.reset();
            }
            catch (NullReferenceException) { }
        }

        if (!dragEnabled || isTaken)
            return;

        dragStarted = true;

        LeanTween.moveZ(gameObject, originPos.z - zoomOnTouch, 0.1f).setEaseOutQuint();

        mOffset = gameObject.transform.position - GetPressAsWorldPoint();

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    private void ObjectPressDrag()
    {
        if (!dragEnabled || isTaken)
            return;
        transform.position = GetPressAsWorldPoint() + mOffset;
        DoTransformation();
    }

    private void ObjectPressUp()
    {
        if (!dragEnabled || isTaken)
            return;

        Ray ray;
#if UNITY_EDITOR
        ray = cam.ScreenPointToRay(Input.mousePosition);

#elif UNITY_ANDROID
        ray = cam.ScreenPointToRay(Input.touches[0].position);
#else 
        ray = cam.ScreenPointToRay(Input.mousePosition);
#endif

        RaycastHit[] hits;
        hits = Physics.RaycastAll(ray);
        foreach (RaycastHit hit in hits)
        {
            DropController dropController = hit.collider.gameObject.GetComponent<DropController>();
            if (dropController != null)
            {
                GameEvents.current.DropTriggerEnter(gameObject, dropController.dropSide);
                isTaken = true;
            }
        }

        if (!isTaken)
            ResetTransformation();

        dragStarted = false;
    }

    private Vector3 GetPressAsWorldPoint()
    {
        Vector3 mousePoint;
#if UNITY_EDITOR
        mousePoint = Input.mousePosition;

#elif UNITY_ANDROID
        mousePoint = Input.touches[0].position;
#else 
        mousePoint = Input.mousePosition;
#endif


        mousePoint.z = gameObject.transform.position.z;

        return cam.ScreenToWorldPoint(mousePoint);
    }

    public void DoTransformation()
    {
        Vector2 currentPos = gameObject.transform.position;
        Vector2 origin2D = originPos;
        Vector2 diff = (currentPos - origin2D) * rotationCoef;
        if (Math.Abs(diff.x) - angleLimiter > eps)
            diff.x = Math.Sign(diff.x) * angleLimiter;
        if (Math.Abs(diff.y) - angleLimiter > eps)
            diff.y = Math.Sign(diff.y) * angleLimiter;
        var startRot = originalRotation.eulerAngles;
        gameObject.transform.rotation = Quaternion.Euler(startRot.x - diff.y, startRot.y + diff.x, startRot.z);
    }

    public void ResetTransformation()
    {
        
        LeanTween.move(gameObject, originPos, 0.3f).setEaseInBack();
        LeanTween.rotate(gameObject, originalRotation.eulerAngles, 0.3f);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void TakeControl()
    {
        isTaken = true;
    }
}