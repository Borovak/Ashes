using System;

namespace Interfaces
{
    public interface IDialogItemSimple : IDialogItem
    {
        Action ActionOnOk { get; }
    }
}