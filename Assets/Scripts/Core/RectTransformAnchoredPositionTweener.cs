using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace sail.animation
{
    public class RectTransformAnchoredPositionTweener : Tweener<Vector2>
    {
        protected override void OnUpdate()
        {
            base.OnUpdate();
            GetComponent<RectTransform>().anchoredPosition = currentTweenValue;
            Debug.Log(currentTweenValue);
        }
    }
}
