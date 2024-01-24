using UnityEngine;
using UnityEngine.EventSystems;

public class FixedTouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public Vector2 TouchDist { get; private set; }
    [HideInInspector] public Vector2 PointerOld { get; private set; }
    [HideInInspector] private int pointerId = -1;
    [HideInInspector] public bool Pressed { get; private set; }

    void Update()
    {
        UpdateTouchInput();
    }

    private void UpdateTouchInput()
    {
        if (Pressed)
        {
            if (IsValidPointerId())
            {
                TouchDist = Input.touches[pointerId].position - PointerOld;
                PointerOld = Input.touches[pointerId].position;
            }
            else
            {
                TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                PointerOld = Input.mousePosition;
            }
        }
        else
        {
            TouchDist = Vector2.zero;
        }
    }

    private bool IsValidPointerId()
    {
        return pointerId >= 0 && pointerId < Input.touches.Length;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Pressed = true;
        pointerId = eventData.pointerId;
        PointerOld = eventData.position;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Pressed = false;
    }
}