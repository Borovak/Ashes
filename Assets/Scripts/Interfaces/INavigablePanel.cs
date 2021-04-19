using System;
using Classes;

namespace Interfaces
{
    public interface INavigablePanel
    {
        event Action<int, Constants.PanelTypes> SelectedIndexChanged;
        event Action<DB.Item, Constants.PanelTypes> SelectedItemChanged;
        event Action ExitUp;
        event Action ExitDown;
        event Action ExitLeft;
        event Action ExitRight;
        event Action FocusGained;
        event Action FocusLost;
        event Action<INavigablePanel> FocusRequested;
        int SelectedIndex { get; set; }    
        bool CanExitUp { get; set; }    
        bool CanExitDown { get; set; }   
        bool CanExitLeft { get; set; }      
        bool CanExitRight { get; set; }   
        bool HasFocus { get; set; }
        Constants.PanelTypes PanelType { get; }
    }
}