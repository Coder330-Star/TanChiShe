using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartUIManager : MonoBehaviour {

    public Text lastText;
    public Text bestText;

    public Toggle Blue;
    public Toggle Yellow;
    public Toggle Border;
    public Toggle Free;


    void Awake() 
    {
        lastText.text = "上次：长度" + PlayerPrefs.GetInt("lastl",0) + "得分" + PlayerPrefs.GetInt("lasts",0);
        bestText.text = "最高：长度" + PlayerPrefs.GetInt("bestl",0) + "得分" + PlayerPrefs.GetInt("bests",0);

    }


    void Start() 
    {
        if (PlayerPrefs.GetString("sh", "sh01") == "sh01")
        {
            Blue.isOn = true;
            PlayerPrefs.SetString("sh", "sh01");//记录蛇头
            PlayerPrefs.SetString("sb01", "sb0101");//记录第奇数个蛇身
            PlayerPrefs.SetString("sb02", "sb0102");//记录第偶数个蛇身
        }
        else 
        {
            Yellow.isOn = true;
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sh0202");
        }
        if (PlayerPrefs.GetInt("border", 1) == 1)
        {
            Border.isOn = true;
            PlayerPrefs.SetInt("border", 1);
            
        }
        else 
        {
            Free.isOn = true;
            PlayerPrefs.SetInt("border", 0);
            
        }
    }


    public void StartGame() 
    {
        SceneManager.LoadScene(1);
    }

    //蓝色小蛇
    public void BlueState(bool isOn) 
    {
        if (isOn) 
        {
            PlayerPrefs.SetString("sh", "sh01");//记录蛇头
            PlayerPrefs.SetString("sb01", "sb0101");//记录第奇数个蛇身
            PlayerPrefs.SetString("sb02", "sb0102");//记录第偶数个蛇身
        }
    }

    //黄色小蛇
    public void YellowState(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sh0202");
        }
    }

    //有边界
    public void BorderState(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 1);
        }
    }

    //无边界
    public void FreeState(bool isOn)
    {
        if (isOn)
        {
            PlayerPrefs.SetInt("border", 0);
        }
    }

}
