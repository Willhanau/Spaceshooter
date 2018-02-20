using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SimpleCanTouchArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool touched;
    private int pointerID;
    private bool canFire;

    void Awake()
    {
        touched = false;
    }

    public void OnPointerDown(PointerEventData data)
    {
        //Set our start point
        if (touched == false)
        {
            touched = true;
            canFire = true;
            pointerID = data.pointerId;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        //Reset Everything
        if (data.pointerId == pointerID)
        {
            touched = false;
            canFire = false;
        }
    }

    public bool CanFire()
    {
        return canFire;
    }

}

