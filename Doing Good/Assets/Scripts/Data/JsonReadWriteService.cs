using System;
using System.IO;
using UnityEngine;

namespace Data
{
	public class JsonReadWriteService : IService
	{
		private const string FileName = "DoingGood_";
		private const string Extension = ".json";

		private const string TotalScoreFileName = "DoingGood_TotalScore";

		public void SaveToJson<T>(T toSave, string date)
		{

			string content = JsonUtility.ToJson(toSave);
			WriteToFile(GetPath(FileName, date), content);
		}

		public DataPerDay ReadFromJson(string date)
		{
			string content = ReadFile(GetPath(FileName, date));

			if(string.IsNullOrEmpty(content) || content == "{}")
			{
				return default( DataPerDay );
			}
			DataPerDay res = JsonUtility.FromJson<DataPerDay>(content);
			return res;
		}

		public void SaveTotalScoreToFile(int scoreToSave)
		{
			WriteToFile(GetPath(TotalScoreFileName), scoreToSave.ToString());
		}

		public int ReadTotalScoreFromJson()
		{
			string content = ReadFile(GetPath(TotalScoreFileName));

			if(string.IsNullOrEmpty(content) || content == "{}")
			{
				return default( Int32 );
			}
			return Int32.Parse(content);
		}

		private string ReadFile(string path)
		{
			if(File.Exists(path))
			{
				using (StreamReader reader = new StreamReader(path))
				{
					string content = reader.ReadToEnd();
					return content;
				}
			}
			return String.Empty;
		}

		private void WriteToFile(string path, string content)
		{
			FileStream fileStream = new FileStream(path, FileMode.Create);

			using (StreamWriter writer = new StreamWriter(fileStream))
			{
				writer.WriteAsync(content);
			}
		}
		private string GetPath(string fileName, string date = "")
		{
			return $"{Application.persistentDataPath}/{fileName}_{date}{Extension}";

		}

	}


}