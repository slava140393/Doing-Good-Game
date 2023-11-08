using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	public class MainPage : VisualElement
	{
		public new class UxmlFactory : UxmlFactory<MainPage>
		{

		}
		private const int BaseCountActionData = 12;
		private const int NextDay = +1;
		private const int PreviousDay = -1;
		private const string RoublesGood = "Рублей Добра";
		private const string PageTemplatePath = "PageTemplate";
		private const string AdviceButtonName = "AdviceButton";
		private const string CalendarPageButtonName = "CalendarButton";
		private const string ScorePerDayLabelName = "ScorePerDay";
		private const string ScoreAllTimeLabelName = "ScoreAll";
		private const string CurrentMonthLabelName = "Month";
		private const string CurrentDayLabelName = "Day";
		private const string PreviousDayButtonName = "PreviousDay";
		private const string NextDayButtonName = "NextDay";

		public Action<DataPerDay> OnCurrentDataChanged;
		public Action<int> OnTotalScoreChanged;
		public Action<string> OnChangeDayButtonClicked;
		public Action OnPopupButtonClicked;
		public Action OnCalendarPageButtonClicked;

		private DataPerDay _currentData;

		private VisualElement _actionsPanel;
		private Button _adviceButton;
		private List<ActionPanelView> _actionPanelViewList;
		private Button _calendarPageButton;
		private Label _scorePerDayLabel;
		private Label _currentMonthLabel;
		private Label _currentDayLabel;
		private Button _nextDayButton;
		private Button _previousDayButton;
		private Label _scoreAllTimeLabel;

		private int _totalScore = 0;
		public void InitializePage()
		{
			_actionPanelViewList = new List<ActionPanelView>();
			style.flexGrow = 1;
			CreatePage();
			SetupActionPanelView();

			_adviceButton = this.Q<Button>(AdviceButtonName);
			_adviceButton.RegisterCallback<PointerUpEvent>(PopupButtonClicked);

			_calendarPageButton = this.Q<Button>(CalendarPageButtonName);
			_calendarPageButton.RegisterCallback<PointerUpEvent>(CalendarPageButtonClicked);

			_scorePerDayLabel = this.Q<Label>(ScorePerDayLabelName);
			_scoreAllTimeLabel = this.Q<Label>(ScoreAllTimeLabelName);

			_currentMonthLabel = this.Q<Label>(CurrentMonthLabelName);
			_currentDayLabel = this.Q<Label>(CurrentDayLabelName);

			_nextDayButton = this.Q<Button>(NextDayButtonName);
			_nextDayButton.RegisterCallback<PointerUpEvent, int>(ShowNextDayPage, NextDay);

			_previousDayButton = this.Q<Button>(PreviousDayButtonName);
			_previousDayButton.RegisterCallback<PointerUpEvent, int>(ShowNextDayPage, PreviousDay);

		}
		public void FillingActionsPanel(DataPerDay currentData)
		{
			if(_actionPanelViewList.Count > 0)
			{
				ClearActionPanelView();
			}
			_currentData = currentData;


			for( int i = 0; i < _currentData.ActionDataList.Count; i++ )
			{
				ActionPanelView panel = CreateEmptyActionPanelView();
				panel.SetupData(currentData.ActionDataList[i]);

				if(i>=BaseCountActionData-1)
				{
					break;
				}
				// if(_currentData.ActionDataList.Count - 1 == i && !_currentData.ActionDataList[i].IsEmptyData())
				// {
				// 	CreateEmptyData();
				// 	break;
				// }
			}
			int emptyActionPanelViewCount = BaseCountActionData - _currentData.ActionDataList.Count;
			
			if(emptyActionPanelViewCount > 0)
			{
				for( int i = 0; i < emptyActionPanelViewCount; i++ )
				{
					CreateEmptyData();
				}
			}

			FillCurrentDateLabels();
			FillCounterLabel();
			ShowPage();
		}
		private void CreateEmptyData()
		{
			ActionPanelView panelView = CreateEmptyActionPanelView();
			ActionData data = _currentData.AddNewActionData();
			panelView.SetupData(data);
		}
		public void HidePage()
		{
			this.style.display = DisplayStyle.None;
		}

		public void ShowPage()
		{
			this.style.display = DisplayStyle.Flex;
		}

		public void UnregisterCallbacks()
		{
			_adviceButton.UnregisterCallback<PointerUpEvent>(PopupButtonClicked);
			_calendarPageButton.UnregisterCallback<PointerUpEvent>(CalendarPageButtonClicked);

			_nextDayButton.UnregisterCallback<PointerUpEvent, int>(ShowNextDayPage);
			_previousDayButton.UnregisterCallback<PointerUpEvent, int>(ShowNextDayPage);

			foreach (ActionPanelView actionPanelView in _actionPanelViewList)
			{
				actionPanelView.OnActionDataFilled -= OnActionDataFilled;

				actionPanelView.UnregisterCallbacks();
			}
		}

		public void SetTotalScoreLabel(int totalScore)
		{
			_totalScore = totalScore;
			_scoreAllTimeLabel.text = $"{(_totalScore).ToString()} {RoublesGood}";
		}
		private void ShowNextDayPage(PointerUpEvent evt, int direction)
		{
			DateTime date = _currentData.GetDateAsDateTime();
			OnChangeDayButtonClicked?.Invoke(date.AddDays(direction).ToString("dd.MM.yyyy"));
		}
		private void CalendarPageButtonClicked(PointerUpEvent evt)
		{
			OnCalendarPageButtonClicked?.Invoke();
		}
		private void CreatePage()
		{
			VisualTreeAsset template = Resources.Load<VisualTreeAsset>(PageTemplatePath);
			VisualElement templateElem = template.Instantiate();
			hierarchy.Add(templateElem);
			templateElem.style.flexGrow = 1;
		}
		private void SetupActionPanelView()
		{
			_actionsPanel = this.Q("ActionsPanel");
			
		}

		private ActionPanelView CreateEmptyActionPanelView()
		{
			ActionPanelView actionPanelView = new ActionPanelView();
			_actionPanelViewList.Add(actionPanelView);
			actionPanelView.Initialize();
			_actionsPanel.Add(actionPanelView);
			actionPanelView.OnActionDataFilled += OnActionDataFilled;
			return actionPanelView;
		}
		private void FillCurrentDateLabels()
		{
			DateTime date = _currentData.GetDateAsDateTime();
			_currentDayLabel.text = date.Day.ToString();
			_currentMonthLabel.text = RussianMonthsName.GetMonthName(date.Month);
		}
		private void ClearActionPanelView()
		{
			foreach (ActionPanelView actionPanelView in _actionPanelViewList)
			{
				actionPanelView.UnregisterCallbacks();
				_actionsPanel.Remove(actionPanelView);
			}
			_actionPanelViewList.Clear();
		}
		private void OnActionDataFilled(ActionData actionData)
		{
			// if(actionData.EntryNumber == _actionPanelViewList.Count)
			// {
			// 	ActionPanelView panelView = CreateEmptyActionPanelView();
			// 	ActionData data = _currentData.AddNewActionData();
			// 	panelView.SetupData(data);
			// }
			FillCounterLabel();

			OnCurrentDataChanged?.Invoke(_currentData);
			OnTotalScoreChanged?.Invoke(_totalScore);

		}
		private void FillCounterLabel()
		{
			int count = 0;

			foreach (ActionData actionData in _currentData.ActionDataList)
			{
				count += actionData.ActionScore == Int32.MinValue ? 0 : actionData.ActionScore;
			}
			_totalScore = _totalScore - _currentData.TotalScore + count;
			_currentData.TotalScore = count;
			_scorePerDayLabel.text = $"{count.ToString()} {RoublesGood}";
			_scoreAllTimeLabel.text = $"{(_totalScore).ToString()} {RoublesGood}";
		}

		private void PopupButtonClicked(PointerUpEvent evt)
		{
			OnPopupButtonClicked?.Invoke();
		}


	}
}