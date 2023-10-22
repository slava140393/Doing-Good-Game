using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
	[RequireComponent(typeof(UIDocument))]
	public class UILoader : MonoBehaviour
	{
		private JsonReadWriteService _jsonReadWriteService;
		private VisualElement _root;

		private List<DataPerDay> _dataPerDayList;
		private MainPage _mainPage;
		private PopupWindow _popupWindow;
		private CalendarPage _calendarPage;

		public void Initialize(JsonReadWriteService jsonReadWriteService)
		{
			_dataPerDayList = new List<DataPerDay>();
			_root = GetComponent<UIDocument>().rootVisualElement;
			_jsonReadWriteService = jsonReadWriteService;

			CreateMainPage();
			CreatePopupWindow();
			ShowMainPageOnDate(GetCurrentDate());
			CreateCalendarPage();
		}
		private void CreateMainPage()
		{
			_mainPage = new MainPage();
			_mainPage.InitializePage();

			_root.Add(_mainPage);
			_mainPage.OnPopupButtonClicked += ShowPopup;
			_mainPage.OnCalendarPageButtonClicked += ShowCalendarPage;
			_mainPage.OnChangeDayButtonClicked += ShowMainPageOnDate;
			_mainPage.OnCurrentDataChanged += SaveDataInFile;
			_mainPage.OnTotalScoreChanged += SaveTotalScoreInFile;
		}
		private void ShowMainPageOnDate(string date)
		{
			DataPerDay dataPerDay = GetOpenedPage(date);

			if(dataPerDay == default)
			{
				dataPerDay = GetDataFromFile(date);

				if(dataPerDay == null)
				{
					dataPerDay = new DataPerDay(date);
				}
			}

			_dataPerDayList.Add(dataPerDay);
			_mainPage.FillingActionsPanel(dataPerDay);
			_mainPage.SetTotalScoreLabel(LoadTotalScoreFromFile());
		}

		private DataPerDay GetOpenedPage(string date) => _dataPerDayList.FirstOrDefault(dataPerDay => dataPerDay.Date == date);
		private void CreateCalendarPage()
		{
			_calendarPage = new CalendarPage();
			_root.Add(_calendarPage);
			_calendarPage.OnDaySelected += ShowMainPageOnDate;
		}

		private void ShowCalendarPage()
		{
			_mainPage.HidePage();
			_calendarPage.ShowPage();
		}
		private void SaveDataInFile(DataPerDay dataPerDay)
		{
			DataPerDay clonedDataPerDay = dataPerDay.Clone() as DataPerDay;

			if(clonedDataPerDay != null)
			{
				_jsonReadWriteService.SaveToJson(clonedDataPerDay, clonedDataPerDay.Date);
			}
		}

		private void SaveTotalScoreInFile(int totalScore)
		{
			_jsonReadWriteService.SaveTotalScoreToFile(totalScore);
		}

		private int LoadTotalScoreFromFile() => _jsonReadWriteService.ReadTotalScoreFromJson();

		private void CreatePopupWindow()
		{
			_popupWindow = new PopupWindow();
			_root.Add(_popupWindow);
		}
		private void ShowPopup()
		{
			_popupWindow.Show();
		}


		private string GetCurrentDate() => System.DateTime.UtcNow.ToLocalTime().ToString("dd.MM.yyyy");
		private DataPerDay GetDataFromFile(string date) => _jsonReadWriteService.ReadFromJson(date);

		private void OnDisable()
		{
			_mainPage.OnPopupButtonClicked -= ShowPopup;

			_mainPage.UnregisterCallbacks();
			_calendarPage.UnregisterCallbacks();
		}
	}
}