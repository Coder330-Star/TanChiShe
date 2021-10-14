using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SnakeHead : MonoBehaviour {

    public List<Transform> bodylist = new List<Transform>();//用来存储蛇身

    public float volecity = 0.35f;
    public int step = 30; //步长表示每次蛇向前移动的距离
    private float x;//x偏移量
    private float y;//y偏移量
    private Vector3 headPos;//记录蛇头的其起始位置
    private bool isDie;

    public Transform canvas;

    public GameObject bodyPrefabs;
    public GameObject dieEffect;

    public AudioClip eatClip;
    public AudioClip dieClip;
  
    public Sprite[] sprite = new Sprite[2];//蛇身的两个贴图数组

    void Awake() 
    {
        //通过Resources.Load(path)方法加载资源，path的书写不需要加Resources/以及文件扩展名
        gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
        sprite[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
        sprite[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));

    }

    void Start() 
    {
        InvokeRepeating("Move", 0, volecity);
        x = 0; y = step;
    }

    //用空格键，W、A、S、D键控制移动
    void Update() 
    {
        #region 表示按下空格键可以加速

        if (Input.GetKeyDown(KeyCode.Space) && UIManager.Instance.ispause ==false && isDie ==false) 
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, volecity - 0.2f);
        }
        if (Input.GetKeyUp(KeyCode.Space) && UIManager.Instance.ispause == false && isDie == false) 
        {
            CancelInvoke();
            InvokeRepeating("Move", 0, volecity);
        }

        #endregion

        #region 控制移动
        //上
        if (Input.GetKey(KeyCode.W) && y != -step && UIManager.Instance.ispause == false && isDie == false) 
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            x = 0; y = step;
        }

        //下
        if (Input.GetKey(KeyCode.S) && y != step && UIManager.Instance.ispause == false && isDie ==false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
            x = 0; y = -step;
        }

        //左
        if (Input.GetKey(KeyCode.A) && x != step && UIManager.Instance.ispause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
            x = -step; y = 0;
        }

        //右
        if (Input.GetKey(KeyCode.D) && x != step && UIManager.Instance.ispause == false && isDie == false)
        {
            gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
            x = step; y = 0;
        }

        #endregion
    }

    //控制蛇移动的方法
    void Move() 
    {
        //蛇头的位置
        headPos = gameObject.transform.localPosition;

        //控制蛇头的移动，因为是二维，所以z轴不需要变
        gameObject.transform.localPosition = new Vector3(headPos.x+x,headPos.y+y,headPos.z);

        #region 控制蛇身的移动
        if (bodylist.Count > 0)
        {
            #region 方法一：通过将蛇身的最后一个放到原来蛇头的位置，来控制蛇身移动
            //bodylist.Last().localPosition = headPos;    //将最后一个蛇身移动到头的位置
            //bodylist.Insert(0, bodylist.Last());         //将蛇尾插入到蛇身的第一个位置
            //bodylist.RemoveAt(bodylist.Count - 1);       //把最后一个蛇尾的记录移除
            #endregion

            #region 通过将蛇身全部向前移的方法控制蛇身移动，将最后一个移到前一个的位置，将倒二的位置移到倒三的位置以此类推

            for (int i = bodylist.Count - 2; i >= 0; i--) 
            {
                bodylist[i + 1].localPosition = bodylist[i].localPosition;
            }
            bodylist[0].localPosition = headPos;
            #endregion
        }

        #endregion
    }

    //死亡状态
    void Die() 
    {
        AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);
        CancelInvoke();
        PlayerPrefs.SetInt("lastl", UIManager.Instance.length);
        PlayerPrefs.SetInt("lasts", UIManager.Instance.score);
        if (PlayerPrefs.GetInt("bests",0) < PlayerPrefs.GetInt("lasts", UIManager.Instance.score)) 
        {
            PlayerPrefs.SetInt("bestl", UIManager.Instance.length);
            PlayerPrefs.SetInt("bests", UIManager.Instance.score);
        }
        isDie = true;
        Instantiate(dieEffect);
        StartCoroutine(GameOver(1.5f));
    }

    //协程，表示死亡后重新加载场景
    IEnumerator GameOver(float t) 
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(1);
    }


    void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.gameObject.CompareTag("Food")) 
        {
            Destroy(other.gameObject);
            UIManager.Instance.UpdateUI();
            BodyGrow();
            CreateFood.Instance.Food((Random.Range(0, 101) < 20) ? true : false);
           
        }
        if (other.gameObject.CompareTag("Reward")) 
        {
            Destroy(other.gameObject);
            UIManager.Instance.UpdateUI(Random.Range(5,15)*10);
            BodyGrow();
        }
        if (other.gameObject.CompareTag("Body")) 
        {
            Die();
        }
        if (other.gameObject.CompareTag("Border")) 
        {
            if (UIManager.Instance.border)
            {
                Die();
            }
            else
            {
                switch (other.gameObject.name)
                {
                    case "Up":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y + 30, transform.localPosition.z);
                        break;
                    case "Down":
                        transform.localPosition = new Vector3(transform.localPosition.x, -transform.localPosition.y - 3, transform.localPosition.z);
                        break;
                    case "Left":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 210, transform.localPosition.y, transform.localPosition.z);
                        break;
                    case "Right":
                        transform.localPosition = new Vector3(-transform.localPosition.x + 270, transform.localPosition.y, transform.localPosition.z);
                        break;
                }
            }
        }
    }

    //表示吃到食物，蛇身长大
    void BodyGrow() 
    {
        AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);
        int index = (bodylist.Count % 2 == 0) ? 0 : 1; //奇数个与偶数个图片不同
        GameObject body = Instantiate(bodyPrefabs);
        body.GetComponent<Image>().sprite = sprite[index]; 
        body.transform.SetParent(canvas, false);
        bodylist.Add(body.transform);

    }
}
