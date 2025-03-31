using System;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ProcedureData", menuName = "ScriptableObject/ProcedureData", order = 0)]
public class ProcedureData : ScriptableObject
{
    public List<ProcedureConfig> Configs;

}

[Serializable]
public struct ProcedureConfig
{
    public string procedureName;
    public string procedureTitle;

    public bool isSpread;
    public List<ProcedureConfig> SpreadConfigs;
}
