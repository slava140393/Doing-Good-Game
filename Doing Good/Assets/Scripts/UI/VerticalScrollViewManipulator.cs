using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class VerticalScrollViewManipulator : Manipulator
	{
		private const float MinScrollingDistanceY = 50f;
		private Vector3 _startPosition;
		private bool _isMoved;
		private bool _wasMoved;

		protected override void RegisterCallbacksOnTarget()
		{
			target.RegisterCallback<PointerDownEvent>(OnPointerDown);
			target.RegisterCallback<PointerUpEvent>(OnPointerUp, TrickleDown.TrickleDown);
			target.RegisterCallback<PointerMoveEvent>(OnPointerMove, TrickleDown.TrickleDown);
		}
		protected override void UnregisterCallbacksFromTarget()
		{
			target.UnregisterCallback<PointerDownEvent>(OnPointerDown);
			target.UnregisterCallback<PointerUpEvent>(OnPointerUp);
			target.UnregisterCallback<PointerMoveEvent>(OnPointerMove);
		}


		private void OnPointerUp(PointerUpEvent evt)
		{
			if(_wasMoved)
			{
				evt.StopPropagation();
			}
			_wasMoved = false;
			_isMoved = false;
		}
		private void OnPointerDown(PointerDownEvent evt)
		{
			_isMoved = true;
			_startPosition = evt.position;
		}
		private void OnPointerMove(PointerMoveEvent evt)
		{
			if(_isMoved)
			{

				if(Mathf.Abs(_startPosition.y - evt.position.y) > MinScrollingDistanceY)
				{
					evt.StopPropagation();
					_wasMoved = true;
					_isMoved = false;
				}
			}
		}
	}
}