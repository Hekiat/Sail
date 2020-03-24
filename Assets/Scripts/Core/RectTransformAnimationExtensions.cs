using System;
using UnityEngine;

namespace sail.animation
{
    public static class RectTransformAnimationExtensions
    {
        public static Tweener MoveAnchoredPositionTo(this RectTransform t, Vector2 position)
        {
            return MoveAnchoredPositionTo(t, position, Tweener.DefaultDuration);
        }

        public static Tweener MoveAnchoredPositionTo(this RectTransform t, Vector2 position, float duration)
        {
            return MoveAnchoredPositionTo(t, position, duration, Tweener.DefaultEquation);
        }

        public static Tweener MoveAnchoredPositionTo(this RectTransform t, Vector2 position, float duration, Func<float, float, float, float> equation)
        {
            RectTransformAnchoredPositionTweener tweener = t.gameObject.AddComponent<RectTransformAnchoredPositionTweener>();
            tweener.startTweenValue = t.anchoredPosition;
            tweener.endTweenValue = position;
            tweener.duration = duration;
            tweener.equation = equation;
            tweener.Play();
            return tweener;
        }
    }
}
