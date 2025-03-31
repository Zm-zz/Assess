using System;
using UnityEngine.UI;

public class SingleToggle : MenuToggleBase
{
    public Action OnSelectAction;
    public Action OnUnSelectAction;

    public bool IsOn => Toggle.isOn;


    public void Init()
    {
        if (hasSpread)
        {
            spreadMgr.gameObject.SetActive(false);
        }

        Toggle.isOn = false;
    }

    public override void Selected()
    {
        SelectState();

        if (hasSpread)
        {
            spreadMgr.gameObject.SetActive(true);
            spreadMgr.Init();
        }
        else
        {
            // FTaskManager.Instance.EnterTask(taskName);
        }
    }

    public void UnSelected()
    {
        UnSelectState();

        if (hasSpread)
        {
            spreadMgr.gameObject.SetActive(false);
        }
    }

    public void SelectState()
    {
    }

    public void UnSelectState()
    {
    }
}
