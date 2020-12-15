using System;
using System.Collections.Generic;
using UnityEngine;

public interface IDialogItemChoices : IDialogItem
{
    Dictionary<string, Action> followUp { get; }
}