using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollViewTouchInput
{
	//読み取るための値
	public bool isScrollable{ get; private set;}
	public bool IsButtonFuncEnable{ get; private set; }


	//受け取る値
	public float screenToCanvasRatio{ private get; set; }

	public Vector2 canvasSize{ private get; set; }

	public RectTransform targetRect{ private get; set; }
	//afforTime以内にaffordSize以上の動きがあったら
	//StartScroll = trueを返す
	//afforTime終了に、affordSize以上の動きがあったら
	//StartButtonFunc = true を返す
	public float affordTime{ private get; set; }

	public Vector2 affordSize{ private get; set; }

	//ローカル変数
	private float touchingTimer;
	private bool isTouching;
	Vector2 touchedPos;



	public void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			touchedPos = Input.mousePosition;
			isTouching = IsInTargetRect (touchedPos);
			touchingTimer = 0;
		}
		if (isTouching) {
			touchingTimer += Time.deltaTime;
			if (touchingTimer < affordTime) {
				if (IsInAfforSize (Input.mousePosition, touchedPos) == false) {
					isScrollable = true;
				}
			} else {
				IsButtonFuncEnable = true;
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			isTouching = false;
			isScrollable = false;
			IsButtonFuncEnable = false;
		}
	}

	private bool IsInAfforSize (Vector2 mousePos, Vector2 touchedPos)
	{
		Vector2 touchingCanvasPos = ScreenToCanvasPos (mousePos);
		Vector2 touchedCanvasPos = ScreenToCanvasPos (touchedPos);
		if (touchingCanvasPos.x > touchedCanvasPos.x - affordSize.x && touchingCanvasPos.x < touchedCanvasPos.x + affordSize.x) {
			if (touchingCanvasPos.y > touchedCanvasPos.y - affordSize.y && touchingCanvasPos.y < touchedCanvasPos.x + affordSize.y) {
				return true;
			}
		}
		return false;
	}


	private bool IsInTargetRect (Vector2 mousePos)
	{
		float minX = targetRect.localPosition.x;
		float maxX = minX + targetRect.sizeDelta.x;
		float minY = targetRect.localPosition.y - targetRect.sizeDelta.y / 2 + canvasSize.y / 2;
		float maxY = minY + targetRect.sizeDelta.y;
		Vector2 posOnCanvas = ScreenToCanvasPos (mousePos);
		if (posOnCanvas.x > minX && posOnCanvas.x < maxX) {
			if (posOnCanvas.y > minY && posOnCanvas.y < maxY) {
				return true;
			}
		}
		return false;
	}

	private Vector2 ScreenToCanvasPos (Vector2 inputPos)
	{
		Vector2 canvasCenter = new Vector2 (canvasSize.x / 2, canvasSize.y / 2);
		Vector2 screenCenter = new Vector2 (Screen.width / 2, Screen.height / 2);
		Vector2 posOnCanvas = (inputPos - screenCenter) * screenToCanvasRatio + canvasCenter;
		return posOnCanvas;
	}
}
