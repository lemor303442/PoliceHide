using System.IO;
using UnityEngine;

public class LogManager
{


	public static void SaveText (string dataPath, string txt, bool encoding)
	{
		
		string[] directries = dataPath.Split ('/');
		for (int i = 0; i < directries.Length - 1; i++) {
			string tmpPath = "";
			for (int j = 0; j < i + 1; j++) {
				tmpPath += directries [j] + "/";
			}
			tmpPath = tmpPath.Remove (tmpPath.Length - 1, 1);
			if (!Directory.Exists (GetPath () + tmpPath)) {
				Directory.CreateDirectory (GetPath () + tmpPath);
			}
		}

		string existingString = OpenTextFile (GetPath () + dataPath);
		if (existingString == "ERROR") {
			FileStream fs = new FileStream (GetPath () + dataPath, FileMode.CreateNew);
			StreamWriter sw = new StreamWriter (fs);
			sw.Write (txt);
			sw.Flush ();
			sw.Close ();
		}else{
			StreamWriter sw = new StreamWriter (GetPath () + dataPath, encoding); //true=追記 false=上書き
			sw.WriteLine (txt);
			sw.Flush ();
			sw.Close ();
		}
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
			StreamReader sr = new StreamReader (fi.OpenRead (), System.Text.Encoding.UTF8);
			returnSt = sr.ReadToEnd ();
			sr.Close ();
		} else {
			returnSt = "ERROR";
		}
		return returnSt;
	}
}
