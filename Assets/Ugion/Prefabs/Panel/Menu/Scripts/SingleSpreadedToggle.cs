using System;
using UnityEngine.UI;

public class SingleSpreadedToggle : MenuToggleBase
{
    public Action OnSelectAction;
    public Action OnUnSelectAction;

    public bool IsOn => Toggle.isOn;


    public void Init()
    {
        Toggle.isOn = false;
    }

    public override void Selected()
    {
        SelectState();

        //if (FDatas.curIndex_Á÷³ÌÐòºÅ != taskIndex)
        //{
          //  FTaskManager.Instance.EnterTask(taskName);
        //}
    }

    public void UnSelected()
    {
        UnSelectState();
    }

    public void SelectState()
    {
    }

    public void UnSelectState()
    {
        // Toggle.isOn = false;
    }
}
