using System;

public class Day
{
	private int _dayNum;
	private bool _isInThisMonth;
	private DateTime _dateOfDay;

	public Day(int dayNum, bool isInThisMonth, DateTime dateOfDay)
	{
		UpdateDay(dayNum, isInThisMonth, dateOfDay);
	}
	public void UpdateDay(int dayNum, bool isInThisMonth, DateTime dateOfDay)
	{
		_dayNum = dayNum;
		_isInThisMonth = isInThisMonth;
		_dateOfDay = dateOfDay;
	}

	public string GetDateOfDay() => _dateOfDay.ToString("dd.MM.yyyy");
	public int DayNum => _dayNum;
	public bool IsInThisMonth => _isInThisMonth;
	public bool IsToday { get; set; }
}