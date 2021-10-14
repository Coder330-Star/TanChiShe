using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateFood : MonoBehaviour {

    //方便通过类名访问对象，而不用通过对象获取
    private static CreateFood _instance;
    public static CreateFood Instance 
    {
        get { return _instance; }
    }
    void Awake() 
    {
        _instance = this;
    }

    public int xlimit = 21; //在x轴正向最大是21
    public int ylimit = 11; //在y轴正向最大是11
    public int xoffset = 8; //在x轴负方向上的偏移量，即x轴负向最大是13

    public GameObject foodPrefabs;//食物的预制体
    public Transform foodHolder; //用来管理生成的食物
    public GameObject rewardPrefab;

    public Sprite[] foodSprite;//一个数组用来存储食物

    void Start() 
    {
        foodHolder = GameObject.Find("FoodHolder").transform;   
        Food(false);
    }

    public void Food(bool isreward) 
    {
        int index = Random.Range(0, foodSprite.Length);//随机生成一个索引值

        GameObject food = Instantiate(foodPrefabs);

        food.GetComponent<Image>().sprite = foodSprite[index]; //为每个食物赋给一个随机的sprit图

        food.transform.SetParent(foodHolder,false);

        //x,y表示生成食物的位置
        int x = Random.Range(-xlimit + xoffset, xlimit);

        int y = Random.Range(-ylimit, ylimit);

        food.transform.localPosition = new Vector3(x * 30, y * 30, 0);

        
        if (isreward) 
        {
            GameObject reward = Instantiate(rewardPrefab);

            reward.transform.SetParent(foodHolder, false);

            //x,y表示生成食物的位置
            x = Random.Range(-xlimit + xoffset, xlimit);

            y = Random.Range(-ylimit, ylimit);

            reward.transform.localPosition = new Vector3(x * 30, y * 30, 0);
        }
    }
}
