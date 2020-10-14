using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;

public class Link : MonoBehaviour 
{

	public void OpenPretestSurvey()
	{
		#if !UNITY_EDITOR
		openWindow("https://ucsantacruz.co1.qualtrics.com/jfe/form/SV_3lAQYV4CwPrbcUZ");
		#endif
	}

	public void OpenPosttestSurvey()
	{
		#if !UNITY_EDITOR
		openWindow("https://www.marjcuerdo.com");
		#endif
	}

	[DllImport("__Internal")]
	private static extern void openWindow(string url);

}