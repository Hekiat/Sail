using System;
using UnityEngine;


namespace sail.animation
{
	public class Tweener : EasingController
	{
		#region Properties
		public static float DefaultDuration = 1f;
		public static Func<float, float, float, float> DefaultEquation = EasingFunctions.EaseInOutQuad;
		public bool destroyOnComplete = true;
		#endregion

		#region Event Handlers
		protected override void OnComplete()
		{
			base.OnComplete();
			if (destroyOnComplete)
			{
				Destroy(this);
			}
		}
		#endregion
	}

    public abstract class Tweener<T> : Tweener
    {
        public T startTweenValue;
        public T endTweenValue;
        public T currentTweenValue { get; private set; }

        protected override void OnUpdate()
        {
            //float currentValue = 0.5f;
            dynamic s = startTweenValue;
            dynamic e = endTweenValue;

            currentTweenValue = (T)((e - s) * currentValue + s);

            base.OnUpdate();
        }
    }
}
