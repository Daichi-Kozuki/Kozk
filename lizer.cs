// ======================================
// ���C�U�[����cs
// ======================================
// �����Â�

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lizer : MonoBehaviour
{
	[SerializeField] public Material[] mat;

	[Header("�v���C���[�I�u�W�F�N�g")]
	[SerializeField] public GameObject player;

	[SerializeField] public GameObject GoalObj;

	[Header("�F�Ƃ̋���")]
	public float distance = 0.0f;   // ��ԋ߂�����
	public float dis = 0.0f;
	public List<float> coldistance;
	[SerializeField] public List<GameObject> ColorObj_distance = new List<GameObject>();
	int a = 0;

	[Header("�F�I�u�W�F�N�g")]
	[SerializeField] public GameObject ColorDummyObj;
	[SerializeField] public List<GameObject> ColorObj = new List<GameObject>();
	[SerializeField] public GameObject[] disRank;   // �����Ⴂ��
	[SerializeField] public GameObject Deleteman;   // �����ꂽ����

	[Header("���C�U�[�}�e���A��")]
	[SerializeField] public Material[] Lizer_mat;
	[SerializeField] public Texture LizerTex; // �T��
	[SerializeField] public bool[] Lizer_mat_Used;  // �g�p�����ǂ���
	[SerializeField] public int LizermatNum = 5;
	[SerializeField] public float lizerpower = 0.0f;    // ���C�U�[�̋���
	[SerializeField] public Color LizerColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);       // ���C�U�[�̐F
	[SerializeField] public Color Sub_LizerColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);   // �T�u�̃��C�U�[�̐F��2�F��
	[SerializeField] public Color col;

[Header("�O�̃I�u�W�F�N�g")]
	[SerializeField] public List<GameObject> m_oldObj = new List<GameObject>();
	[SerializeField] public List<Material> m_oldObj_mat = new List<Material>();

	[Header("�O�̃}�e���A��")]
	[SerializeField] Material[] previous_mat;
	int previousNum = 0;

	[Header("�Đ����x")]
	[SerializeField] public float SearchRange = 30.0f;

	[Header("�Đ����x")]
	[SerializeField] public float frame = 60.0f;
	[SerializeField] public float frame_2 = 60.0f;

	[Header("�{�̂̌��")]
	public float Albedo_Light = 0.0f;

	[Header("���C�U�[�̌��")]
	public float Emission_Light = 1.0f;

	private int[] m_emissionBlock;


	// �f�o�b�O�p
	[Header("�f�o�b�O�p")]
	[SerializeField] public Texture tex;
	[SerializeField] public Texture bump;

	void Awake()
	{
		m_emissionBlock = new int[LizermatNum];
		GoalObj.SetActive(false);
		ColorObj = new List<GameObject> { ColorDummyObj };
		coldistance = new List<float> { 0 };
	}

	// Start is called before the first frame update
	void Start()
	{
		// coldistance = new float[ColorObj.Count];
		for (int i = 0; i < Lizer_mat.Length; i++)
		{
			SetCol_Albedo(i, ColorDummyObj.GetComponent<Renderer>().material.color);
			SetCol_Emission(i, LizerColor);
			Lizer_mat[i].SetFloat("_Threshold", 0.0f);
			Lizer_mat_Used[i] = false;  // �S�Ė��g�p
		
		}
		ColorObj_distance = ColorObj;
	}

	// Update is called once per frame
	void Update()
	{
		if (ColorObj.Count == 1) { GoalObj.SetActive(true); }
	//	if (Input.GetKeyDown(KeyCode.T)) { Lizer_Tex(tex);}
	//	if (Input.GetKeyDown(KeyCode.B)) { Lizer_BumpTex(bump); }

		// �F�I�u�W�F�N�g���m�̋����𑪂�
		for (int i = 0; i < ColorObj.Count; i++)
		{
			if (i == 0) { continue; }
			coldistance[i] = Vector3.Distance(player.transform.position, ColorObj[i].transform.position);

			if(disRank[0] != ColorObj[i] && coldistance[i] < coldistance[1])
			{
				Debug.Log("�P");
				//distance = coldistance[i];
				GameObject obj = ColorObj[1];
				ColorObj[1] = ColorObj[i];
				ColorObj[i] = obj;
			}
			else if(disRank[0] != ColorObj[i] && disRank[1] != ColorObj[i] && coldistance[i] < coldistance[2])
			{
				Debug.Log("�Q");
				//distance = coldistance[i];
				GameObject obj = ColorObj[2];
				ColorObj[2] = ColorObj[i];
				ColorObj[i] = obj;
			}
		}

		disRank[0] = ColorObj[1];
		disRank[1] = ColorObj[2];
		distance = Vector3.Distance(player.transform.position, disRank[0].transform.position);

		lizerpower = (1.1f - (distance / SearchRange));
		LizerColor = disRank[0].GetComponent<Renderer>().material.color;

		//	// �T�u�J���[
		if (disRank[1] != null)
		{
			Sub_LizerColor = disRank[1].GetComponent<Renderer>().material.color;
			float Subpower = (1.1f - (Vector3.Distance(player.transform.position, disRank[1].transform.position) / SearchRange));
			Sub_LizerColor.r *= Subpower;
			Sub_LizerColor.g *= Subpower;
			Sub_LizerColor.b *= Subpower;
			Sub_LizerColor /= 2;

		}
		for (int k = 0; k < Lizer_mat.Length; k++)
		{
			SetCol_Emission(k, LizerColor + Sub_LizerColor);
		}
		col = LizerColor + Sub_LizerColor;

		#region ������r
		/*
		// �F�I�u�W�F�N�g���m�̋����𑪂�
		for (int i = 0; i < ColorObj.Count; i++)
		{
			float discol = Vector3.Distance(player.transform.position, ColorObj[i].transform.position);

			if (i == 0) { distance = dis = discol; }

			if (dis > discol)
			{
				// ���܂ł̈�ԋ߂������������Ԏ��
				if (ColorObj.Count <= 2) { disRank[1] = ColorObj[1]; }
				if (disRank[0] != ColorObj[i]) { disRank[1] = disRank[0]; }
				disRank[0] = ColorObj[i];

				// ��ԋ߂�����������
				distance = dis = discol;

				// ���C�U�[�̋����������ɂ���ĕύX
				// �������P�������� 1.1 - 0.1 = 1.0 �ɂȂ�A���C�U�[�̋�����1.0�Ŏn�܂�
				lizerpower = (1.1f - (distance / SearchRange));

				// ���C�U�[����ԋ߂������̐F�I�u�W�F�N�g�̐F�ɕύX
				LizerColor = disRank[0].GetComponent<Renderer>().material.color;

				// �T�u�J���[
				if (disRank[1] != null)
				{
					Sub_LizerColor = disRank[1].GetComponent<Renderer>().material.color;
					//float Subpower = (1.1f - (Vector3.Distance(player.transform.position, disRank[1].transform.position) / SearchRange));
					//Sub_LizerColor.r *= Subpower;
					//Sub_LizerColor.g *= Subpower;
					//Sub_LizerColor.b *= Subpower;

					//Sub_LizerColor /= 2; 
					//SetCol_Emission(3, Sub_LizerColor);
				}

				//SetCol_Emission(0, LizerColor + Sub_LizerColor);
				//SetCol_Emission(1, LizerColor + Sub_LizerColor);
				//SetCol_Emission(2, LizerColor + Sub_LizerColor);
				//SetCol_Emission(3, LizerColor + Sub_LizerColor);
				//SetCol_Emission(4, LizerColor + Sub_LizerColor);
				for (int k = 0; k < Lizer_mat.Length; k++)
				{
					SetCol_Emission(k, LizerColor + Sub_LizerColor);
				}

				//	SetPrevious_mat(previous_mat[4],4);

			}
			else if (disRank[1] != null && discol < Vector3.Distance(player.transform.position, disRank[1].transform.position) || disRank[1] == Deleteman)
			{
				disRank[1] = ColorObj[i];
			}
		}
		*/
		#endregion

	}

		// ���C�U�[�����ύX
	public void Lizer_Dir(int LizerNum, int dir)
	{
		Lizer_mat[LizerNum].SetInt("_Dir", dir);
	}

	// ���C�U�[��texture�ꋓ�ύX
	public void Lizer_Tex(Texture tex)
	{
		LizerTex = tex;
		for (int i = 0; i < Lizer_mat.Length; i++)
		{
			Lizer_mat[i].SetTexture("_MainTex", tex);
		}
	}
	public void Lizer_Tex(Texture tex,int num)
	{
		Lizer_mat[num].SetTexture("_MainTex", tex);
	}

	// ���C�U�[�̃o���v�}�b�v�ꋓ�ύX
	public void Lizer_BumpTex(Texture bump)
	{
		for (int i = 0; i < Lizer_mat.Length; i++)
		{
			Lizer_mat[i].SetTexture("_BumpMap", bump);
		}
	}

	// ���C�U�[�̋�����
	public void Lizer_Metallic(float meta)
	{
		for (int i = 0; i < Lizer_mat.Length; i++)
		{
			Lizer_mat[i].SetFloat("_Metallic", meta);
		}
	}

	// ���C�U�[�̋�����
	public void Lizer_Smooth(float smoo)
	{
		for (int i = 0; i < Lizer_mat.Length; i++)
		{
			Lizer_mat[i].SetFloat("_Glossiness", smoo);
		}
	}
	
	// �O����Ăяo���p
	public void Start_Lizer(int num, float max, int dir = 0)
	{
		Lizer_mat_Used[num] = true;

		// 0:�� 1:�� 2:�� 3:��
		Lizer_Dir(num,dir);

		StartCoroutine(Start_lizer(Lizer_mat[num], max, num));

		//for (int LizerNum = 0; LizerNum < Lizer_mat.Length; LizerNum++)
		//{
		//	if (Lizer_mat_Used[LizerNum] == false)
		//	{
		//		Lizer_mat_Used[LizerNum] = true;
		//		StartCoroutine(Start_lizer(Lizer_mat[LizerNum], max, LizerNum));
		//		break;
		//	}
		//}
	}

	// �{�̃J���[�ύX
	public void SetCol_Albedo(int LizerNum, Color color)
	{
		//float factor = Mathf.Pow(1, Albedo_Light);
		//color.r *= factor; 
		//color.g *= factor; 
		//color.b *= factor; 
		Lizer_mat[LizerNum].SetColor("_Color", color * Albedo_Light);
	}

	// ���C�U�[�J���[�ύX
	public void SetCol_Emission(int LizerNum, Color color)
	{
		float factor = Emission_Light;// Mathf.Pow(1, Emission_Light);
		color.r *= factor;
		color.g *= factor;
		color.b *= factor;
		Lizer_mat[LizerNum].SetColor("_Specified", color);
	}

	// �{�̂̌���ύX
	public void SetLight_Albedo(float light) { Albedo_Light = light; }

	// ���C�U�[�̌���ύX
	public void SetLight_Emission(float light) { Emission_Light = light; }

	// �ω��O�̖{�̃J���[�ɕύX
	public void SetPrevious_mat(Material mat, int num)
	{
		if (previousNum < LizermatNum)
		{
			previousNum = 0;
		}
		previous_mat[num] = mat;    // �ω��O�̖{�̃J���[�T��
		SetCol_Albedo(num, previous_mat[num].color);    // �{�̃J���[�ύX
	}

	// �F�I�u�W�F�N�g���Z�b�g
	public void SetColObj(GameObject colobj)
	{
		ColorObj.Add(colobj);
		coldistance.Add(0f);
	}

	// ���X�g������
	public void Reset_ColObj()
	{
		ColorObj = new List<GameObject>();
	}

	// �F�I�u�W�F�N�g�폜
	public void Delete_ColObj(GameObject obj)
	{
		Deleteman = obj;
		ColorObj.Remove(obj);
	}

	public void SetoOldObj(int lizerNum,GameObject obj, GameObject obj2 = null )
	{
		
		SetOldObj_mat(obj.GetComponent<Renderer>().material);
		m_oldObj.Add(obj);
		m_emissionBlock[lizerNum] = 1;
		if (obj2 == null) return;
		SetOldObj_mat(obj2.GetComponent<Renderer>().material);
		m_oldObj.Add(obj2);
		m_emissionBlock[lizerNum] = 2;
	}

	public void SetoOldObj(int lizerNum, NeStageLoader.Block block_1, NeStageLoader.Block block_2, int isblock_2 = 0)
	{
		if (block_1.eBlockType == NeStageLoader.EBlockType.BLOCK_NORMAL)
		{
			SetOldObj_mat(mat[0]);
		}
		else if (block_1.eBlockType == NeStageLoader.EBlockType.BLOCK_COLOR)
		{
			switch (block_1.eColorValue)
			{
				case NeStageLoader.EColorValue.COLOR_NONE:
					{
						SetOldObj_mat(mat[0]);
						break;
					}
                case NeStageLoader.EColorValue.COLOR_RED:
                    {
                        SetOldObj_mat(mat[1]);
                        break;
                    }
                case NeStageLoader.EColorValue.COLOR_GREEN:
                    {
                        SetOldObj_mat(mat[2]);
                        break;
                    }
                case NeStageLoader.EColorValue.COLOR_BRUE:
					{
						SetOldObj_mat(mat[3]);
						break;
					}
                case NeStageLoader.EColorValue.COLOR_YELLOW:
                    {
                        SetOldObj_mat(mat[4]);
                        break;
                    }
                    // ���㑝����
            }
		}
		m_oldObj.Add(block_1.gameObj);
		m_emissionBlock[lizerNum] = 1;
		//SetOldObj_mat(obj.GetComponent<Renderer>().material);

		if (isblock_2 == 0) return;

		if (block_2.eBlockType == NeStageLoader.EBlockType.BLOCK_NORMAL)
		{
			SetOldObj_mat(mat[0]);
		}
		else if (block_2.eBlockType == NeStageLoader.EBlockType.BLOCK_COLOR)
		{
			switch (block_2.eColorValue)
			{
				case NeStageLoader.EColorValue.COLOR_NONE:
					{
						SetOldObj_mat(mat[0]);
						break;
					}
				case NeStageLoader.EColorValue.COLOR_RED:
					{
						SetOldObj_mat(mat[1]);
						break;
					}
                case NeStageLoader.EColorValue.COLOR_GREEN:
                    {
                        SetOldObj_mat(mat[2]);
                        break;
                    }
                case NeStageLoader.EColorValue.COLOR_BRUE:
                    {
                        SetOldObj_mat(mat[3]);
                        break;
                    }
                case NeStageLoader.EColorValue.COLOR_YELLOW:
                    {
                        SetOldObj_mat(mat[4]);
                        break;
                    }
                    // ���㑝����
            }
		}
		m_oldObj.Add(block_2.gameObj);
		m_emissionBlock[lizerNum] = 2;
		//if (obj2 == null) return;
		//SetOldObj_mat(obj2.GetComponent<Renderer>().material);

	}
	public void SetOldObj_mat(Material mat)
	{
		m_oldObj_mat.Add(mat);
	}

	IEnumerator Start_lizer(Material Lizer, float max, int Objnum)
	{

		SetPrevious_mat(m_oldObj_mat[m_oldObj_mat.Count - 1], Objnum);

		int i = 0;
		float f = 0.0f;
		max = lizerpower;
		Lizer.SetFloat("_Threshold", 0.0f);
		while (i <= frame)
		{
			i++;
			f += max / frame;
			Lizer.SetFloat("_Threshold", f);
			yield return null;
		}
		Lizer.SetFloat("_Threshold", max);
		i = 0;
		f = 0.0f;
		while (i <= frame_2)
		{
			i++;
			f += max / frame_2;
			Lizer.SetFloat("_Threshold", max - f);
			yield return null;
		}
		Lizer.SetFloat("_Threshold", 0.0f);
		Debug.Log("m_oldObj.Count" + m_oldObj.Count);


		int count = m_oldObj.Count;
		for (int oldNum = 0; oldNum < m_emissionBlock[Objnum]; oldNum++)
		{
			if (m_oldObj[0] != null && m_oldObj_mat[0] != null)
			{
				m_oldObj[0].GetComponent<Renderer>().material = m_oldObj_mat[0];
				m_oldObj_mat.RemoveAt(0);
				m_oldObj.RemoveAt(0);
			}



		}
		Lizer_mat[Objnum].SetTexture("_MainTex", LizerTex);
		Lizer_mat_Used[Objnum] = false;
	}

	//IEnumerator Loop_Lizer()
	//{
	//	int i = 1;
	//	float f = 0.1f;
	//	while (true)
	//	{
	//		if(i >= 5 + 1) { i = 1; f = 0.1f; }
	//		StartCoroutine(Start_lizer(Lizer_mat[i-1],f));
	//		i++;
	//		f += 0.1f;
	//		yield return new WaitForSeconds(0.1f);
	//	}
	//}
}
