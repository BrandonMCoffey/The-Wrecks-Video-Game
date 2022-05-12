using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts.UserInput.MobileInput
{
    public class MobileTapButton : MobileInputBase
    {
        public override void OnPointerDown(PointerEventData eventData)
        {
            Holding = true;

            Clicked = true;
            StartCoroutine(StopClickEvent());
        }

        private WaitForEndOfFrame _waitForEndOfFrame = new WaitForEndOfFrame();

        private IEnumerator StopClickEvent()
        {
            yield return _waitForEndOfFrame;

            Clicked = false;
        }

        public override void OnDrag(PointerEventData eventData)
        {
        }

        //Do this when the mouse click on this selectable UI object is released.
        public override void OnPointerUp(PointerEventData eventData)
        {
            Holding = false;
        }
    }
}