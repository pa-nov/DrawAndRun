using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private readonly List<Vector2> Path = new();
    public GameObject DrawArea, LeftDown, RightTop, Line, Point;

    private bool Touching;
    private int TouchId;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!Touching)
        {
            Touching = true;
            TouchId = eventData.pointerId;

            Vector2 TouchPosition = eventData.position;

            GameObject NewPoint = Instantiate(Point, TouchPosition, Point.transform.rotation, DrawArea.transform);
            NewPoint.SetActive(true);

            Path.Add(TouchPosition);
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Touching && eventData.pointerId == TouchId && Vector3.Magnitude(eventData.position - Path[^1]) > 5)
        {
            Vector2 TouchPosition = eventData.position;

            if (TouchPosition.x > LeftDown.transform.position.x && TouchPosition.x < RightTop.transform.position.x &&
                TouchPosition.y > LeftDown.transform.position.y && TouchPosition.y < RightTop.transform.position.y)
            {
                Vector2 LinePosition = Path[^1] + (TouchPosition - Path[^1]) / 2;
                Quaternion LineRotation = Quaternion.Euler(0, 0, GetAngle(TouchPosition, Path[^1]));
                GameObject NewLine = Instantiate(Line, LinePosition, LineRotation, DrawArea.transform);
                NewLine.transform.localScale = new(Vector3.Magnitude(TouchPosition - Path[^1]) * (1080f / Screen.width), 1, 1);
                NewLine.SetActive(true);

                GameObject NewPoint = Instantiate(Point, TouchPosition, Point.transform.rotation, DrawArea.transform);
                NewPoint.SetActive(true);

                Path.Add(TouchPosition);
            }
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (Touching && eventData.pointerId == TouchId)
        {
            Touching = false;

            Player.Instance.MoveCharacters(Path, LeftDown.transform.position, RightTop.transform.position);

            for (int i = 0; i < DrawArea.transform.childCount; i++)
            {
                Destroy(DrawArea.transform.GetChild(i).gameObject);
            }
            Path.Clear();
        }
    }

    private float GetAngle(Vector2 NewPoint, Vector2 OldPoint)
    {
        Vector2 Distance = NewPoint - OldPoint;
        return Mathf.Rad2Deg * Mathf.Atan2(Distance.y, Distance.x);
    }
}