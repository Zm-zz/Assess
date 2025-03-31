using GameFramework.Fsm;
using GameFramework.Procedure;
using Launch;
using UnityEngine;

public class GlobalManager : SingletonMonoIdlerAuto<GlobalManager>
{
    public IFsm<IProcedureManager> procedureOwner;

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            ChangeState("Procedure_InGame");
        }
    }

    public void ChangeState(string stateName)
    {
        Main.Procedure.CurrentProcedure.ChangeState(stateName, procedureOwner);
    }
}
