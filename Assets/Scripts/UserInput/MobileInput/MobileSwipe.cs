using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UserInput.MobileInput
{
    public class MobileSwipe : MonoBehaviour
    {
        [SerializeField] private float _dragMin = 10;

        private Dictionary<int, TouchSet> _touches = new Dictionary<int, TouchSet>();

        private void Update()
        {
            for (int i = 0; i < Input.touchCount; i++) {
                Touch touch = Input.GetTouch(i);
                switch (touch.phase) {
                    case TouchPhase.Began:
                        _touches.Add(i, new TouchSet(touch.position));
                        break;
                    case TouchPhase.Moved:
                        _touches[i].SetPos(touch.position);
                        break;
                    case TouchPhase.Ended:
                        ConfirmAction(_touches[i].GetAction(_dragMin));
                        _touches.Remove(i);
                        break;
                    case TouchPhase.Canceled:
                        _touches.Remove(i);
                        break;
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
        }

        private void ConfirmAction(TouchAction action)
        {
            Debug.Log(action);
        }
    }
}

internal class TouchSet
{
    private Vector2 _first;
    private Vector2 _current;

    public TouchSet(Vector2 pos)
    {
        _first = pos;
        _current = pos;
    }

    public void SetPos(Vector2 pos)
    {
        _current = pos;
    }

    public TouchAction GetAction(float dragMin)
    {
        float horz = Mathf.Abs(_current.x - _first.x);
        float vert = Mathf.Abs(_current.y - _first.y);
        if (horz > dragMin || vert > dragMin) {
            if (horz > vert) {
                return _current.x > _first.x ? TouchAction.SwipeRight : TouchAction.SwipeLeft;
            }
            return _current.y > _first.y ? TouchAction.SwipeUp : TouchAction.SwipeDown;
        }
        return TouchAction.Tap;
    }
}

internal enum TouchAction
{
    SwipeLeft,
    SwipeUp,
    SwipeDown,
    SwipeRight,
    Tap
}