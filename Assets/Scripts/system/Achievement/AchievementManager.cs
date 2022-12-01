using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    public static int _score = 0;
    public static int _win = 0;

    static public void initAchievement() {

        PlayerPrefs.GetInt("Score", 0);
        PlayerPrefs.GetInt("Win", 0);
    }

    static public void IsAchievement() {
        _score++;
        PlayerPrefs.SetInt("Score", _score);

        if (PlayerPrefs.GetInt("Score") == 10) {
            Debug.Log("<color=orange>�F�����N�G�o��10��</color>");
        }
        else if (PlayerPrefs.GetInt("Score") == 16) {
            Debug.Log("<color=orange>�F�����N�G�o��16��</color>");
        }
        else if (PlayerPrefs.GetInt("Score") == 24) {
            Debug.Log("<color=orange>�F�����N�G�o��24��</color>");
        }

    }
}