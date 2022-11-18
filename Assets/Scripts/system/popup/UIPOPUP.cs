using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPOPUP : baseUIView
{
    public Text content;

	[Header("Yes / OK Type")]
    public GameObject btn1;
	public Text btn1_label;
	[Header("No / Cancel Type")]
	public GameObject btn2;
	public Text btn2_label;

	btnType btn_Type = btnType.other;
	public enum btnType {
		ok,
		yes_no,
		other
	}
	public btnType GetBtnType() {
		return btn_Type;
	}

	public void Setting_Title_content(string title_text, string content_text) {
		if (title != null) {
			title.text = title_text;
			title.text = title.text.Replace("\\n", "\n");
		}

		if (content != null) {
			content.text = content_text;
			content.text = content.text.Replace("\\n", "\n");
		}

	}
	public void setting_btnType(btnType type) {
		btn_Type = type;

		switch (type) {
			case btnType.ok:
				needLockClose = true;

				if (btn1 != null) {
					if (btn1.activeSelf == false) {
						btn1.SetActive(true);
					}
				}
				if (btn2 != null) {
					if (btn2.activeSelf == true) {
						btn2.SetActive(false);
					}
				}

				break;
			case btnType.yes_no:
				if (btn1 != null) {
					if (btn1.activeSelf == false) {
						btn1.SetActive(true);
					}
				}
				if (btn2 != null) {
					if (btn2.activeSelf == false) {
						btn2.SetActive(true);
					}
				}
				break;
		}
	}
	public void setting_btnLabel(string label1 = "", string label2 = "") {
		if (string.IsNullOrEmpty(label1)) {
			switch (btn_Type) {
				case btnType.ok:
					label1 = Language_manager.GetLanguage_value("button_ok");
					break;
				case btnType.yes_no:
					label1 = Language_manager.GetLanguage_value("button_yes");
					break;
			}
		}
		if (string.IsNullOrEmpty(label2)) {
			switch (btn_Type) {
				case btnType.yes_no:
					label2 = Language_manager.GetLanguage_value("button_no");
					break;
			}
		}

		if (btn1_label != null) {
			btn1_label.text = label1;
			btn1_label.text = btn1_label.text.Replace("\\n", "\n");
		}
		if (btn2_label != null) {
			btn2_label.text = label2;
			btn2_label.text = btn2_label.text.Replace("\\n", "\n");
		}
	}
}
