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
            ChangeProcedure("Procedure_InGame");
        }
    }

    public void ChangeProcedure(string procedureName)
    {
        Main.Procedure.CurrentProcedure.ChangeState(procedureName, procedureOwner);
    }
}
