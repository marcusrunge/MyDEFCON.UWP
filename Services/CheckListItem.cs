using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MyDEFCON_UWP.Models
{
    public class CheckListItem : INotifyPropertyChanged
    {
        #region Fields
        string _item = default(string);
        bool _checked = default(bool);
        double _fontSize = default(double);
        double _width = default(double);
        long _unixTimeStamp;
        short _defconStatus;
        #endregion

        #region Properties
        public string Item { get { return _item; } set { Set(ref _item, value);  } }
        public bool Checked { get { return _checked; } set { Set(ref _checked, value); } }
        public double FontSize { get { return _fontSize; } set { Set(ref _fontSize, value); } }
        public double Width { get { return _width; } set { Set(ref _width, value); } }
        public long UnixTimeStamp { get { return _unixTimeStamp; } set { Set(ref _unixTimeStamp, value); } }
        public short DefconStatus { get { return _defconStatus; } set { Set(ref _defconStatus, value); } }
        #endregion

        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
