using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Data
{
	[System.Serializable]
	public class ActionData
	{
		private const int First = 1;

		public ActionData(int entryNumber)
		{
			EntryNumber = entryNumber;
		}

		public int EntryNumber;
		public string ActionName = string.Empty;
		public int ActionScore = Int32.MinValue;


		public bool IsEmptyData() => IsEmptyActionName() && IsEmptyScore();
		public bool IsEmptyActionName() => String.IsNullOrWhiteSpace(ActionName);
		public bool IsEmptyScore() => ActionScore == Int32.MinValue;
		public bool IsFirst() => EntryNumber == First;

	}
}