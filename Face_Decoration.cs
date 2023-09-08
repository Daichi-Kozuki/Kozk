using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;


public class Face_Decoration : MonoBehaviour
{
	[SerializeField] public DecBackButton backcs;
	[SerializeField] public save_dec save;

	public struct ACC
	{
		[SerializeField] public Color color;
		[SerializeField] public BODYTEX e_Body;
		[SerializeField] public ACC_1 e_Acc_1;
		[SerializeField] public ACC_2 e_Acc_2;
		[SerializeField] public ACC_3 e_Acc_3;
		[SerializeField] public Vector3Int ReleaseNum;	// �������Ă���ԍ��ێ��p (Acc1,Acc2,Acc3)
	}
	[SerializeField] public static ACC ACC_Data;

	public enum BODYTEX
	{
		NONE,
		MAX,
	}

	[Header("--- �{�f�B -----")]
	[SerializeField] public GameObject Player_Body;
	[SerializeField] public Texture[] Body_Tex;
	[SerializeField] public Material Body_Mat;

	[SerializeField] public GameObject BodyFront;
	[SerializeField] public Material[] FrontList;
	[SerializeField] public GameObject BodyLeft;
	[SerializeField] public Material[] LeftList;
	[SerializeField] public GameObject BodyRight;
	[SerializeField] public Material[] RightList;
	[SerializeField] public GameObject BodyBack;
	[SerializeField] public Material[] BackList;



	[Header("- �J���[�p���b�g ---")]
	[SerializeField] public GameObject Pallet_Original;
	[SerializeField] public List<GameObject> ColPallets = new List<GameObject>();
	[SerializeField] public Color[] PalletColors = new Color[36];
	[SerializeField] public Vector2Int ColPal_XY = new Vector2Int(9,4);
	[SerializeField] public GameObject PosObj_ColList;	//�J�n�n�_�p�I�u�W�F�N�g
	[SerializeField] public Vector2Int Pallet_xy = new Vector2Int(95,-100);
	[SerializeField] public GameObject Obj_PalletSelect;
	//[SerializeField] public GameObject Obj_PalletDecision;
	[SerializeField] public int PalletNum = 0;
//	[SerializeField] public int PalletDecNum = 0;
	[SerializeField] public Vector3 PalletNextPos;

	[Header("RGB�J���[")]
	[SerializeField, Range(0.0f, 1.0f)] public float Col_Red = 0.0f;
	[SerializeField] public Slider Slider_Red;
	[SerializeField, Range(0.0f, 1.0f)] public float Col_Green = 0.0f;
	[SerializeField] public Slider Slider_Green;
	[SerializeField, Range(0.0f, 1.0f)] public float Col_Blue = 0.0f;
	[SerializeField] public Slider Slider_Blue;

	[Header("--- �A�N�Z�T���[ -----")]
	[SerializeField] public GameObject Player_Acc_1;    // �\��Ɋ֌W����
	[SerializeField] public GameObject Player_Acc_2;    // �ዾ�Ȃ�
	[SerializeField] public GameObject Player_Acc_3;    // �ق��؂��Ȃ�

	[Header("������ԍ�")]
	[SerializeField] public Vector3Int Curry_ReleaseNum = new Vector3Int(999,999,999);

	public enum Dir
	{
		NONE = 0,
		UP,
		DOWN,
		LEFT,
		RIGHT,
		LU,
		RU,
		LD,
		RD,
		MAX,
	}
	[Header("--- ���́��R���g���[���[ -----")]
	[Header("�R���g���[���[")]
	// �R���g���[���[
	[SerializeField] public Gamepad pad = null;
	[SerializeField] public Dir dir = Dir.NONE;
	[SerializeField] public float LStickdead = 0.5f;        // L�X�e�B�b�N�̓|�������C���i�ȏ�ŔF���j
	[SerializeField] public bool Stick_OnOff = false;   // true:�쓮�\
	[SerializeField] public bool Button_OnOff = false;  // true:�쓮�s��
	[SerializeField] public bool R_Shoulder_OnOff = false;  // true:�쓮�s��
	[SerializeField] public bool L_Shoulder_OnOff = false;  // true:�쓮�s��
	public int ConA = 30;
	public int Con_A = 30;
	public int ConB = 120;
	public int Con_B = 120;
	bool stop = false;
	// 
	public enum Select_Acc
	{
		ACC_1,
		ACC_2,
		ACC_3,
		COLOR,
		MAX,
	}
	// �A�N�Z�T���[�P
	public enum ACC_1
	{
		NONE,
		GLASSES,
		_2,
		_3,
		_4,
		_5,
		_6,
		_7,
		_8,
		_9,
		_10,
		_11,
		_12,
		_13,
		_14,
		_15,
		_16,
		MAX,
	}
	// �A�N�Z�T���[�Q
	public enum ACC_2
	{
		NONE,
		CHEEKS_1,
		CHEEKS_2,
		CHEEKS_3,
		_4,
		_5,
		_6,
		_7,
		_8,
		_9,
		_10,
		_11,
		_12,
		_13,
		_14,
		_15,
		_16,
		_17,
		_18,
		_19,
		_20,
		_21,
		_22,
		_23,
		_24,
		_25,
		_26,
		_27,
		_28,
		MAX,
	}
	// �A�N�Z�T���[�R
	public enum ACC_3
	{
		NONE,
		_1,
		_2,
		_3,
		_4,
		_5,
		_6,
		_7,
		_8,
		_9,
		_10,
		_11,
		_12,
		_13,
		_14,
		_15,
		_16,
		_17,
		_18,
		_19,
		_20,

		MAX,
	}

	[SerializeField] public Material[] Acc_1_MatList;
	[SerializeField] public Material[] Acc_2_MatList;
	[SerializeField] public Material[] Acc_3_MatList;
	[SerializeField] public Material NoneMat;

	[Header("--- UI�\�� -----")]
	[Header("�e�I�u�W�F�N�g")]
	[SerializeField] public GameObject UI_Parent;
	[Header("�����������")]
	[SerializeField] public GameObject Obj_Original;

	[Header("- �I���� ---")]
	[Header("�I���I�u�W�F�N�g")]
	[SerializeField] public GameObject Obj_Select;
	[SerializeField] public GameObject Obj_Decision;
	[SerializeField] public Color Col;
	[Header("�I�𒆔ԍ�")]
	[SerializeField] public int SelectNum = 0;
	[Header("�������ԍ�")]
	[SerializeField] public Vector3Int DecNum;

	[Header("�I�𒆃O���[�v")]
	[SerializeField] public Select_Acc e_NowSelect = Select_Acc.ACC_1;
	[SerializeField] public Vector3 Nextpos;
	[SerializeField] public float speed = 1.0F;
	[SerializeField] public bool isMoving = false; // true : �ړ���

	[Header("- ����UI ---")]
	[SerializeField] public Image[] Majoritems;
	[SerializeField] public Image InvBgImage;
	[SerializeField] public Color itemCol;

	[Header("- ���X�g�� ---")]
	[Header("�q�����X�g")]
	[SerializeField] public List<GameObject> UI_ChildList = new List<GameObject>();
	[Header("�ő吔")]
	[SerializeField] public int MaxChildNum = 0;
	[Header("1��̌�")]
	public int X = 0;
	public int Y = 0;
	[Header("���� ��")]
	public int b = 0;
	[Header("���� �c")]
	public int c = 0;

	[Header("- �J���[ ---")]
	[SerializeField] public GameObject Obj_Color;
	[SerializeField] public bool isColor = false;
	[SerializeField, Range(0, 4)] public int mode = 0;
	[SerializeField] public Text[] ColTexts = new Text[3];
	[SerializeField] public Image[] SliderBgs = new Image[3];
	[SerializeField] public Material CarryMat;

	[Header("--- SE ---")]
	[Header("���艹")]
	[SerializeField] public SESoundData.SE SE_decision;
	[Header("�J�[�\���ړ���")]
	[SerializeField] public SESoundData.SE SE_cursor;
	[Header("�L�����Z����")]
	[SerializeField] public SESoundData.SE SE_cancel;


	// Start is called before the first frame update
	void Start()
    {
		// ���}�e���A��
		CarryMat.color = Body_Mat.color;

		// ���[
		curry();

		// �ۑ�����Ă��鑕���\��
		SetDecoration_SaveData();

		for (int y = 0; y < Y; y++)
		{
			for (int x = 0; x < X; x++)
			{
				UI_ChildList.Add(Instantiate(Obj_Original, new Vector3(0f, 0f, 0f), Quaternion.identity));
				UI_ChildList[y * X + x].transform.SetParent(UI_Parent.transform);
				UI_ChildList[y * X + x].GetComponent<RectTransform>().localPosition = new Vector3(x * b, y * c, 0);

				NowSelectAccUI(y * X + x);

				if((y * X + x) > ACC_Data.ReleaseNum.x)
				{
					Acc_1_MatList[y * X + x] = NoneMat;
				}
				if ((y * X + x) > ACC_Data.ReleaseNum.y)
				{
					Acc_2_MatList[y * X + x] = NoneMat;
				}
				if ((y * X + x) > ACC_Data.ReleaseNum.z)
				{
					Acc_3_MatList[y * X + x] = NoneMat;
				}
			}
		}
		MaxChildNum = UI_ChildList.Count;

		// ���݂̑區�ڂ̐F�ƃT�C�Y�ύX
		Majoritems[(int)e_NowSelect].color = itemCol;
		Majoritems[(int)e_NowSelect].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);

		// �������g
		InitDecision();

		for (int y = 0; y < ColPal_XY.y; y++)
		{
			for (int x = 0; x < ColPal_XY.x; x++)
			{
				ColPallets.Add(Instantiate(Pallet_Original));
				ColPallets[y * ColPal_XY.x + x].transform.SetParent(PosObj_ColList.transform);
				ColPallets[y * ColPal_XY.x + x].GetComponent<RectTransform>().localPosition = new Vector3(x * Pallet_xy.x, y * Pallet_xy.y, 0);
				ColPallets[y * ColPal_XY.x + x].transform.GetChild(0).GetComponent<Image>().color = PalletColors[y * ColPal_XY.x + x];
			}
		}

		// �R���g���[���[�擾
		if (pad == null)
		{
			pad = Gamepad.current;
		}
	}

	// Update is called once per frame
	void Update()
    {
		// �R���g���[���[�擾
		if (pad == null)
		{
			pad = Gamepad.current;
		}

		SetAcc();

		if (isColor) { Obj_Color.SetActive(true); }
		else { Obj_Color.SetActive(false); }

		if(backcs.Window_OnOff != true) { 
			// ���͌n -----------------
			#region ���͌n

			ConKeyDir();

			// �C���x���g���ړ�
		if ( isColor!=true && isMoving != true)
		{
			if (dir == Dir.LEFT){SelectNum--;}
			else if (dir == Dir.RIGHT){SelectNum++;}
			else if (dir == Dir.UP){SelectNum -= X;}
			else if (dir == Dir.DOWN){SelectNum += X;}
			else if(dir== Dir.LU){ SelectNum -= X + 1; }
			else if(dir== Dir.RU){ SelectNum -= X - 1; }
			else if(dir== Dir.LD){ SelectNum += X - 1; }
			else if(dir== Dir.RD){ SelectNum += X + 1; }

			if(SelectNum < 0) { SelectNum += UI_ChildList.Count; }
			else if (SelectNum >= UI_ChildList.Count) { SelectNum -= UI_ChildList.Count; }
		}
		else if(isColor==true)
		{
			switch(mode)
			{
				// ���[�h
				case 0: {
						if (dir == Dir.LEFT) { PalletNum--; SoundManager.Instance.PlaySE(SE_cursor); }
						else if (dir == Dir.RIGHT) { PalletNum++; SoundManager.Instance.PlaySE(SE_cursor); }
						else if (dir == Dir.UP)
						{
							PalletNum -= ColPal_XY.x;
							if (PalletNum < 0) {
								// ���[�h�`�F���W
								mode = 3;
								PalletNum += ColPallets.Count;
							}						
						}
						else if (dir == Dir.DOWN)
						{
							PalletNum += ColPal_XY.x;
							if (PalletNum >= ColPallets.Count)
							{
								// ���[�h�`�F���W
								mode = 1;
								PalletNum -= ColPal_XY.x;
								//PalletNum -= ColPallets.Count;
							}
						}
						else if (dir == Dir.LU)
						{
							SoundManager.Instance.PlaySE(SE_cursor);
							PalletNum -= ColPal_XY.x+1;
							if (PalletNum < 0)
							{
								// ���[�h�`�F���W
								mode = 3;
								PalletNum += ColPallets.Count;
							}
						}
						else if (dir == Dir.RU)
						{
							PalletNum -= ColPal_XY.x - 1;
							SoundManager.Instance.PlaySE(SE_cursor);
							if (PalletNum < 0)
							{
								// ���[�h�`�F���W
								mode = 3;
								PalletNum += ColPallets.Count;
							}
						}
						else if (dir == Dir.LD)
						{
							SoundManager.Instance.PlaySE(SE_cursor);
							PalletNum += ColPal_XY.x-1;
							if (PalletNum >= ColPallets.Count)
							{
								// ���[�h�`�F���W
								mode = 1;
								PalletNum -= ColPal_XY.x;
							}
						}
						else if (dir == Dir.RD)
						{
							SoundManager.Instance.PlaySE(SE_cursor);
							PalletNum += ColPal_XY.x + 1;
							if (PalletNum >= ColPallets.Count)
							{
								// ���[�h�`�F���W
								mode = 1;
								PalletNum -= ColPal_XY.x;
							}
						}
					
						SliderBgs[0].gameObject.SetActive(false);
						SliderBgs[1].gameObject.SetActive(false);
						SliderBgs[2].gameObject.SetActive(false);

						if (PalletNum < 0) { PalletNum += ColPallets.Count; }
						if (PalletNum >= ColPallets.Count) { PalletNum -= ColPallets.Count; }
						Color col = PalletColors[PalletNum];
						CarryMat.color = col;
						Debug.Log(col);
						//Slider_Red.value = CarryMat.color.r;
						//Slider_Green.value = CarryMat.color.g;
						//Slider_Blue.value = CarryMat.color.b;
						break;
					}
				case 1: {
						if (dir == Dir.UP) { mode = 0; }
						else if (dir == Dir.DOWN) { mode = 2; }
						if(dir == Dir.LEFT) { Slider_Red.value -= 1.0f / 255.0f; }
						if(dir == Dir.RIGHT){ Slider_Red.value += 1.0f / 255.0f; }
						SliderBgs[0].gameObject.SetActive(true);
						SliderBgs[1].gameObject.SetActive(false);
						SliderBgs[2].gameObject.SetActive(false);
					//	if(Slider_Red.value<=0|| Slider_Red.value >= 1.0f) { stop = true; }
						break;
					}
				case 2: {
						if (dir == Dir.UP) { mode = 1;  }
						else if (dir == Dir.DOWN) { mode = 3;  }
						if (dir == Dir.LEFT) { Slider_Green.value -= 1.0f / 255.0f; }
						if (dir == Dir.RIGHT) { Slider_Green.value += 1.0f / 255.0f; }
						SliderBgs[0].gameObject.SetActive(false);
						SliderBgs[1].gameObject.SetActive(true);
						SliderBgs[2].gameObject.SetActive(false);
						break;
					}
				case 3: {
						if (dir == Dir.UP) { mode = 2; }
						else if (dir == Dir.DOWN) { mode = 0; PalletNum -= ColPallets.Count - ColPal_XY.x; }
						if (dir == Dir.LEFT) { Slider_Blue.value -= 1.0f / 255.0f; }
						if (dir == Dir.RIGHT) { Slider_Blue.value += 1.0f / 255.0f; }
						SliderBgs[0].gameObject.SetActive(false);
						SliderBgs[1].gameObject.SetActive(false);
						SliderBgs[2].gameObject.SetActive(true);
						break;
					}
			}

			if(mode != 0) { Obj_PalletSelect.SetActive(false); CarryMat.color = Body_Mat.color; }
			else { Obj_PalletSelect.SetActive(true); }


			PalletNextPos = ColPallets[PalletNum].GetComponent<RectTransform>().position;
			MoveSelectColpal();
		}


			// �區�ڂ̑I��
			if (pad != null){ 
				if (L_Shoulder_OnOff != true && (Input.GetKeyDown(KeyCode.LeftShift) || pad.leftShoulder.isPressed))
		{
			// L1�����ꂽ
			L_Shoulder_OnOff = true;
			ChangeMajorItem(false);
			e_NowSelect--;
			mode = 0;
			if (e_NowSelect < 0)
			{
				e_NowSelect = Select_Acc.MAX - 1;
			}
			ChangeMajorItem(true);
		}
				else if (R_Shoulder_OnOff != true && (Input.GetKeyDown(KeyCode.RightShift) || pad.rightShoulder.isPressed))
		{
			// R1�����ꂽ
			R_Shoulder_OnOff = true;
			ChangeMajorItem(false);
			e_NowSelect++;
			mode = 0;
			if (e_NowSelect > Select_Acc.MAX - 1)
			{
				e_NowSelect = Select_Acc.ACC_1;
			}
			ChangeMajorItem(true);
		}
				if(!pad.rightShoulder.isPressed) { R_Shoulder_OnOff = false; }
				if(!pad.leftShoulder.isPressed) { L_Shoulder_OnOff = false; }
			}
			else if(pad == null)
			{
				if (L_Shoulder_OnOff != true && Input.GetKeyDown(KeyCode.LeftShift))
				{
					// L1�����ꂽ
					//L_Shoulder_OnOff = true;
					ChangeMajorItem(false);
					e_NowSelect--;
					mode = 0;
					if (e_NowSelect < 0)
					{
						e_NowSelect = Select_Acc.MAX - 1;
					}
					ChangeMajorItem(true);
				}
				else if (R_Shoulder_OnOff != true && Input.GetKeyDown(KeyCode.RightShift))
				{
					// R1�����ꂽ
				//	R_Shoulder_OnOff = true;
					ChangeMajorItem(false);
					e_NowSelect++;
					mode = 0;
					if (e_NowSelect > Select_Acc.MAX - 1)
					{
						e_NowSelect = Select_Acc.ACC_1;
					}
					ChangeMajorItem(true);
				}
			}
			// ����
			if ((Input.GetButtonDown("Decision") && !Button_OnOff) || Input.GetKeyDown(KeyCode.Return))
			{
				Debug.Log("������ă�");
				Button_OnOff = true;
				switch (e_NowSelect)
				{
					case Select_Acc.ACC_1:
						{
							if (SelectNum < (int)ACC_1.MAX) { 
								DecNum.x = SelectNum;
								ACC_Data.e_Acc_1 = (ACC_1)DecNum.x;
								SoundManager.Instance.PlaySE(SE_decision);
							}
							break;
						}
					case Select_Acc.ACC_2:
						{
							if (SelectNum < (int)ACC_2.MAX)
							{
								DecNum.y = SelectNum;
								ACC_Data.e_Acc_2 = (ACC_2)DecNum.y;
								SoundManager.Instance.PlaySE(SE_decision);
							}
							break;
						}
					case Select_Acc.ACC_3:
						{
							if (SelectNum < (int)ACC_3.MAX)
							{
								DecNum.z = SelectNum;
								ACC_Data.e_Acc_3 = (ACC_3)DecNum.z;
								SoundManager.Instance.PlaySE(SE_decision);
							}
							break;
						}
					case Select_Acc.COLOR:
						{
							if(mode == 0 && PalletNum < ColPallets.Count)
							{
								Color col = ColPallets[PalletNum].transform.GetChild(0).GetComponent<Image>().color;

								Slider_Red.value = col.r;
								Slider_Green.value = col.g;
								Slider_Blue.value = col.b;
								SoundManager.Instance.PlaySE(SE_decision);
								Body_Mat.color = ColPallets[PalletNum].transform.GetChild(0).GetComponent<Image>().color;
								Debug.Log("�J���[�ύX");
							}
							break;
						}
				}
			}

			// �����_��
			if (Input.GetKeyDown(KeyCode.R))
			{
				RandomDecoration();
				Debug.Log("�����_���I");
			}

			// �I�v�V�����{�^��(�ۑ��{�^��)
			if (Input.GetKeyDown(KeyCode.S))
			{
				Debug.Log("�Z�[�u�J�n�I");
				SaveDecoration();
				Debug.Log("�Z�[�u_�����I");
			}

			// �L�[����������ςȂ���
			GetKeyDown();

			#endregion
			// -----------------------

			// ���ݑI������Ă���ԍ��ƍ��ڂɈ�v����A�C�e����������
			SetDecoration();

			// �I����UI�̈ړ�
			Nextpos = UI_ChildList[SelectNum].GetComponent<RectTransform>().position;
			MoveSelectAcc();
			MoveDecisionAcc();

			// �J���[�ύX
			Col_Red = Slider_Red.value;
			Col_Green = Slider_Green.value;
			Col_Blue = Slider_Blue.value;
			Body_Mat.color = new Color(Col_Red, Col_Green, Col_Blue);

			ColTexts[0].text = Mathf.Ceil(Slider_Red.value * 255).ToString();
			ColTexts[1].text = Mathf.Ceil(Slider_Green.value * 255).ToString();
			ColTexts[2].text = Mathf.Ceil(Slider_Blue.value * 255).ToString();

			if (e_NowSelect != Select_Acc.COLOR) { isColor = false;CarryMat.color = Body_Mat.color; }
		}

	}
	
	// ���͊֐�
	public void ConKeyDir()
	{
		float LStickdead_2 = LStickdead;
		if (pad != null && mode==0) { 
			if((pad.leftStick.ReadValue().x < -LStickdead_2 && pad.leftStick.ReadValue().y > LStickdead_2)&& !Stick_OnOff)
			{ dir = Dir.LU; SoundManager.Instance.PlaySE(SE_cursor); }
			else if((pad.leftStick.ReadValue().x > LStickdead_2 && pad.leftStick.ReadValue().y > LStickdead_2)&& !Stick_OnOff)
			{ dir = Dir.RU; SoundManager.Instance.PlaySE(SE_cursor); }
			else if((pad.leftStick.ReadValue().x < -LStickdead_2 && pad.leftStick.ReadValue().y < -LStickdead_2) && !Stick_OnOff)
			{ dir = Dir.LD; SoundManager.Instance.PlaySE(SE_cursor); }
			else if((pad.leftStick.ReadValue().x > LStickdead_2 && pad.leftStick.ReadValue().y < -LStickdead_2)&& !Stick_OnOff)
			{ dir = Dir.RD; SoundManager.Instance.PlaySE(SE_cursor); }
			else if ((Input.GetKey(KeyCode.LeftArrow) || pad.leftStick.ReadValue().x < -LStickdead) && !Stick_OnOff)
			{ dir = Dir.LEFT; SoundManager.Instance.PlaySE(SE_cursor); }
			else if ((Input.GetKey(KeyCode.RightArrow) || pad.leftStick.ReadValue().x > LStickdead) && !Stick_OnOff)
			{ dir = Dir.RIGHT; SoundManager.Instance.PlaySE(SE_cursor); }
			else if ((Input.GetKey(KeyCode.UpArrow) || pad.leftStick.ReadValue().y > LStickdead) && !Stick_OnOff)
			{ dir = Dir.UP; SoundManager.Instance.PlaySE(SE_cursor); }
			else if ((Input.GetKey(KeyCode.DownArrow) || pad.leftStick.ReadValue().y < -LStickdead) && !Stick_OnOff)
			{ dir = Dir.DOWN; SoundManager.Instance.PlaySE(SE_cursor); }
			else { dir = Dir.NONE; }

		}
		else if(pad!=null&&mode!=0)
		{
			if ((Input.GetKey(KeyCode.LeftArrow) || pad.leftStick.ReadValue().x < -LStickdead) && !Stick_OnOff)
			{ dir = Dir.LEFT; }
			else if ((Input.GetKey(KeyCode.RightArrow) || pad.leftStick.ReadValue().x > LStickdead) && !Stick_OnOff)
			{ dir = Dir.RIGHT; }
			else if ((Input.GetKey(KeyCode.UpArrow) || pad.leftStick.ReadValue().y > LStickdead) && !Stick_OnOff)
			{ dir = Dir.UP; SoundManager.Instance.PlaySE(SE_cursor); }
			else if ((Input.GetKey(KeyCode.DownArrow) || pad.leftStick.ReadValue().y < -LStickdead) && !Stick_OnOff)
			{ dir = Dir.DOWN; SoundManager.Instance.PlaySE(SE_cursor); }
			else { dir = Dir.NONE; }
		}
		else if(pad == null)
		{
			if (Input.GetKey(KeyCode.LeftArrow) && !Stick_OnOff){ dir = Dir.LEFT; }
			else if (Input.GetKey(KeyCode.RightArrow) && !Stick_OnOff) { dir = Dir.RIGHT; }
			else if (Input.GetKey(KeyCode.UpArrow) && !Stick_OnOff) { dir = Dir.UP; }
			else if (Input.GetKey(KeyCode.DownArrow) && !Stick_OnOff){ dir = Dir.DOWN; }
			else { dir = Dir.NONE; }

		}
	}

	// �L�[�������ꑱ���Ă��邩�̊֐�
	public void GetKeyDown()
	{
		if (pad != null)
		{
			float LStickdead_2 = LStickdead;
			// �R���g���[���[�X����������
			if(stop!=true && (pad.leftStick.ReadValue().x > LStickdead_2 || pad.leftStick.ReadValue().x < -LStickdead_2 ||
			pad.leftStick.ReadValue().y > LStickdead_2 || pad.leftStick.ReadValue().y < -LStickdead_2 ||
			 Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
			 Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow)))
			{
				Stick_OnOff = true;
				ConB--;
				if (mode == 0 && ConB < 0)
				{
					ConA--;
					if (ConA < 0)
					{
					//	SoundManager.Instance.PlaySE(SE_cursor);
						ConA = Con_A;
						Stick_OnOff = false;
					}
				}
				else if (mode != 0 && ConB < 0) {
					ConA--;
					if (ConA < Con_A/2)
					{
						ConA = Con_A;
						SoundManager.Instance.PlaySE(SE_cursor);
					}
					Stick_OnOff = false;
				}
			}
			else { Stick_OnOff = stop = false; ConB = Con_B; ConA = Con_A; }

			if( !Input.GetButton("Decision") && !Input.GetButton("Cancel"))
			{
				//Debug.Log("������ĂȂ�");
				Button_OnOff = false;
			}
		}
		else if(pad == null)
		{
			if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) ||
				Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
			{
				Stick_OnOff = true;
				ConB--;
				if (mode == 0 && ConB < 0)
				{
					ConA--;
					if (ConA < 0)
					{
				//		SoundManager.Instance.PlaySE(SE_cursor);
						ConA = Con_A;
						Stick_OnOff = false;
					}
				}
				else if (mode != 0 && ConB < 0)
				{
					ConA--;
					if (ConA < Con_A / 2)
					{
						ConA = Con_A;
						SoundManager.Instance.PlaySE(SE_cursor);
					}
					Stick_OnOff = false;
				}
			}
			else { Stick_OnOff = false; ConB = Con_B; ConA = Con_A; }
		}
	}

	// ��
	public void curry()
	{
	//	ACC_Data.color = Color.white;
		ACC_Data.e_Body = BODYTEX.NONE;
		//	ACC_Data.e_Acc_1 = ACC_1.NONE;
		//	ACC_Data.e_Acc_2 = ACC_2.NONE;
		//	ACC_Data.e_Acc_3 = ACC_3.NONE;
		SetBodyAcc(0);
		ACC_Data.ReleaseNum = Curry_ReleaseNum;
	}

	// ����g������
	public void InitDecision()
	{
		Obj_Decision = Instantiate(Obj_Select);
		Obj_Decision.transform.SetParent(Obj_Select.transform.parent);
		Obj_Decision.transform.SetSiblingIndex(1);
		Obj_Decision.transform.GetChild(0).GetComponent<Image>().color = Col;
	}

	// �A�N�Z�T���[��ݒ�(�Z�[�u�f�[�^)
	public void SetDecoration_SaveData()
	{
		// �{�f�B ----------------------------------------
		Body_Mat.SetTexture("_MainTex", Body_Tex[(int)ACC_Data.e_Body]);

		// �����Őg�̂̐F�f�[�^�擾���Ă�������
		// �܂��͂��́hPlayer_Color�h�̐F���ɕς��Ă�������

		/* �� */
		ACC_Data.color = save.LoadColor();//new Color(Col_Red, Col_Green, Col_Blue);
		


		// �g�̂̐F��ύX
		Body_Mat.color = ACC_Data.color;
		Slider_Red.value = Body_Mat.color.r;
		Slider_Green.value = Body_Mat.color.g;
		Slider_Blue.value = Body_Mat.color.b;
		// ----------------------------------------------

		// �A�N�Z�T���[ ----------------------------------

		// �����ŃZ�[�u����Ă���A�N�Z�T���[�̔ԍ����擾����悤�ɂ��Ă������� ---

		// ---

		// �A�N�Z�T���[��0�ԖڂȂ��\��
		if (ACC_Data.e_Acc_1 != ACC_1.NONE && ACC_Data.e_Acc_1 != ACC_1.MAX){
			Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[(int)ACC_Data.e_Acc_1];
			Player_Acc_1.SetActive(true);
		}
		else { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }

		if (ACC_Data.e_Acc_2 != ACC_2.NONE && ACC_Data.e_Acc_2 != ACC_2.MAX) {
			Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[(int)ACC_Data.e_Acc_2];
			Player_Acc_2.SetActive(true);
		}
		else { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }

		if (ACC_Data.e_Acc_3 != ACC_3.NONE && ACC_Data.e_Acc_3 != ACC_3.MAX) {
			//	Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[(int)ACC_Data.e_Acc_3];
			SetBodyAcc((int)ACC_Data.e_Acc_3);
			Player_Acc_3.SetActive(true);
		}
		else { Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }

		// �������Ă���A�N�Z�̔ԍ��擾
		DecNum.x = (int)ACC_Data.e_Acc_1;
		DecNum.y = (int)ACC_Data.e_Acc_2;
		DecNum.z = (int)ACC_Data.e_Acc_3;
		// ----------------------------------------------
	}

	// �A�N�Z�T���[��ݒ�
	public void SetDecoration()
	{
		
		switch(e_NowSelect)
		{
			case Select_Acc.ACC_1:
				{
					Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[(int)ACC_Data.e_Acc_2];
				//	Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[(int)ACC_Data.e_Acc_3];

					if(Acc_1_MatList[SelectNum] != null) { 
						Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[SelectNum];
					}
					else { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }

					if (SelectNum <= 0) { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }
					if(ACC_Data.e_Acc_2 <= 0) { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }
					if(ACC_Data.e_Acc_3 <= 0) { Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }

					break;
				}
			case Select_Acc.ACC_2:
				{
					Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[(int)ACC_Data.e_Acc_1];
				//	Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[(int)ACC_Data.e_Acc_3];

					if (Acc_2_MatList[SelectNum] != null)
					{
						Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[SelectNum];
					}
					else { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }

					if (SelectNum <= 0) { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_1 <= 0) { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_3 <= 0) { Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }
					break;
				}
			case Select_Acc.ACC_3:
				{
					Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[(int)ACC_Data.e_Acc_1];
					Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[(int)ACC_Data.e_Acc_2];

					//	Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[SelectNum];

					SetBodyAcc(SelectNum);

					if (SelectNum <= 0) { SetBodyAcc(0); }//Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_1 <= 0) { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_2 <= 0) { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }
					break;
				}
			case Select_Acc.COLOR:
				{
					Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[(int)ACC_Data.e_Acc_1];
					Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[(int)ACC_Data.e_Acc_2];
				//	Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[(int)ACC_Data.e_Acc_3];

					if (ACC_Data.e_Acc_1 <= 0) { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_2 <= 0) { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }
					if (ACC_Data.e_Acc_3 <= 0) { Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }
					break;
				}
		}
	}

	public void SetAcc()
	{
		for (int y = 0; y < Y; y++)
		{
			for (int x = 0; x < X; x++)
			{
				NowSelectAccUI(y * X + x);
			}
		}
	}

	// �����_��
	public void RandomDecoration()
	{
		// �{�f�B ----------------------------------------
		float r = Random.Range(0.0f,255.0f) / 255;
		float g = Random.Range(0.0f,255.0f) / 255;
		float b = Random.Range(0.0f,255.0f) / 255;
		ACC_Data.color = new Color(r, g, b);

		// �g�̂̐F��ύX
		Body_Mat.color = ACC_Data.color;
		Slider_Red.value = r;
		Slider_Green.value = g;
		Slider_Blue.value = b;
		// ----------------------------------------------

		// �A�N�Z�T���[ ----------------------------------
		int acc_1 = Random.Range(0, ACC_Data.ReleaseNum.x + 1);
		int acc_2 = Random.Range(0, ACC_Data.ReleaseNum.y + 1);
		int acc_3 = Random.Range(0, ACC_Data.ReleaseNum.z + 1);

		Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[acc_1];
		Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[acc_2];
		Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[acc_3];

		// �A�N�Z�T���[��0�ԖڂȂ��\��
		if (ACC_Data.e_Acc_1 != ACC_1.NONE && ACC_Data.e_Acc_1 != ACC_1.MAX)
		{
			Player_Acc_1.GetComponent<Renderer>().material = Acc_1_MatList[(int)ACC_Data.e_Acc_1];
			Player_Acc_1.SetActive(true);
		}
		else { Player_Acc_1.GetComponent<Renderer>().material = NoneMat; }

		if (ACC_Data.e_Acc_2 != ACC_2.NONE && ACC_Data.e_Acc_2 != ACC_2.MAX)
		{
			Player_Acc_2.GetComponent<Renderer>().material = Acc_2_MatList[(int)ACC_Data.e_Acc_2];
			Player_Acc_2.SetActive(true);
		}
		else { Player_Acc_2.GetComponent<Renderer>().material = NoneMat; }

		if (ACC_Data.e_Acc_3 != ACC_3.NONE && ACC_Data.e_Acc_3 != ACC_3.MAX)
		{
			Player_Acc_3.GetComponent<Renderer>().material = Acc_3_MatList[(int)ACC_Data.e_Acc_3];
			Player_Acc_3.SetActive(true);
		}
		else { Player_Acc_3.GetComponent<Renderer>().material = NoneMat; }

		// �������Ă���A�N�Z�̔ԍ��擾
		DecNum.x = acc_1;
		DecNum.y = acc_2;
		DecNum.z = acc_3;
		ACC_Data.e_Acc_1 = (ACC_1)DecNum.x;
		ACC_Data.e_Acc_2 = (ACC_2)DecNum.y;
		ACC_Data.e_Acc_3 = (ACC_3)DecNum.z;

		switch(e_NowSelect)
		{
			case Select_Acc.ACC_1: SelectNum = acc_1; break;
			case Select_Acc.ACC_2: SelectNum = acc_2; break;
			case Select_Acc.ACC_3: SelectNum = acc_3; break;
		}
		// ----------------------------------------------
		Debug.Log(ACC_Data.ReleaseNum);
		Debug.Log(acc_1 + "," + acc_2 + "," + acc_3);
	}


	// ���ݑI�𒆂̃A�N�ZUI��\��������(�����̔ԍ�����)
	public void NowSelectAccUI(int num)
	{
		MaxChildNum = UI_ChildList.Count;

		// �܂��͕\��������
		UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(true);

		switch(e_NowSelect)
		{
			case Select_Acc.ACC_1:
				{
					
					if (Acc_1_MatList.Length > num && Acc_1_MatList[num] != null)
					{ 
						UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(true);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = Acc_1_MatList[num];
					}
					else if (num > ACC_Data.ReleaseNum.x)
					{
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
					//	UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					else
					{
						//UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(false);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
						//UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					break;
				}
			case Select_Acc.ACC_2:
				{
					if (Acc_2_MatList.Length > num && Acc_2_MatList[num] != null)
					{
						UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(true);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = Acc_2_MatList[num];
					}
					else if (num > ACC_Data.ReleaseNum.y)
					{
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
						UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					else
					{
						//	UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(false);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
						UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					break;
				}
			case Select_Acc.ACC_3:
				{
					if (Acc_3_MatList.Length > num && Acc_3_MatList[num] != null)
					{
						UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(true);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = Acc_3_MatList[num];
					}
					else if (num > ACC_Data.ReleaseNum.z)
					{
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
						UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					else
					{
						//	UI_ChildList[num].transform.GetChild(0).gameObject.SetActive(false);
						UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
						UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					}
					break;
				}
			case Select_Acc.COLOR:
				{
					// �C���x���g�������
					UI_ChildList[num].transform.GetChild(1).transform.GetComponent<Image>().material = NoneMat;
					UI_ChildList[num].transform.GetChild(1).gameObject.SetActive(false);
					isColor = true;
					break;
				}
		}
	}

	public void ChangeMajorItem(bool bo)
	{
		if(bo != true)
		{ 
			// ���܂őI�����Ă����區�ڂ�ύX
			Majoritems[(int)e_NowSelect].color = Color.white;
			Majoritems[(int)e_NowSelect].transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			// ���݂̑區�ڂ̐F�ƃT�C�Y�ύX
			Majoritems[(int)e_NowSelect].color = itemCol;
			Majoritems[(int)e_NowSelect].transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
		}
	}

	public void MoveSelectAcc()
	{
		Obj_Select.GetComponent<RectTransform>().position = Vector3.Lerp(Obj_Select.GetComponent<RectTransform>().position, Nextpos, Time.deltaTime * speed);
	}

	public void MoveDecisionAcc()
	{
		int Dec = 0;
		switch(e_NowSelect)
		{
			case Select_Acc.ACC_1:{
					Dec = DecNum.x;					
					break;
				}
			case Select_Acc.ACC_2:{
					Dec = DecNum.y;
					break;
				}
			case Select_Acc.ACC_3:{
					Dec = DecNum.z;
					break;
				}
		}

		Obj_Decision.GetComponent<RectTransform>().position = Vector3.Lerp(Obj_Decision.GetComponent<RectTransform>().position,
																UI_ChildList[Dec].transform.position, Time.deltaTime * speed * 10);
	}

	public void MoveSelectColpal()
	{
		Obj_PalletSelect.GetComponent<RectTransform>().position = Vector3.Lerp(Obj_PalletSelect.GetComponent<RectTransform>().position,PalletNextPos, Time.deltaTime * speed);
	}

	public void SetBodyAcc(int num)
	{

		BodyFront.GetComponent<Renderer>().material = FrontList[num];
		BodyLeft.GetComponent<Renderer>().material = LeftList[num];
		BodyRight.GetComponent<Renderer>().material = RightList[num];
		BodyBack.GetComponent<Renderer>().material = BackList[num];

		switch ((ACC_3)num)
		{
			case ACC_3.NONE: {
					BodyFront.GetComponent<Renderer>().material =NoneMat;
					BodyLeft.GetComponent<Renderer>().material = NoneMat;
					BodyRight.GetComponent<Renderer>().material =NoneMat;
					BodyBack.GetComponent<Renderer>().material = NoneMat;
					break;
			}
			case ACC_3.MAX: {
					BodyFront.GetComponent<Renderer>().material = NoneMat;
					BodyLeft.GetComponent<Renderer>().material = NoneMat;
					BodyRight.GetComponent<Renderer>().material = NoneMat;
					BodyBack.GetComponent<Renderer>().material = NoneMat;
					break;
			}
		}
	}

	// �Z�[�u
	public void SaveDecoration()
	{
		Debug.Log("�Z�[�u�J�n�I");
		ACC_Data.color = Body_Mat.color;

		// ��
		save.SaveAcc(DecNum);
		save.SaveCol(Body_Mat.color);

		Debug.Log("�Z�[�u_�����I");
	}

	// �J���[�Q�b�g�Z�b�g
	public Color GetSet_Color
	{
		get { ACC_Data.color = Body_Mat.color; return ACC_Data.color; }
		set { ACC_Data.color = value; Body_Mat.color = ACC_Data.color; }
	}

	// �����A�N�Z�ԍ��Q�b�g�Z�b�g
	public Vector3Int GetSet_AccNum
	{
		get { return DecNum; }
		set { DecNum = value; }
	}

	// ����ԍ��Q�b�g�Z�b�g
	public Vector3Int GetSet_ReleaseNum
	{
		get { return Curry_ReleaseNum; }
		set { Curry_ReleaseNum = value; }
	}
}
