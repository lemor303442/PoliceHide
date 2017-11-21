using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Text;


public class csvManager
{
	/// <summary>
	/// Resourcesフォルダ内のcsvファイルを読み込み、string型2次元配列で返す。
	/// </summary>
	public static string[,] ReadCsvFile (string dataPath)
	{
		string data;
		string textData = OpenTextFile (GetPath () + dataPath);
		if (textData != "ERROR") {
			data = textData;
		} else {
			TextAsset textAsset = (TextAsset)Resources.Load (dataPath.Split ("." [0]) [0]);
			data = textAsset.ToString ();
		}

		if (data == null) {
			return null;
		}
		string[] rows = data.Replace ("\r\n", "\n").Split ('\n');
		int dataRows = rows.Length;
		int dataCols = rows [0].Split ("," [0]).Length;
		string[,] csvData = new string[dataRows, dataCols];
		for (int i = 0; i < csvData.GetLength (0); i++) {
			for (int j = 0; j < csvData.GetLength (1); j++) {
				string[] value = rows [i].Split ("," [0]);
				csvData [i, j] = value [j];
			}
		}
		return csvData;
	}


	/// <summary>
	/// Resourcesフォルダ内に、ファイルを書き込む。
	/// 例) csvManager.WriteData("data.csv", dataArray);
	/// </summary>
	public static void WriteData (string dataPath, string[,] newData)
	{
		string stringData = "";
		for (int i = 0; i < newData.GetLength (0); i++) {
			for (int j = 0; j < newData.GetLength (1); j++) {
				if (j < newData.GetLength (1) - 1) {
					stringData += newData [i, j] + ",";
				} else if (j == newData.GetLength (1) - 1 && i < newData.GetLength (0) - 1) {
					stringData += newData [i, j] + "\n";
				} else {
					stringData += newData [i, j];
				}
			}
		}
		Debug.Log (GetPath () + dataPath);
		FileStream fs = new FileStream (GetPath () + dataPath, FileMode.Create);
		StreamWriter sw = new StreamWriter (fs);
		sw.Write (stringData);
		sw.Flush ();
		sw.Close ();
	}


	public static string GetPath ()
	{
		#if UNITY_EDITOR
		return Application.dataPath + "/Resources/";
		#elif UNITY_ANDROID
		return Application.persistentDataPath + "/";
		#elif UNITY_IPHONE
		return Application.persistentDataPath + "/";
		#else
		return Application.dataPath + "";
		#endif
	}

	public static string OpenTextFile (string _filePath)
	{
		FileInfo fi = new FileInfo (_filePath);
		string returnSt = "";
		if (fi.Exists) {
			StreamReader sr = new StreamReader (fi.OpenRead (), Encoding.UTF8);
			returnSt = sr.ReadToEnd ();
			sr.Close ();
		} else {
			returnSt = "ERROR";
		}
		return returnSt;
	}
}
