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
//          ����������       �����ӹ���
public class ButtonImgAndText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler
{
    #region ��������
    private Image image;
    private Text text;
    private Button button;

    [Header("��ť����ͼƬ")]
    [Tooltip("��ťĬ�ϱ���")]
    public Sprite normalSprite;
    [Tooltip("��ť���뱳��")]
    public Sprite highLightSprite;
    [Tooltip("��ť���±���")]
    public Sprite pressSprite;

    [Header("������ɫ")]
    [Tooltip("����Ĭ�ϱ���")]
    public Color normalText = new(0, 0, 0, 1);
    [Tooltip("�������뱳��")]
    public Color highLightText = new(0.3764706f, 0.7450981f, 0.7019608f, 1);
    [Tooltip("���ְ��±���")]
    public Color pressText = new(1, 1, 1, 1);

    //[Header("��ť����")]
    //[Tooltip("�����С")]
    //public int fontSize = 14;
    //[Tooltip("��������")]
    //public string textContent = "��ʼѵ��";
    [Tooltip("�Ƿ����")]
    public bool isToggle = false;
    [Tooltip("��ť��")]
    public List<Button> ToggleGroup = new List<Button>();
    [Tooltip("ѡ����ʾͼ��")]
    public Image graphic;
    [Tooltip("�ð�ť��ʼʱ�Ƿ񱻰��£��뱣֤ÿ��Toggle��ֻ��һ��isInitPress��true��")]
    public bool isInitPress;
    [Tooltip("�Ƿ��¼�ϴα����°�ť���뱣֤ÿ��Toggle��isRecordLastToggle����һ�£�")]
    public bool isRecordLastToggle;
    #endregion

    #region ��������
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
                    //�е�����
                    ToggleGroup.Add(father.GetChild(i).GetComponent<Button>());
                }
            }
        }
        button.onClick.AddListener(() =>
        {
            //����¼�
            Debug.Log(name + "�ĵ���¼�");
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

    #region ���ĵ���¼�
    // �������밴ťʱ�������¼�
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (image.sprite == normalSprite)
        {
            image.sprite = highLightSprite;
            text.color = highLightText;
        }
    }

    // ������뿪��ťʱ�������¼�
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

    // ���������ťʱ�������¼�
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
                EventCenter.RemoveListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);//��ֹ�Լ��ӵ�
                EventCenter.Broadcast(EventDefine.ButtonCanelPress,transform.parent.name);
                EventCenter.AddListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
            }
        }
    }

    // ����갴�°�ťʱ�������¼�
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

    //// ������ɿ���ťʱ�������¼�
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

    #region ί�з���
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
        EventCenter.RemoveListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);//��ֹ�Լ��ӵ�
        EventCenter.Broadcast(EventDefine.ButtonCanelPress, toggleGroupName);
        EventCenter.AddListener<string>(EventDefine.ButtonCanelPress, ButtonCancelPress);
    }
}
#region Inspector�����ʾ
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