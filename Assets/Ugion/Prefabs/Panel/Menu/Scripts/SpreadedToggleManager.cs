using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SpreadedToggleManager : MonoBehaviour
{
    public int lastSelectIndex = -1;
    private List<SingleSpreadedToggle> _toggles;
    public List<SingleSpreadedToggle> Toggles { get => _toggles; private set => _toggles = value; }

    private ToggleGroup group;

    public void Init()
    {
        group = transform.GetComponentInChildren<ToggleGroup>();
        group.allowSwitchOff = true;

        if (Toggles == null)
        {
            Toggles = transform.GetComponentsInChildren<SingleSpreadedToggle>().ToList();
        }

        for (int i = 0; i < Toggles.Count; i++)
        {
            Toggles[i].Init();
            Toggles[i].UnSelected();
            lastSelectIndex = -1;
            int index = i;
            Toggles[i].Toggle.onValueChanged.RemoveAllListeners();
            Toggles[i].OnSelectAction = null;
            Toggles[i].OnUnSelectAction = null;
            Toggles[i].Toggle.onValueChanged.AddListener((bool value) => OnTogglesValueChanged(value, index));

            AssignEvent();
        }

        SelectToggle(0);
    }

    private void SelectToggle(int index)
    {
        // lastSelectIndex = index;
        // Toggles[index].SelectState();
        Toggles[index].SelectState();
        Toggles[index].Toggle.isOn = true;
    }

    private void AssignEvent()
    {
        // Toggles[0].OnSelectAction += OnCheckStepSelect;
        // Toggles[0].OnUnSelectAction += OnCheckStepUnSelect;
    }

    public void OnTogglesValueChanged(bool value, int index)
    {
        //if (GameManager.Instance.mode && value)
        //{
        //    Debug.Log(Toggles[index].taskIndex);
        //    Debug.Log(FDatas.curIndex_流程序号);

        //    //if (Toggles[index].taskIndex < FDatas.curIndex_流程序号)
        //    //{
        //    //    Toggles[lastSelectIndex].Toggle.isOn = true;
        //    //    return;
        //    //}

        //    if (index < lastSelectIndex)
        //    {
        //        Toggles[lastSelectIndex].Toggle.isOn = true;
        //        return;
        //    }
        //}

        if (value)
        {
            if (lastSelectIndex == index)
            {
                return;
            }

            if (lastSelectIndex != -1)
            {
                Toggles[lastSelectIndex].UnSelected();
            }

            lastSelectIndex = index;
            Toggles[index].Selected();

            // 选中了就不许再空选
            group.allowSwitchOff = false;
        }
        else
        {
            lastSelectIndex = index;
            Toggles[index].UnSelected();
        }
    }

    public void UnInit()
    {
        for (int i = 0; i < Toggles.Count; i++)
        {
            Toggles[i].UnSelected();
            int index = i;
            Toggles[i].Toggle.onValueChanged.RemoveAllListeners();
            Toggles[i].OnSelectAction = null;
            Toggles[i].OnUnSelectAction = null;
        }
    }
}
