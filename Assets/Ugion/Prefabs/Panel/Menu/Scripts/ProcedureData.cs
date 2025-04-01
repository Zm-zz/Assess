using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProcedureData", menuName = "ScriptableObject/ProcedureData", order = 0)]
public class ProcedureData : ScriptableObject
{
    [Searchable]
    [TableList(ShowIndexLabels = true)]
    public List<ProcedureInfo> Procedures = new List<ProcedureInfo>();
}


[Serializable]
public class ProcedureInfo
{
    public ProcedureConfig ProcedureConfig;

    [LabelText("Extension")]
    public bool hasExtension;      // 是否有扩展流程

    [ShowIf("hasExtension")]
    [TableList(ShowIndexLabels = true)]
    public List<ProcedureConfig> extendedProcedures; // 扩展流程列表

    public ProcedureInfo(ProcedureConfig procedureConfig, bool hasExtension, List<ProcedureConfig> extendedProcedures)
    {
        this.ProcedureConfig = procedureConfig;
        this.hasExtension = hasExtension;
        this.extendedProcedures = extendedProcedures;
    }
}

[Serializable]
public struct ProcedureConfig
{
    [LabelText("Procedure")]
    [HorizontalGroup("BaseInfo")]
    public string procedureName;    // 流程唯一标识

    [LabelText("Title")]
    [HorizontalGroup("BaseInfo")]
    public string procedureTitle;  // 流程显示名称
}
