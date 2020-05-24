using System;
using Windows.UI.Xaml.Controls;

namespace Services
{
    public interface IEventService
    {
        event EventHandler AppBarButtonClicked;
        void OnAppBarButtonClicked(AppBarButtonClickedEventArgs eventArgs);

        event EventHandler ChecklistChanged;
        void OnChecklistChanged(EventArgs eventArgs);

        event EventHandler PaneDisplayModeChangeChanged;
        void OnPaneDisplayModeChangeChanged(PaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs);
    }
    public class EventService : IEventService
    {
        public event EventHandler AppBarButtonClicked;
        public void OnAppBarButtonClicked(AppBarButtonClickedEventArgs eventArgs) => AppBarButtonClicked?.Invoke(this, eventArgs);

        public event EventHandler ChecklistChanged;
        public void OnChecklistChanged(EventArgs eventArgs) => ChecklistChanged?.Invoke(this, eventArgs);

        public event EventHandler PaneDisplayModeChangeChanged;
        public void OnPaneDisplayModeChangeChanged(PaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs) => PaneDisplayModeChangeChanged?.Invoke(this, paneDisplayModeChangedEventArgs);
    }
    public class AppBarButtonClickedEventArgs : EventArgs
    {
        public string Button { get; }
        public AppBarButtonClickedEventArgs(string button) => Button = button;
    }
    public class PaneDisplayModeChangedEventArgs : EventArgs
    {
        public int Mode { get; }
        public PaneDisplayModeChangedEventArgs(int mode) => Mode = mode;
    }
}
