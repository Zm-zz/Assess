using GameFramework.Fsm;
using GameFramework.Procedure;
using Launch;
using UnityEngine;

public class GlobalManager : SingletonMonoIdlerAuto<GlobalManager>
{
    private IFsm<IProcedureManager> procedureOwner;

    private MenuManager _MenuManager;
    private EventDataManager _EventDataManager;


    public void Launch(IFsm<IProcedureManager> procedureOwner)
    {
        this.procedureOwner = procedureOwner;

        _MenuManager = FindObjectOfType<MenuManager>();
        _EventDataManager = FindObjectOfType<EventDataManager>();

        InitManagers();
    }


    private void InitManagers()
    {
        _MenuManager.Initialize();
        _EventDataManager.Initialize();
    }

    public void ChangeProcedure(string procedureName)
    {
        Main.Procedure.CurrentProcedure.ChangeState(procedureName, procedureOwner);
    }
}
