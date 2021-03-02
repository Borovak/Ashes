using System;

namespace Interfaces
{
    public interface IOptionItemControl
    {
        event Action<string> ValueChanged;
        void SetValue(string value);
        string GetValue();
        void OnDLeftPressed();
        void OnDRightPressed();
    }
}