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
            Debug.Log("<color=orange>達成成就：得分10分</color>");
        }
        else if (PlayerPrefs.GetInt("Score") == 16) {
            Debug.Log("<color=orange>達成成就：得分16分</color>");
        }
        else if (PlayerPrefs.GetInt("Score") == 24) {
            Debug.Log("<color=orange>達成成就：得分24分</color>");
        }

    }
}