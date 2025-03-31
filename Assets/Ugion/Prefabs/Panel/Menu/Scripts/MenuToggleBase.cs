using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 环节Menu基类
/// </summary>
public abstract class MenuToggleBase : MonoBehaviour
{
    public Toggle Toggle;
    public Text Label;
    public Image Image;

    [Header("展开")]
    public bool hasSpread;
    public SpreadedToggleManager spreadMgr;


    [HideInInspector]
    public string taskName;
    [HideInInspector]
    public int taskIndex;

    public virtual void Selected()
    {

    }
}