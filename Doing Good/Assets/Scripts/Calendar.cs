using System;
using System.Collections.Generic;
using Data;

public class Calendar
{
	public Action OnCalendarChanged;
	private const int WeekInMouth = 6;
	private const int DaysInWeek = 7;
	private const int DaysInMonthTable = 42;
	private const int OffsetToTheRussianCalendar = 1;
	private const int StartMonthDate = 1;

	private List<Day> _days = new List<Day>();
	private DateTime _currentDate;

	public Calendar()
	{
		SetupCalendar(DateTime.Now.Month, DateTime.Now.Year);
	}
	public List<Day> Days => _days;
	public string GetMonthName() => RussianMonthsName.GetMonthName(_currentDate.Month);
	public void SwitchMonth(int direction)
	{
		if(direction < 0)
		{
			_currentDate = _currentDate.AddMonths(-1);
		}
		else
		{
			_currentDate = _currentDate.AddMonths(1);
		}
		SetupCalendar(_currentDate.Month, _currentDate.Year);
	}
	private void SetupCalendar(int month, int year)
	{
		_currentDate = new DateTime(year, month, 1);
		int startDayOfWeek = GetMonthStartDayOfWeek(month, year);
		int endDay = GetTotalNumberOfDays(month, year);

		if(Days.Count == 0)
		{
			CreateNewCalendarList(startDayOfWeek, endDay, month, year);
		}
		else
		{
			UpdateCalendarList(startDayOfWeek, endDay, month, year);
		}

		if(DateTime.Now.Year == year && DateTime.Now.Month == month)
		{
			Days[(DateTime.Now.Day - 1) + startDayOfWeek].IsToday = true;
		}
		OnCalendarChanged?.Invoke();
	}
	private void CreateNewCalendarList(int startDay, int endDay, int month, int year)
	{
		for( int w = 0; w < WeekInMouth; w++ )
		{
			for( int d = 0; d < DaysInWeek; d++ )
			{
				Day newDay;
				int curDay = (w * DaysInWeek) + d;
				int numDay = curDay - startDay + OffsetToTheRussianCalendar;

				DateTime date = GetDate(numDay, month, year, startDay, endDay);

				if(curDay < startDay || curDay - startDay >= endDay)
				{
					newDay = new Day(numDay, isInThisMonth: false, date);
				}
				else
				{
					newDay = new Day(numDay, isInThisMonth: true, date);
				}
				Days.Add(newDay);
			}
		}
	}
	private void UpdateCalendarList(int startDay, int endDay, int month, int year)
	{

		for( int i = 0; i < DaysInMonthTable; i++ )
		{
			int numDay = i - startDay + OffsetToTheRussianCalendar;
			DateTime date = GetDate(numDay, month, year, startDay, endDay);

			if(i < startDay || i - startDay >= endDay)
			{
				Days[i].UpdateDay(numDay, isInThisMonth: false, date);
			}
			else
			{
				Days[i].UpdateDay(numDay, isInThisMonth: true, date);
			}
		}
	}

	private int GetMonthStartDayOfWeek(int month, int year)
	{
		DateTime temp = new DateTime(year, month, 1);
		return (int)temp.DayOfWeek - 1 == -1 ? 6 : (int)temp.DayOfWeek - 1; //Where Вс == 0,ПН==1 ВТ==2 СР==3 ЧТ==4 ПТ==5 СБ==6;
		//-1: Sunday == -1, Saturday==5, if(-1) -  Вс == 6,ПН==0 ВТ==1 СР==2 ЧТ==3 ПТ==4 СБ==5;
	}
	private int GetTotalNumberOfDays(int month, int year)
	{
		return DateTime.DaysInMonth(year, month);
	}

	private DateTime GetDate(int day, int month, int year, int startDay, int endDay)
	{
		if(day < StartMonthDate || day > endDay)
		{
			return DateTime.UnixEpoch;
		}
		return new DateTime(year, month, day);
	}

}