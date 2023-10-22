using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class DataPerDay:ICloneable
	{
		public string Date;
		public List<ActionData> ActionDataList;
	[field: NonSerialized]	public int TotalScore { get; set; }

		private const int BaseCountActionData = 10;
		public DataPerDay(string date)
		{
			Date = date;
			ActionDataList = new List<ActionData>();

			for( int i = 0; i < BaseCountActionData; i++ )
			{
				AddNewActionData();
			}
		}




		public ActionData AddNewActionData()
		{
			ActionData newActionData = new ActionData(entryNumber: ActionDataList.Count + 1);
			ActionDataList.Add(newActionData);
			return newActionData;
		}
		

		public DateTime GetDateAsDateTime()
		{
			string[] splitDate = Date.Split(".");
			List<int> intDates = new List<int>();

			for( int i = 0; i < splitDate.Length; i++ )
			{
				int intResult = 0;
				Int32.TryParse(splitDate[i], out intResult);
				intDates.Add(intResult);
			}

			DateTime dateTime = new DateTime(intDates[2], intDates[1], intDates[0]);
			return dateTime;
		}
		
		
		public object Clone()
		{
			return new DataPerDay(Date)
			{
				ActionDataList = new List<ActionData>(ActionDataList), TotalScore = TotalScore
			};
		}
	}
}