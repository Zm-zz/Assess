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
    public bool hasExtension;      // �Ƿ�����չ����

    [ShowIf("hasExtension")]
    [TableList(ShowIndexLabels = true)]
    public List<ProcedureConfig> extendedProcedures; // ��չ�����б�

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
    public string procedureName;    // ����Ψһ��ʶ

    [LabelText("Title")]
    [HorizontalGroup("BaseInfo")]
    public string procedureTitle;  // ������ʾ����
}
