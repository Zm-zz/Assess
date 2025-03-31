using UnityGameFramework.Runtime;

public static class UIExtensionOther
{
    public static void CloseUIForm(this UIComponent uiComponent, UGUIForm uiForm)
    {
        uiComponent.CloseUIForm(uiForm.UIForm);
    }
}
