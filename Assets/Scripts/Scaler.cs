using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scaler : MonoBehaviour
{
	void Start()
	{
	    float width = ScreenSize.GetScreenToWorldWidth;
	    transform.localScale = Vector3.one * width;
	    GetComponent<SpriteRenderer>().flipX = true;
	    GetComponent<SpriteRenderer>().flipY = true;
	}
}
