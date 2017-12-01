﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class IntUnityEvent : UnityEvent<int>
{
}

//scrollViewに対する入力の取得、scrollViewを実際に動かす、ボタンの押した時のイベントを発火
public class ScrollViewController : MonoBehaviour
{
	public IntUnityEvent OnButtonDown = new IntUnityEvent ();
	public IntUnityEvent OnButtonUp = new IntUnityEvent ();



	RectTransform scrollTargetRect;
	float scrollViewWidth;

	float screenRatio;
	CanvasScaler canvasScaler;

	bool isButtonDown;
	bool isButtonUp;
	int pressedButtonIndex;


	Vector2 touchStartPos;
	Vector2 touchEndPos;
	float touchingTimer;
	float touchUpTimer;

	Vector2 touchingPos;
	Vector2[] preTouchingPos = new Vector2[5];

	private float scrollVelocity;
	private float scrollOverRatio;
	private bool is_scrollable = true;

	private float decelateRatio;

	private float movableDistance;

	/// <summary>
	/// The scroll phase.
	/// 0:待機中
	/// 1:タップした瞬間
	/// 2:タップ中
	/// 3:離した瞬間
	/// 4:離した後の移動
	/// 5:リセット
	/// </summary>
	private int scrollPhase = 5;

	public TouchDetectCalculator touchDetectCalculator = new TouchDetectCalculator ();


	void Start ()
	{
		canvasScaler = this.transform.GetComponentInParent<CanvasScaler> ();
		//set screen ratio;
		if (Screen.width * (canvasScaler.referenceResolution.y / canvasScaler.referenceResolution.x) <= Screen.height) {
			screenRatio = canvasScaler.referenceResolution.x / Screen.width;
		} else {
			screenRatio = canvasScaler.referenceResolution.y / Screen.height;
		}

		scrollTargetRect = this.transform.Find ("Content").GetComponent<RectTransform> ();
		scrollViewWidth = this.gameObject.GetComponent<RectTransform> ().rect.width;
		SetMovableDistance ();

		//ScrollViewTouchInputに初期値を渡す
		touchDetectCalculator.screenToCanvasRatio = screenRatio;
		touchDetectCalculator.canvasSize = new Vector2 (canvasScaler.referenceResolution.x, canvasScaler.referenceResolution.y);
		touchDetectCalculator.targetRect = this.gameObject.GetComponent<RectTransform> ();
		touchDetectCalculator.affordTime = 0.2f;
		touchDetectCalculator.affordSize = new Vector2 (30, 30);

		InstantiateAnimationButtons ();

//		OnButtonDown.Invoke(3);
	}


	void Update ()
	{
		touchDetectCalculator.Update ();
		EventFire ();

		Move();
	}

	void EventFire ()
	{
		if (touchDetectCalculator.isButtonFuncEnable && isButtonDown) {
			isButtonDown = false;
			OnButtonDown.Invoke (pressedButtonIndex);
		}
		if (!touchDetectCalculator.isButtonFuncEnable && isButtonUp) {
			isButtonUp = false;
			OnButtonUp.Invoke (pressedButtonIndex);
		}
	}

	void Move ()
	{
		switch (scrollPhase) {
		case 0:
			//待機中
			if (touchDetectCalculator.isScrollable)
				scrollPhase++;
			break;
		case 1:
			//タップした瞬間
			touchStartPos = Input.mousePosition;
			for (int i = 0; i < preTouchingPos.Length; i++) {
				preTouchingPos [i] = Input.mousePosition;
			}
			scrollPhase++;
			break;
		case 2:
			//タップ中
			touchingTimer += Time.deltaTime;
			touchingPos = Input.mousePosition;
			SetScrollOverRatio ();
			ScrollTarget (1);
			SetPreTouchingPos ();
			if (Input.GetMouseButtonUp (0))
				scrollPhase++;
			break;
		case 3:
			//離した瞬間
			scrollOverRatio = 1;
			touchEndPos = Input.mousePosition;
			SetDecelateRatio (preTouchingPos.Length);
			scrollPhase++;
			break;
		case 4:
			//離した後(内側）
			touchUpTimer += Time.deltaTime;
			float floatingTime = 0.3f;
			if (scrollTargetRect.localPosition.x < 0 && scrollTargetRect.localPosition.x > -movableDistance) {
				ScrollTarget (preTouchingPos.Length);
				scrollVelocity = Mathf.Pow (1 - touchUpTimer / floatingTime, 2);
				if (scrollVelocity < 0.1f) {
					scrollPhase++;
				}
			} else {
				ScrollTargetBack ();
				if (scrollOverRatio > 0.95f) {
					scrollPhase++;
				}
			}
			if (Input.GetMouseButtonDown (0)) {
				scrollPhase++;
			}
			break;
		case 5:
			//リセット
			touchingTimer = 0;
			touchUpTimer = 0;
			scrollVelocity = 1;

			if (Input.GetMouseButton (0)) {
				scrollPhase = 1;
			} else {
				scrollPhase = 0;
			}
			break;
		default:
			scrollPhase = 0;
			break;
		}
	}




	void ScrollTarget (int frameDifference)
	{
		float moveDistance = (((touchingPos.x - preTouchingPos [frameDifference - 1].x) * screenRatio) / frameDifference) * scrollVelocity * scrollOverRatio;
		scrollTargetRect.localPosition += new Vector3 (moveDistance, 0, 0);
	}

	void ScrollTargetBack ()
	{
		SetScrollOverRatio ();
		float scrollBackRatio = 1 - scrollOverRatio;
		float scrollBackSpeed;
		scrollBackSpeed = scrollViewWidth;
		float moveDistance;
		if (scrollTargetRect.localPosition.x > 0) {
			moveDistance = -scrollBackRatio * scrollBackSpeed * Time.deltaTime;
		} else {
			moveDistance = scrollBackRatio * scrollBackSpeed * Time.deltaTime;
		}
		scrollTargetRect.localPosition += new Vector3 (moveDistance, 0, 0);
	}

	void SetDecelateRatio (int frameDifference)
	{
		//DecelateRatioは0.3~0.9が理想
		float minDecelateRatio = 0.3f;
		float maxDecelateRatio = 0.9f;
		//endSpeedは20をMAXとする
		float endSpeed = Mathf.Abs (((touchingPos.x - preTouchingPos [frameDifference - 1].x) * screenRatio) / frameDifference);
		float maxEndSpeed = 10;
		endSpeed = Mathf.Clamp (endSpeed, 0, maxEndSpeed);
		decelateRatio = minDecelateRatio + endSpeed / maxEndSpeed * (maxDecelateRatio - minDecelateRatio);
	}

	void SetPreTouchingPos ()
	{
		for (int i = 0; i < preTouchingPos.Length; i++) {
			if (preTouchingPos.Length - (i + 1) == 0) {
				preTouchingPos [preTouchingPos.Length - (i + 1)] = Input.mousePosition;
			} else {
				preTouchingPos [preTouchingPos.Length - (i + 1)] = preTouchingPos [preTouchingPos.Length - (i + 2)];
			}
		}
	}

	void SetMovableDistance ()
	{
		GridLayoutGroup targetGridLayoutGroup = scrollTargetRect.gameObject.GetComponent<GridLayoutGroup> ();
		float paddingLeft = targetGridLayoutGroup.padding.left;
		float paddingRight = targetGridLayoutGroup.padding.right;
		float cellSizeX = targetGridLayoutGroup.cellSize.x;
		float spacingX = targetGridLayoutGroup.spacing.x;
		int numberOfChilds = scrollTargetRect.gameObject.transform.childCount;

		float scrollTargetWidth = cellSizeX * numberOfChilds + spacingX * (numberOfChilds - 1) + paddingLeft + paddingRight;
		movableDistance = scrollTargetWidth - scrollViewWidth;
	}

	void SetScrollOverRatio ()
	{
		float movableDis = scrollViewWidth / 4;
		if (scrollTargetRect.localPosition.x > 0) {
			scrollOverRatio = Mathf.Clamp ((movableDis - scrollTargetRect.localPosition.x) / movableDis, 0, 1);
			if ((touchingPos.x - preTouchingPos [1].x) < 0) {
				scrollOverRatio *= 1.5f;
				scrollOverRatio = Mathf.Clamp (scrollOverRatio, 0, 1);
			}
		} else if (scrollTargetRect.localPosition.x < -movableDistance) {
			float posDif = -(movableDistance + scrollTargetRect.localPosition.x);
			scrollOverRatio = Mathf.Clamp ((movableDis - posDif) / movableDis, 0, 1);
			if ((touchingPos.x - preTouchingPos [1].x) > 0) {
				scrollOverRatio *= 1.5f;
				scrollOverRatio = Mathf.Clamp (scrollOverRatio, 0, 1);
			}
		} else {
			scrollOverRatio = 1;
		}
	}



	private void InstantiateAnimationButtons ()
	{
		GameObject animationButton = Resources.Load ("Prefabs/AnimationButton") as GameObject;
		DataManager dataManager = GameObject.FindObjectOfType<DataManager> ();
		Transform animationButtonParent = this.transform.Find ("Content").transform;
		for (int i = 0; i < dataManager.enabledAnimationIds.Length; i++) {
			if (dataManager.enabledAnimationIds [i]) {
				GameObject eventClone = Instantiate (animationButton, animationButtonParent) as GameObject;
				var trigger = eventClone.GetComponent<EventTrigger> ();
				trigger.triggers = new List<EventTrigger.Entry> ();

				var eventPointerDown = new EventTrigger.Entry ();
				eventPointerDown.eventID = EventTriggerType.PointerDown;
				int buttonId = i + 1;
				eventPointerDown.callback.AddListener ((x) => ButtonDown (buttonId));
				trigger.triggers.Add (eventPointerDown);

				var eventPointerUp = new EventTrigger.Entry ();
				eventPointerUp.eventID = EventTriggerType.PointerUp;
				eventPointerUp.callback.AddListener ((x) => ButtonUp (buttonId));
				trigger.triggers.Add (eventPointerUp);
			}
		}
	}

	private void ButtonDown (int index)
	{
		isButtonDown = true;
		pressedButtonIndex = index;
	}

	private void ButtonUp (int index)
	{
		isButtonUp = true;
	}
}
