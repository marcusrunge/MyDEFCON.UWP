using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Services
{
    public interface IEventService
    {
        event EventHandler AppBarButtonClicked;
        void OnAppBarButtonClicked(AppBarButtonClickedEventArgs eventArgs);

        event EventHandler ChecklistChanged;
        void OnChecklistChanged(EventArgs eventArgs);
    }
    public class EventService : IEventService
    {
        public event EventHandler AppBarButtonClicked;
        public void OnAppBarButtonClicked(AppBarButtonClickedEventArgs eventArgs) => AppBarButtonClicked?.Invoke(this, eventArgs);

        public event EventHandler ChecklistChanged;
        public void OnChecklistChanged(EventArgs eventArgs) => ChecklistChanged?.Invoke(this, eventArgs);
    }
    public class AppBarButtonClickedEventArgs : EventArgs
    {
        public string Button { get; }
        public AppBarButtonClickedEventArgs(string button) => Button = button;
    }
}
