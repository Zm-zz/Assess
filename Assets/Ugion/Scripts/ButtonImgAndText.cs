/*using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
//                 _ooOoo_
//                o8888888o
//                88" . "88
//                (| -_- |)
//                O\  =  /O
//             ____/`---'\____
//           .'  \\|     |//  `.
//          /  \\|||  :  |||//  \
//         / _ ||||| -:- ||||| - \
//         |   | \\\  -  /// |   |
//         | \_|  ''\---/ '' |_/ |
//         \  .-\__  `-`  __/-.  /
//       ___`. .'  /--.--\  `. . ___
//    ."" '<  `.___\_<|>_/___.'  > '"".
//   | | :  `- \`.;`\ _ /`;.`/ -`  : | |
//   \  \ `-.   \_ __\ /__ _/   .-` /  /
//====`-.____`-.___\_____/___.-`____.-'======
//                 `=---='
//^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
//          拜赛博佛祖       积电子功德
public class ButtonImgAndText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    #region 声明变量
    private Image image;
    private Text text;
    private Button button;

    [Header("按钮背景图片")]
    [Tooltip("按钮默认背景")]
    public Sprite normalSprite;
    [Tooltip("按钮移入背景")]
    public Sprite highLightSprite;
    [Tooltip("按钮按下背景")]
    public Sprite pressSprite;

    [Header("文字颜色")]
    [Tooltip("文字默认背景")]
    public Color normalText = new(0, 0, 0, 1);
    [Tooltip("文字移入背景")]
    public Color highLightText = new(0.3764706f, 0.7450981f, 0.7019608f, 1);
    [Tooltip("文字按下背景")]
    public Color pressText = new(1, 1, 1, 1);

    //[Header("按钮设置")]
    //[Tooltip("字体大小")]
    //public int fontSize = 14;
    //[Tooltip("文字内容")]
    //public string textContent = "开始训练";
    [Tooltip("是否成组")]
    public bool isToggle = false;
    [Tooltip("按钮组")]
    public List<Button> ToggleGroup = new List<Button>();
    [Tooltip("选中显示图标")]
    public Image graphic;
    [Tooltip("该按钮初始时是否被按下（请保证每组Toggle中只有一个isInitPress是true）")]
    public bool isInitPress;
    [Tooltip("是否记录上次被按下按钮（请保证每组Toggle的isRecordLastToggle保持一致）")]
    public bool isRecordLastToggle;
    #endregion

    #region 生命周期
    private void Awake()
    {
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
        button = GetComponent<Button>();
        //text.fontSize = fontSize;
        //text.text = textContent;
        if (isToggle)
        {
            if (ToggleGroup.Count == 0)
            {
                Transform father = transform.parent;
                for (int i = 0; i < father.childCount; i++)
                {
                    //有点问题
                    ToggleGroup.Add(father.GetChild(i).GetComponent<Button>());
                }
            }
        }
        button.onClick.AddListener(() =>
        {
            //点击事件
            Debug.Log(name + "的点击事件");
        });
    }

    private void OnEnable()
    {
        EventCenter.AddListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
        EventCenter.AddListener(EventDefine.IMInitPress, () => { isInitPress = false; });
        if (isToggle && isInitPress)
        {
            image.sprite = pressSprite;
            text.color = pressText;
            if (graphic != null )
            {
                if (graphic.sprite != null)
                graphic.gameObject.SetActive(true);
            }
            button.onClick.Invoke();
        }
        else 
        {
            image.sprite = normalSprite;
            text.color = normalText;
            if (isToggle && graphic!= null )
            {
                if (graphic.sprite != null)
                graphic.gameObject.SetActive(false);
            }
        }
    }

    private void OnDisable()
    {
        EventCenter.RemoveListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
        EventCenter.RemoveListener(EventDefine.IMInitPress, () => { isInitPress = false; });
    }
    #endregion

    #region 鼠标的点击事件
    // 当鼠标进入按钮时触发的事件
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image.sprite == normalSprite)
        {
            image.sprite = highLightSprite;
            text.color = highLightText;
        }
    }

    // 当鼠标离开按钮时触发的事件
    public void OnPointerExit(PointerEventData eventData)
    {
        if(isToggle)
        {
            if (image.sprite == highLightSprite)
            {
                image.sprite = normalSprite;
                text.color = normalText;
            }
        }
        else
        {
            if (image.sprite == highLightSprite || image.sprite == pressSprite)
            {
                image.sprite = normalSprite;
                text.color = normalText;
            }
        }
    }

    // 当鼠标点击按钮时触发的事件
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isToggle)
        {
            if (image.sprite == highLightSprite)
            {
                image.sprite = pressSprite;
                text.color = pressText;
                if(graphic != null )
                {
                    if (graphic.sprite != null)
                    graphic.gameObject.SetActive(true);
                }
                if(isRecordLastToggle)
                {
                    isInitPress = true;
                    EventCenter.RemoveListener(EventDefine.IMInitPress, () => { isInitPress = false; });
                    EventCenter.Broadcast(EventDefine.IMInitPress);
                    EventCenter.AddListener(EventDefine.IMInitPress, () => { isInitPress = false; });
                }
                EventCenter.RemoveListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);//防止自己接到
                EventCenter.Broadcast(EventDefine.ButtonCanelPress,transform.parent.name);
                EventCenter.AddListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
            }
        }
    }

    // 当鼠标按下按钮时触发的事件
    public void OnPointerDown(PointerEventData eventData)
    {
        if (!isToggle)
        {
            if (image.sprite == highLightSprite)
            {
                image.sprite = pressSprite;
                text.color = pressText;
            }
        }
    }

    //// 当鼠标松开按钮时触发的事件
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isToggle)
        {
            if (image.sprite == pressSprite)
            {
                image.sprite = highLightSprite;
                text.color = highLightText;
            }
        }
    }
    #endregion

    #region 委托方法
    private void ButtonCancelPress(string toggleGroupName)
    {
        if(transform!=null)
        {
            if (transform.parent.name == toggleGroupName)
            {
                image.sprite = normalSprite;
                text.color = normalText;
                if (graphic != null )
                {
                    if (graphic.sprite != null)
                    graphic.gameObject.SetActive(false);
                }
            }
        }
        
    }
    #endregion

    public void ChangeButtonSprite(string toggleGroupName)
    {
        image.sprite = pressSprite;
        text.color = pressText;
        EventCenter.RemoveListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);//防止自己接到
        EventCenter.Broadcast(EventDefine.ButtonCanelPress, toggleGroupName);
        EventCenter.AddListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
    }
}
#region Inspector面板显示
#if UNITY_EDITOR
[CustomEditor(typeof(ButtonImgAndText))]
public class MyComponentEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("fontSize"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("textContent"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("isToggle"));
        if (serializedObject.FindProperty("isToggle").boolValue)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ToggleGroup"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("graphic"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isInitPress"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isRecordLastToggle"));
        }
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("highLightSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pressSprite"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("normalText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("highLightText"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("pressText"));
        serializedObject.ApplyModifiedProperties();
    }
} 
#endif
#endregion


*/