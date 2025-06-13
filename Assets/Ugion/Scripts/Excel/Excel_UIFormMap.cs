using System;
using UnityEngine;

[Serializable]
public class Excel_UIFormMap
{
    public int ID;
    public string Name;
    public string AssetName;
    public GroupType Group;
    public bool WhetherAllowMulInstance;
    public bool WhetherPauseInterfaceOverwrittenByIt;
}

public enum GroupType
{
    Default,
    UIForm,
    Entity,
}
