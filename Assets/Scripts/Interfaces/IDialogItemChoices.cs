using System;
using System.Collections.Generic;

namespace Interfaces
{
    public interface IDialogItemChoices : IDialogItem
    {
        Dictionary<string, Action> followUp { get; }
    }
}