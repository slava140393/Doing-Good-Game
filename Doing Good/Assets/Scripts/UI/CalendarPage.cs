using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class CalendarPage : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<CalendarPage>
		{

		}
		public Action<string> OnDaySelected;



		private const string PageCalendarTemplate = "PageCalendarTemplate";
		private const string StyleResource = "Stylesheet_Calendar";
		private const string USSFrameClean = "frame_clean";
		private const string USSFrameToday = "frame_today";

		private const string DayViewName = "Day";
		private const string MonthViewName = "CurrentMonth";
		private const string PreviousMonthButtonName = "PreviousMonth";
		private const int PreviousMonthDirection = -1;
		private const string NextMonthButtonName = "NextMonth";
		private const int NextMonthDirection = +1;

		private List<Label> _daysContainerList;
		private Calendar _calendar;
		private Label _monthView;

		public CalendarPage()
		{
			this.style.flexGrow = 1;
			VisualTreeAsset template = Resources.Load<VisualTreeAsset>(PageCalendarTemplate);
			VisualElement container = SetupContainer(template);
			SetupCurrentMonthPanel(container);
			SetupChangeMonthButtons(container);
			SetupDaysPanel(container);
			HidePage();
		}
		public void UnregisterCallbacks()
		{
			_calendar.OnCalendarChanged -= UpdateCalendarView;

			foreach (Label label in _daysContainerList)
			{
				label.UnregisterCallback<PointerDownEvent, Day>(OnDayClicked);
			}
		}
		
		public void HidePage()
		{
			this.style.display = DisplayStyle.None;
		}
		
		public void ShowPage()
		{
			this.style.display = DisplayStyle.Flex;
		}
		private VisualElement SetupContainer(VisualTreeAsset template)
		{
			VisualElement container = template.Instantiate();
			styleSheets.Add(Resources.Load<StyleSheet>(StyleResource));
			container.style.flexGrow = 1;
			hierarchy.Add(container);
			return container;
		}
		private void SetupCurrentMonthPanel(VisualElement container)
		{
			_monthView = container.Q<Label>(MonthViewName);
		}
		private void SetupChangeMonthButtons(VisualElement container)
		{
			Button previousMonthButton = container.Query<Button>(PreviousMonthButtonName);
			previousMonthButton.RegisterCallback<PointerUpEvent, int>(OnChangeMonthButtonClicked, PreviousMonthDirection);

			Button nextMonthButton = container.Query<Button>(NextMonthButtonName);
			nextMonthButton.RegisterCallback<PointerUpEvent, int>(OnChangeMonthButtonClicked, NextMonthDirection);
		}
		private void OnChangeMonthButtonClicked(PointerUpEvent evt, int nextMonthDirection)
		{
			_calendar.SwitchMonth(nextMonthDirection);
		}

		private void SetupDaysPanel(VisualElement container)
		{
			_daysContainerList = new List<Label>();
			_daysContainerList = container.Query<Label>(DayViewName).ToList();


			_calendar = new Calendar();
			_calendar.OnCalendarChanged += UpdateCalendarView;
			UpdateCalendarView();
		}
		private void UpdateCalendarView()
		{
			_monthView.text = _calendar.GetMonthName();

			for( int i = 0; i < _daysContainerList.Count; i++ )
			{
				Day day = _calendar.Days[i];
				Label dayView = _daysContainerList[i];

				if(!day.IsInThisMonth)
				{
					dayView.AddToClassList(USSFrameClean);
					dayView.focusable = false;
					continue;
				}
				dayView.RemoveFromClassList(USSFrameClean);
				dayView.focusable = true;
				dayView.text = day.DayNum.ToString();
				dayView.RegisterCallback<PointerDownEvent, Day>(OnDayClicked, day);

				if(day.IsToday)
				{
					dayView.AddToClassList(USSFrameToday);
				}
			}
		}
		private void OnDayClicked(PointerDownEvent evt, Day day)
		{
			OnDaySelected?.Invoke(day.GetDateOfDay());
			HidePage();
		}


	}
}