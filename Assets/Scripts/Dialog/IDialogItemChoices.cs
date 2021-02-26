using System;
using System.Collections.Generic;

namespace Dialog
{
    public interface IDialogItemChoices : IDialogItem
    {
        Dictionary<string, Action> FollowUp { get; }
    }
}