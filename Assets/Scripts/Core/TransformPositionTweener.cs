using System;
using UnityEngine;

namespace sail.animation
{
	public class TransformPositionTweener : Tweener<Vector3>
	{
		protected override void OnUpdate()
		{
			base.OnUpdate();
			transform.position = currentTweenValue;
		}
	}
}
