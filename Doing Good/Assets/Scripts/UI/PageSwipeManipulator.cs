using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class PageSwipeManipulator : Manipulator
	{
		private readonly Action<int> _toNextPage;
		private Vector3 _startPosition;
		private bool _isMoved;
		private bool _wasMoved;

		private bool _pageChanged;
		private float _distanceSwipe = 150f;
		private float _minScrollingDistanceY = 50f;

		public PageSwipeManipulator(Action<int> toNextPage)
		{
			_toNextPage = toNextPage;

		}

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
			_pageChanged = false;
			_startPosition = evt.position;
		}
		private void OnPointerMove(PointerMoveEvent evt)
		{
			if(_isMoved)
			{
				float diff = _startPosition.x - evt.position.x;

				if(Mathf.Abs(diff) > _distanceSwipe && Mathf.Abs(_startPosition.y - evt.position.y) < _minScrollingDistanceY)
				{
					_wasMoved = true;
					Debug.Log("Swipe");
					evt.StopPropagation();
					_isMoved = false;

					if(!_pageChanged && diff > 0)
					{
						_toNextPage(1);
						_pageChanged = true;
					}

					if(!_pageChanged && diff < 0)
					{
						_toNextPage(-1);
						_pageChanged = true;
					}
				}
			}
		}
	}
}