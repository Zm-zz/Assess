using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuManager : MonoBehaviour
{
    [Header("Create")]
    public GameObject pre_ParMenu;
    public Transform trans_Parent;

    public ProcedureData procedureData;

    [SerializeField]
    private List<OptionBase> options = new List<OptionBase>();

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        procedureData.Configs.ForEach(i =>
        {
            CreateOption(false, i.isSpread, i.procedureName, i.procedureTitle);
        });
    }

    public void CreateOption(bool isOn, bool isSpread, string procedureName, string procedureTitle, UnityAction clickAction = null)
    {
        OptionBase option = Instantiate(pre_ParMenu, trans_Parent).GetComponent<OptionBase>();
        option.Initialize(isOn, isSpread, this, procedureName, procedureTitle, clickAction = null);

        options.Add(option);
    }

    public void ChangeState(OptionBase currOption)
    {
        options.ForEach(i =>
        {
            if (i != currOption && i.Bool_IsOn == true)
            {
                i.ChangeState(false);
            }
        });
    }
}
