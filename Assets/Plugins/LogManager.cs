using System.IO;
using UnityEngine;

public class LogManager {


	public static void SaveText(string dataPath, string txt, bool encoding){
		StreamWriter sw = new StreamWriter(GetPath () + dataPath, encoding); //true=追記 false=上書き
		sw.WriteLine(txt);
		sw.Flush();
		sw.Close();
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
}
