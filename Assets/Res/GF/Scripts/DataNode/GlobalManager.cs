using GameFramework.Fsm;
using GameFramework.Procedure;
using JKFrame;
using Launch;
using UnityEngine;
using UnityGameFramework.Runtime;

public class GlobalManager : SingletonMono<GlobalManager>
{
    public static IFsm<IProcedureManager> procedureOwner;
    void Start()
    {
        Main.DataNode.SetData<VarInt32>("Player.Id", 10010);
        Main.DataNode.SetData<VarString>("Player.Name", "Test");
        Main.DataNode.SetData<VarBoolean>("Player.Sex", true);

        var id = Main.DataNode.GetData("Player.Id");
        var name = Main.DataNode.GetData("Player.Name");
        var sex = Main.DataNode.GetData("Player.Sex");

        Log.Info($"ID = {id},Name = {name},Sex = {sex}");
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            ChangeState("Procedure_InGame");
        }
    }

    public static void ChangeState(string stateName)
    {
        Main.Procedure.CurrentProcedure.ChangeState(stateName, procedureOwner);
    }
}
