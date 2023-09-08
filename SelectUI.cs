// ======================================
// セレクト画面のUIcs
// ======================================
// こうづき

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SelectUI : MonoBehaviour
{
	[Header("自動で検索してリストに追加するかどうか")]
	[SerializeField] bool AutoOnOff = false;	// true : 自動	だが手動の方がそりゃ軽い、が、手間

	[Header("ステージ画像リスト")]
	[SerializeField] public int MaxStage = 0;
	[SerializeField] public List<Sprite> StageSpriteList;

	[Header("ステージナンバー画像配列")]
	[SerializeField] public Sprite[] StageNumtexList;

	[Header("-進行状況リスト-----")]
	[Header("クリア済みか")]
	[SerializeField] public List<bool> StageClearList;
	[Header("選択可能か")]
	[SerializeField] public List<bool> SelectStageList;
	[Header("-------------------")]

	[Header("ステージ Path")]
	[SerializeField] public string path = "Stagefolder/stage";
	[SerializeField] public string jsonpath = "/Select/Resources/Stagefolder/stage";
	
	[Header("オブジェクト")]
	[SerializeField] public Image thumbnailObj;
	[SerializeField] public Image stagenameObj;
	[SerializeField] public GameObject[] stageNumObj = new GameObject[2]; // ステージ番号オブジェクト(桁で変わる表示用) 
	[SerializeField] public Image[] stageNumImage = new Image[3];	// ステージ番号
	
	[Header("選択中のステージ番号")]
	[SerializeField,Range(0,48)] public int SelectNum = 0;

	[Header("以前選択されていたステージ番号")]
	[SerializeField] public int SelectNumOld = 0;

	[Header("チュートリアルかどうか")]
	[SerializeField] public bool isTutorial = false;
	[SerializeField] public Image Tutorial_name;

	[Header("保存される数値")]
	[SerializeField] public int jsonnum = 0;
	[SerializeField] public bool jsonbool = false;
	[SerializeField] public StageClear jsonData = new StageClear();

	#region キラキラ
	[Header("キラキラ")]
	[SerializeField] public GameObject[] glitterObj = new GameObject[2];
	[SerializeField] public float glittertime = 3.0f;
	#endregion
	#region -クリア系-
	[Header("クリア")]
	[SerializeField] public GameObject ClearObj;
	[SerializeField] public GameObject ClearImage;
	#endregion
	#region -NEW系-
	[Header("NEW")]
	[SerializeField] public GameObject NewObj;
	[SerializeField] public GameObject NewImage;
	#endregion
	#region -選択不可系-
	[Header("選択不可")]
	[SerializeField] public GameObject NotSelectObj;
	[SerializeField] public GameObject PadLockImage;    // 南京錠画像

	[Header("南京錠拡大")]
	[SerializeField] public int Scaletime = 60;
	[SerializeField] public Vector3 MaxScale = new Vector3(1.25f, 1.25f, 1);
	[SerializeField] public Vector3 AddScale = new Vector3(1,1,0);

	[Header("南京錠回転")]
	[SerializeField] public int Rotetime = 60;
	[SerializeField] public Vector3 Roterange = new Vector3(0,0,1);
	[SerializeField] public SESoundData.SE SE_Lock;

	[Header("南京錠解除")]
	[SerializeField] public GameObject UnlockObj;
	[SerializeField] public GameObject[] ImageQuad_Unlock;
	[SerializeField] public GameObject UnlockEfect;
	[SerializeField] public SESoundData.SE SE_UnLock;
	[SerializeField] public float Unlock_Alpha = 0.9f;
	[SerializeField] public float Unlock_Delay;
	#endregion
	
	#region デバッグ
	[Header("デバッグモード")]
	// [SerializeField] public GameObject player;
	[SerializeField] public GameObject DebugObj;
	[SerializeField] public bool isDebug = false;
	[SerializeField] public Text SelectNum_txtObj;
	[SerializeField] public Text StageClear_txtObj;
	bool save = false;
	private int a = 0;
	#endregion

	// ==================================================
	// Clearしたかどうかの判断用クラス
	// 一括で判断出来たら最高だが自分の技術が足りていないので
	// 個々で持って貰う
	#region セーブデータ※使えない
	[System.Serializable]
	public class StageClear
	{
		public bool isclear;
	}

	[System.Serializable]
	public class C_StageClearList
	{
		public bool[] isclearList;
	}

	// セーブ関数
	public void SaveStageData(StageClear stage,int num)
	{
	//	StreamWriter writer;

		//ステージデータをjsonに変換
		string jsonstr = JsonUtility.ToJson(stage);

		//jsonファイルに書き込み
	//	writer = new StreamWriter(Application.dataPath + jsonpath + num.ToString() + "/stage" + num.ToString() +".json", false);
		File.WriteAllText(Application.dataPath + jsonpath + num.ToString() + "/stage" + num.ToString() + ".json", jsonstr, System.Text.Encoding.UTF8);
		//writer.Write(jsonstr);
		//writer.Flush();
		//writer.Close();
	}

	//  ロード関数
	public StageClear loadStageData(int num)
	{
		//string datastr = "";
		//StreamReader reader;
		//reader = new StreamReader(Application.dataPath + jsonpath + num.ToString() + "/stage" + num.ToString() + ".json");
		//datastr = reader.ReadToEnd();
		//reader.Close();

		//return JsonUtility.FromJson<StageClear>(datastr);
		StageClear Uncode = JsonUtility.FromJson<StageClear>(File.ReadAllText(Application.dataPath + jsonpath + num.ToString() + "/stage" + num.ToString() + ".json"));
		return Uncode;
	}
	#endregion
	// ==================================================



	// Start is called before the first frame update
	void Start()
    {
		// Clearしてなかったら半透明の画像表示
		NotSelectObj.SetActive(true);

		UnlockObj.SetActive(false);
		UnlockEfect.SetActive(false);

		{
			//// 自動でResourceフォルダから探す
			//if (AutoOnOff == true)
			//{
			//	for (int i = 0; i < MaxStage; i++)
			//	{
			//		StageClear stage = loadStageData(i);
			//		StageSpriteList.Insert(i, Resources.Load<Sprite>(path + i.ToString() + "/stage" + i.ToString()));
			//		StageNameList.Insert(i, Resources.Load<Sprite>(path + i.ToString() + "/stagename" + i.ToString()));
			//		StageClearList.Insert(i, stage.isclear);
			//	}
			//}
		}

	}

    // Update is called once per frame
    void Update()
    {

		// デバッグ時表示
		DebugObj.SetActive(isDebug);
		if(isDebug)
		{
			SelectNum_txtObj.text = SelectNum.ToString();
			StageClear_txtObj.text = StageClearList[SelectNum].ToString();
		}

		// if (Input.GetKeyDown(KeyCode.I)) { StartSelectStage(); }
		// if (Input.GetKeyDown(KeyCode.I)) { a++; SetSelectStageNum(a); }
		// if (Input.GetKeyDown(KeyCode.K)) { SetStartUnlockObj(player.transform.position); }
		// if (Input.GetKeyDown(KeyCode.K)) { a--; SetSelectStageNum(a); }

		if (save == true)
		{
			for (int i = 0; i < MaxStage; i++)
			{
				StageClear stage = new StageClear();
				stage.isclear = StageClearList[i];
				SaveStageData(stage, i);
				Debug.Log("完了");
			}
			save = false;
		}

		// セレクトステージ表示
		ShowSelectUI(SelectNum);

		// 解放してるかどうか
		if (SelectStageList[SelectNum] && !StageClearList[SelectNum])
		{
			// 解放済みでクリアしてなかったら
			NewObj.SetActive(true);
		}
		else { NewObj.SetActive(false); }
		NotSelectObj.SetActive(!SelectStageList[SelectNum]);
		ClearObj.SetActive(StageClearList[SelectNum]);

		glittertime -= Time.deltaTime;
		if (glittertime < 0)
		{
			glittertime = 3.0f;
			// クリアしていたら
			if(StageClearList[SelectNum])
			{
				StartCoroutine(Glitter(0));
				return;
			}
			if(SelectStageList[SelectNum])
			{
				StartCoroutine(Glitter(1));
			}
		}

	}

	// 解除オブジェクトの座標指定
	public void SetStartUnlockObj(Vector3 pos)
	{
		UnlockObj.SetActive(true);
		UnlockObj.transform.localPosition = pos;
		StartCoroutine(Unlock());
	}

	// 選択中のステージを実行
	public void StartSelectStage()
	{
		// 選択可能なら入る
		if(SelectStageList[SelectNum] == true)
		{
			// ステージ呼び出し
		}
		else
		{
			// 実行不可演出
			StartLockRote();
			SelectNumOld = SelectNum;
		}
	}

	// 選択中のステージ番号セッター
	public void SetSelectStageNum(int num)
	{
		int i = num;
		if(num < 0) { i = 0; }
		if(num > MaxStage-1) { i = MaxStage-1; }
		SelectNum = i;
	}
	// 選択中のステージ番号ゲッタｧｧｧｧｧｧ・・・ﾋﾞｨｨｨｨｨﾑｯ！！
	public int GetSelectStageNum()
	{
		return SelectNum;
	}

	// セレクトステージの表示
	// num : 選択された番号
	public void ShowSelectUI(int num)
	{
		// もしも 0 ならばチュートリアル
		//if (num == 0) { isTutorial = true; }
		//else { isTutorial = false; }
		//
		num += 1;
		// 一桁目、二桁目
		int digit_1 = num % 10;
		int	digit_2 = num / 10;

		// 二桁目
		stageNumImage[1].sprite = StageNumtexList[digit_2];

		// 一桁目
		stageNumImage[0].sprite = stageNumImage[2].sprite = StageNumtexList[digit_1];

		// チュートリアル名
		Tutorial_name.gameObject.SetActive(isTutorial);

		// チュートリアルじゃなければ
		if (isTutorial == false)
		{
			// 二桁だったなら
			if (digit_2 != 0)
			{
				// 二桁表示
				stageNumObj[1].gameObject.SetActive(true);
				// 一桁非表示
				stageNumObj[0].gameObject.SetActive(false);
			}
			else
			{
				// 一桁表示
				stageNumObj[0].gameObject.SetActive(true);
				// 二桁非表示
				stageNumObj[1].gameObject.SetActive(false);
			}
		}
		else
		{   // チュートリアルならば

			// 二桁非表示
			stageNumObj[1].gameObject.SetActive(false);
			// 一桁非表示
			stageNumObj[0].gameObject.SetActive(false);
		}

		if (StageSpriteList[num] == null) { return; }
	//	if(StageNameList[num] == null) { return; }

		thumbnailObj.sprite = StageSpriteList[num];
	//	stagenameObj.sprite = StageNameList[num];

	}

	// ベジエ
	public Vector3 Bezier(Vector3 start, Vector3 end, Vector3 relay, float t)
	{
		Vector3 line_1 = Vector3.Lerp(start, relay, t);
		Vector3 line_2 = Vector3.Lerp(relay, end, t);
		Vector3 bezier = Vector3.Lerp(line_1, line_2, t);
		return bezier;
	}

	public void StartLockRote()
	{
		StartCoroutine(rote());
		// チュートリアル以外で以前選択した番号と同じなら
	//	if (SelectNum != 0 && SelectNum == SelectNumOld) { };

	}

	public IEnumerator rote()
	{
		int i = 0;

		// 1,1,1の差
		Vector3 difference = new Vector3((MaxScale.x - 1.0f)/Scaletime, (MaxScale.y - 1.0f)/Scaletime, 0.0f);
		Vector3 rote_2 = new Vector3(Roterange.x, Roterange.y, -(Roterange.z * 2));

		SoundManager.Instance.PlaySE(SE_Lock);

		while (i < Scaletime)
		{
			i++;
			
			//Scalerange = new Vector3()
			PadLockImage.transform.localScale += difference;
			yield return null;
		}
		i = 0;
		while(i < Rotetime)
		{
			i++;
			PadLockImage.transform.Rotate(Roterange);

			yield return null;
		}
		i = 0;
		while (i < Rotetime)
		{
			i++;
			PadLockImage.transform.Rotate(rote_2);

			yield return null;
		}
		i = 0;
		while (i < Rotetime)
		{
			i++;
			PadLockImage.transform.Rotate(Roterange);

			yield return null;
		}
		// 元の位置へ
		PadLockImage.transform.localEulerAngles = Vector3.zero;

		//// 999回連打された
		//if(isSecretOn == true && PunchCount == PunchMax)
		//{
		//	PunchCount = 0;
		//	SelectStageList[SelectNum] = true;
		//	StartCoroutine(StartSecret());
		//}
	}

	// アンロック
	public IEnumerator Unlock()
	{
		// 初期化＆変数宣言
		float f = 0.0f;
		ImageQuad_Unlock[0].SetActive(false);
		ImageQuad_Unlock[1].SetActive(true);
		ImageQuad_Unlock[1].GetComponent<Renderer>().material.color = Color.white;

		// 解除音＆南京錠透明化(1割)
		SoundManager.Instance.PlaySE(SE_UnLock);
		ImageQuad_Unlock[1].GetComponent<Renderer>().material.color = new Color(1,1,1, Unlock_Alpha);
		UnlockEfect.SetActive(true);

		// 若干待機
		yield return new WaitForSeconds(Unlock_Delay);

		// 段々透明化
		while(f < Unlock_Alpha)
		{
			f += 0.002f;
			ImageQuad_Unlock[1].GetComponent<Renderer>().material.color = new Color(1, 1, 1, Unlock_Alpha - f);

			yield return null;
		}

		ImageQuad_Unlock[1].GetComponent<Renderer>().material.color = new Color(1,1,1,0);
		ImageQuad_Unlock[1].SetActive(false);
		ImageQuad_Unlock[1].GetComponent<Renderer>().material.color = Color.white;
	}

	// キラキラ
	public IEnumerator Glitter(int num)
	{
		float f = -135;
		while(f < 140.0f)
		{
			f += 2.0f;
			glitterObj[num].transform.localPosition = new Vector3(f, 0, 0);
			yield return null;
		}
		glitterObj[num].transform.localPosition = new Vector3(-135, 0, 0);
	}

	//public IEnumerator StartSecret()
	//{
	//	float a = 0.0f;
	//	while (a < 1.0f)
	//	{
	//		a += 0.01f;
	//		PadLockImage.transform.Rotate(0, 0, 1);
	//		PadLockImage.GetComponent<Image>().color = new Color(1,1,1,1-a);
	//		PadLockImage.transform.position = Bezier(obj1.transform.position, obj3.transform.position, obj2.transform.position, a);
	//		yield return null;
	//	}
	//	PadLockImage.transform.position = obj1.transform.position;
	//	PadLockImage.transform.localEulerAngles = Vector3.zero;
	//}

	// デバック類 ---
	#region デバック用
	// 指定されたステージをクリア状態にする
	public void Debug_StageClear()
	{
		if(StageClearList.Count<SelectNum)
		{
			return;
		}

		StageClearList[SelectNum] = true;
	}

	// 指定されたステージを未クリア状態にする
	public void Debug_Not_StageClear()
	{
		if (StageClearList.Count < SelectNum)
		{
			return;
		}

		StageClearList[SelectNum] = false;
	}

	// 全てステージをクリア状態にする
	public void Debug_AllStageClear()
	{
		for(int i = 0; i < StageClearList.Count; i++)
		{
			StageClearList[i] = true;
		}
	}

	// 全てステージを未クリア状態にする
	public void Debug_Not_AllStageClear()
	{
		for (int i = 0; i < StageClearList.Count; i++)
		{
			StageClearList[i] = false;
		}
	}

	// 現在のステージ進行状況を保存(デバッグ用)
	public void Debug_SaveJson()
	{
		save = true;
	}
	#endregion

}
