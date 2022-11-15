using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class itemData : MonoBehaviour
{
	[SerializeField]
	public class infomation {
		/// <summary>
		/// ID
		/// </summary>
		[SerializeField] public int ID;
		/// <summary>
		/// 排序ID
		/// </summary>
		[SerializeField] public int sortID;
		/// <summary>
		/// 指令
		/// </summary>
		[SerializeField] public int command = -1;
		/// <summary>
		/// 立即觸發效果
		/// </summary>
		[SerializeField] public List<int> Effect = new List<int>();
		/// <summary>
		/// icon 名稱
		/// </summary>
		[SerializeField] public string iconName = "";
	}


	private static List<infomation> dataTable; //從 JsonDataBase 建置

	public static void update_data() {
		dataTable = JsonMapper.ToObject<List<infomation>>(JsonDataBase.Item_datas.ToJson());
	}

	public static List<infomation> GetDatas() {
		if(dataTable == null) {
			update_data();
		}
		if (dataTable == null) {
			dataTable = new List<infomation>();
		}
		return dataTable;
	}

	public static infomation GetData(int id) {
		infomation output = null;
		if (dataTable == null) {
			update_data();
		}
		if (dataTable != null) {
			output = dataTable.Find( o => o.ID == id);
		}
		return output;
	}

}
