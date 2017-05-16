using UnityEngine;
using UnityEngine.UI;

public class TimerView : MonoBehaviour
{
	//それぞれインスタンスはインスペクタビューから設定

	[SerializeField] private TimeCounter timeCounter; 
	[SerializeField] private Text counterText; //uGUIのText

	void Start()
	{
		//タイマのカウンタが変化したイベントを受けてuGUI Textを更新する
		timeCounter.OnTimeChanged += time => // =>は「ラムダ式」と呼ばれる匿名関数の記法
		{
			//現在のタイマ値をUIに反映する
			counterText.text = time.ToString();
		};
	}
}