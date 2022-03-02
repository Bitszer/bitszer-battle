using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class FileIO
{
	public static void WriteTextToPersistentDataFile(string filePath, string text)
	{
		if (!File.Exists (filePath)) {
			StreamWriter sw = File.CreateText (filePath);
			sw.WriteLine (text);
			sw.Close ();
			Debug.Log ("-- File Created and Text Wrote --");
		} else {
			File.WriteAllText (filePath, text);
			Debug.Log ("-- File Already Exist --");
			Debug.Log ("-- File Overwritten --");
		}
	}

	public static string ReadTextFromPersistentDataFile(string filePath)
	{
		if (File.Exists (filePath)) {
			StreamReader sr = new StreamReader (filePath);
			return sr.ReadToEnd ();
		} else {
			Debug.Log ("-- File doesn't Exist --");
			return "";
		}
	}

	public static void SavePNGtoPersistentDataPath(Texture2D tex, string path)
	{
		Byte[] _byte = tex.EncodeToPNG ();
		File.WriteAllBytes (path, _byte);
	}
}
