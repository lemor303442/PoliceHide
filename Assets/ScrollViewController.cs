using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{

	Transform scrollTarget;
	RectTransform scrollTargetRect;
	float scrollViewWidth;

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
	[SerializeField]
	private int scrollPhase = 5;

	public Text text;

	void Start ()
	{
		scrollTarget = this.transform.FindChild ("Content");
		scrollTargetRect = scrollTarget.gameObject.GetComponent<RectTransform> ();
		scrollViewWidth = this.gameObject.GetComponent<RectTransform> ().rect.width;
		SetMovableDistance ();
	}

	void Update ()
	{
		text.text = scrollPhase.ToString();
		switch (scrollPhase) {
		case 0:
			//待機中
			if (Input.GetMouseButton (0))
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
		float moveDistance = ((touchingPos.x - preTouchingPos [frameDifference - 1].x) / frameDifference) * scrollVelocity * scrollOverRatio;
		scrollTarget.position += new Vector3 (moveDistance, 0, 0);
	}

	void ScrollTargetBack ()
	{
		SetScrollOverRatio ();
		float scrollBackRatio = 1 - scrollOverRatio;
		float scrollBackSpeed = scrollViewWidth / 1.3f;
		float moveDistance;
		if (scrollTargetRect.localPosition.x > 0) {
			moveDistance = -scrollBackRatio * scrollBackSpeed * Time.deltaTime;
		} else {
			moveDistance = scrollBackRatio * scrollBackSpeed * Time.deltaTime;
		}
		scrollTarget.position += new Vector3 (moveDistance, 0, 0);
	}

	void SetDecelateRatio (int frameDifference)
	{
		//DecelateRatioは0.3~0.9が理想
		float minDecelateRatio = 0.3f;
		float maxDecelateRatio = 0.9f;
		//endSpeedは20をMAXとする
		float endSpeed = Mathf.Abs ((touchingPos.x - preTouchingPos [frameDifference - 1].x) / frameDifference);
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
		GridLayoutGroup targetGridLayoutGroup = scrollTarget.GetComponent<GridLayoutGroup> ();
		float paddingLeft = targetGridLayoutGroup.padding.left;
		float paddingRight = targetGridLayoutGroup.padding.right;
		float cellSizeX = targetGridLayoutGroup.cellSize.x;
		float spacingX = targetGridLayoutGroup.spacing.x;
		int numberOfChilds = scrollTarget.childCount;

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
}
