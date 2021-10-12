using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.UI.Xaml;

namespace Models
{
    public class CheckListItem : INotifyPropertyChanged
    {
        #region Fields

        private string _item = default;
        private bool _checked = default;
        private bool _deleted = default;
        private double _fontSize = default;
        private double _width = default;
        private long _unixTimeStampCreated;
        private long _unixTimeStampUpdated;
        private short _defconStatus;
        private int _id;
        private Visibility _visibility;
        #endregion Fields



        #region Properties

        public int Id { get { return _id; } set { Set(ref _id, value); } }
        public long UnixTimeStampCreated { get { return _unixTimeStampCreated; } set { Set(ref _unixTimeStampCreated, value); } }
        public long UnixTimeStampUpdated { get { return _unixTimeStampUpdated; } set { Set(ref _unixTimeStampUpdated, value); } }
        public short DefconStatus { get { return _defconStatus; } set { Set(ref _defconStatus, value); } }
        public string Item { get { return _item; } set { Set(ref _item, value); UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds(); } }
        public bool Checked { get { return _checked; } set { Set(ref _checked, value); UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds(); } }
        public bool Deleted { get { return _deleted; } set { Set(ref _deleted, value); } }
        public Visibility Visibility { get { return _visibility; } set { Set(ref _visibility, value); } }
        public double FontSize { get { return _fontSize; } set { Set(ref _fontSize, value); } }
        public double Width { get { return _width; } set { Set(ref _width, value); } }

        #endregion Properties

        #region NotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }

        #endregion NotifyPropertyChanged
    }
}