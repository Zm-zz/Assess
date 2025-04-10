using GameFramework.Fsm;
using GameFramework.Procedure;
using Launch;

public class GlobalManager : SingletonMonoIdlerAuto<GlobalManager>
{
    public GameMode GameMode = GameMode.Train;

    private IFsm<IProcedureManager> procedureOwner;

    private MenuManager _MenuManager;
    private EventDataManager _EventDataManager;


    public void Launch(IFsm<IProcedureManager> procedureOwner)
    {
        this.procedureOwner = procedureOwner;

        InitManagers();
    }


    private void InitManagers()
    {
        _MenuManager = FindObjectOfType<MenuManager>();
        _EventDataManager = FindObjectOfType<EventDataManager>();

        _MenuManager.Initialize();
        _EventDataManager.Initialize();
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
