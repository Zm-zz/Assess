using GameFramework.Fsm;
using GameFramework.Procedure;
using Launch;
using UnityEngine;
using UnityEngine.Events;
using UnityGameFramework.Runtime;

[DisallowMultipleComponent]
public class GlobalComponent : GameFrameworkComponent
{
    public GameMode GameMode;

    private IFsm<IProcedureManager> procedureOwner;

    private EventDataManager _EventDataManager;
    private MenuManager _MenuManager;
    private CameraManager _CameraManager;


    public void Launch(IFsm<IProcedureManager> procedureOwner)
    {
        this.procedureOwner = procedureOwner;

        InitManagers();
    }

    private void InitManagers()
    {
        _EventDataManager = FindObjectOfType<EventDataManager>();
        _MenuManager = FindObjectOfType<MenuManager>();
        _CameraManager = FindObjectOfType<CameraManager>();

        _EventDataManager.Initialize();
        _MenuManager.Initialize();
        _CameraManager.Initialize();
    }

    public void MoveToTarget(string pointName, float motionTime = 0, UnityAction action = null, bool canMove = true, bool canLift = true)
    {
        _CameraManager.MoveToTarget(pointName, motionTime, action, canMove, canLift);
    }

    public void ChangeProcedure(string procedureName)
    {
        Main.Procedure.CurrentProcedure.ChangeState(procedureName, procedureOwner);
    }
}

public enum GameMode
{
    Train,
    Exam,
}
