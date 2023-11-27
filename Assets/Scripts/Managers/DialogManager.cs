using System;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField]
    private Dialog activeDialog;

    void ShowDialog(Dialog dialog)
    {
        if (activeDialog is not null && activeDialog.IsActive)
        {
            activeDialog.Hide();
        }

        activeDialog = dialog;
        dialog.PlayText();
    }

    void HideActive()
    {
        if (activeDialog is not null && activeDialog.IsActive)
        {
            activeDialog.Hide();
        }
    }

    void SkipDialog()
    {
        if (activeDialog is not null && activeDialog.IsActive)
        {
            activeDialog.Skip = true;
        }
    }
}
