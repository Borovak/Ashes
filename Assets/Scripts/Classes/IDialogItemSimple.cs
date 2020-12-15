using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogItemSimple : IDialogItem
{
    Action ActionOnOk { get; }
}