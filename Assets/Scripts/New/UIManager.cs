using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour {

    private static UIManager _instance;
    public static UIManager Instance 
    {
        get { return _instance; }
    }

    void Awake() 
    {
        _instance = this;
    }
    public int score = 0;
    public int length = 0;

    public Text scoreText;
    public Text messageText;
    public Text lengthText;
    public Image bgColor;
    private Color tempColor;

    public Button pauseBtn;
    public Sprite[] pauseSprite;
    public bool ispause;
    public bool border;

    void Start() 
    {
        //表示是自由模式
        if (PlayerPrefs.GetInt("border", 1) == 0)
        {
            border = false;
            foreach (Transform t in bgColor.gameObject.transform)
            {
                t.GetComponent<Image>().enabled = false;
            }
            
        }
        else 
        {
            border = true;
            foreach (Transform t in bgColor.gameObject.transform)
            {
                t.GetComponent<Image>().enabled = false;
            }
            
        }   
    }
    
    //得分划分阶段，改变背景颜色
    void Update() 
    {
        switch (score / 100) 
        {
            case 0:
            case 1:
            case 2:
                break;
            case 3:
            case 4:
                ColorUtility.TryParseHtmlString("#CCEEFFFF", out tempColor);
                bgColor.color = tempColor;
                messageText.text = "阶段" + 2;
                break;
            case 5:
            case 6:
                ColorUtility.TryParseHtmlString("#CCFFDBFF", out tempColor);
                bgColor.color = tempColor;
                messageText.text = "阶段" + 3;
                break;
            case 7:
            case 8:
                ColorUtility.TryParseHtmlString("#EGFFCCFF", out tempColor);
                bgColor.color = tempColor;
                messageText.text = "阶段" + 4;
                break;
            case 9:
            case 10:
                ColorUtility.TryParseHtmlString("#FFF3CCFF", out tempColor);
                bgColor.color = tempColor;
                messageText.text = "阶段" + 5;
                break;
            default:
                ColorUtility.TryParseHtmlString("#FFDACCFF", out tempColor);
                bgColor.color = tempColor;
                messageText.text = "无尽阶段";
                break;
        }
    }

    //每吃一个食物加10分，长度加1
    public void UpdateUI(int s = 10,int l = 1) 
    {
        score += s;
        length += l;
        scoreText.text = "得分：\n" + score;
        lengthText.text = "长度：\n" + length;
    }

    //暂停按钮的方法
    public void Pasue() 
    {
        ispause = !ispause;
        if (ispause)
        {
            Time.timeScale = 0;
            pauseBtn.GetComponent<Image>().sprite = pauseSprite[1];
        }
        else 
        {
            Time.timeScale = 1;
            pauseBtn.GetComponent<Image>().sprite = pauseSprite[0];
        }
    }

    public void Home() 
    {
        SceneManager.LoadScene(0);
    }
}
