﻿using MyDEFCON_UWP.Core.Eventaggregator;
using MyDEFCON_UWP.Helpers;
using MyDEFCON_UWP.Services;
using MyDEFCON_UWP.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using WinUI = Microsoft.UI.Xaml.Controls;

namespace MyDEFCON_UWP.ViewModels
{
    public class ShellViewModel : Observable
    {
        private readonly KeyboardAccelerator _altLeftKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.Left, VirtualKeyModifiers.Menu);
        private readonly KeyboardAccelerator _backKeyboardAccelerator = BuildKeyboardAccelerator(VirtualKey.GoBack);

        private bool _isBackEnabled;
        private IList<KeyboardAccelerator> _keyboardAccelerators;
        private WinUI.NavigationView _navigationView;
        private WinUI.NavigationViewItem _selected;
        private ICommand _loadedCommand;
        private ICommand _itemInvokedCommand;
        private readonly IEventAggregator _eventAggregator;

        private string _visualState;
        public string VisualState { get => _visualState; set => Set(ref _visualState, value); }

        private WinUI.NavigationViewPaneDisplayMode _paneDisplayMode;
        public WinUI.NavigationViewPaneDisplayMode PaneDisplayMode { get => _paneDisplayMode; set => Set(ref _paneDisplayMode, value); }

        public bool IsBackEnabled
        {
            get => _isBackEnabled;
            set => Set(ref _isBackEnabled, value);
        }

        public WinUI.NavigationViewItem Selected
        {
            get => _selected;
            set => Set(ref _selected, value);
        }

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoaded));

        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));

        public ShellViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            PaneDisplayMode = WinUI.NavigationViewPaneDisplayMode.LeftCompact;
            _eventAggregator.Subscribe.PaneDisplayModeChangeChanged += (s, e) => PaneDisplayMode = (WinUI.NavigationViewPaneDisplayMode)e.Mode;
        }

        public void Initialize(Windows.UI.Xaml.Controls.Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
            _navigationView = navigationView;
            _keyboardAccelerators = keyboardAccelerators;
            NavigationService.Frame = frame;
            NavigationService.NavigationFailed += Frame_NavigationFailed;
            NavigationService.Navigated += Frame_Navigated;
            _navigationView.BackRequested += OnBackRequested;
        }

        private async void OnLoaded()
        {
            VisualState = "ShareState";
            // Keyboard accelerators are added here to avoid showing 'Alt + left' tooltip on the page.
            // More info on tracking issue https://github.com/Microsoft/microsoft-ui-xaml/issues/8
            _keyboardAccelerators.Add(_altLeftKeyboardAccelerator);
            _keyboardAccelerators.Add(_backKeyboardAccelerator);
            _eventAggregator.Subscribe.ChecklistChanged += (s, e) => { VisualState = "AddItemState"; };
            await Task.CompletedTask;
        }

        private void OnItemInvoked(WinUI.NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                NavigationService.Navigate(typeof(SettingsPage));
                return;
            }

            var item = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .First(menuItem => (string)menuItem.Content == (string)args.InvokedItem);
            var pageType = item.GetValue(NavHelper.NavigateToProperty) as Type;
            NavigationService.Navigate(pageType);
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
            NavigationService.GoBack();
        }

        private void Frame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw e.Exception;
        }

        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
            if (e.SourcePageType == typeof(SettingsPage))
            {
                Selected = _navigationView.SettingsItem as WinUI.NavigationViewItem;
                return;
            }

            Selected = _navigationView.MenuItems
                            .OfType<WinUI.NavigationViewItem>()
                            .FirstOrDefault(menuItem => IsMenuItemForPageType(menuItem, e.SourcePageType));
        }

        private bool IsMenuItemForPageType(WinUI.NavigationViewItem menuItem, Type sourcePageType)
        {
            var pageType = menuItem.GetValue(NavHelper.NavigateToProperty) as Type;
            return pageType == sourcePageType;
        }

        private static KeyboardAccelerator BuildKeyboardAccelerator(VirtualKey key, VirtualKeyModifiers? modifiers = null)
        {
            var keyboardAccelerator = new KeyboardAccelerator() { Key = key };
            if (modifiers.HasValue)
            {
                keyboardAccelerator.Modifiers = modifiers.Value;
            }

            keyboardAccelerator.Invoked += OnKeyboardAcceleratorInvoked;
            return keyboardAccelerator;
        }

        private static void OnKeyboardAcceleratorInvoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {
            var result = NavigationService.GoBack();
            args.Handled = result;
        }

        private ICommand _appBarButtonClickedCommand;

        public ICommand AppBarButtonClickedCommand => _appBarButtonClickedCommand ?? (_appBarButtonClickedCommand = new RelayCommand<object>((param) =>
        {
            _eventAggregator.Publish.OnAppBarButtonClicked(EventArgsFactory.CreateEventArgs<IAppBarButtonClickedEventArgs>((string)param));
            if ((string)param == "List" && VisualState == "DeleteItemsState") VisualState = "AddItemState";
        }));

        private ICommand _currentStateChangedCommand;

        public ICommand CurrentStateChangedCommand => _currentStateChangedCommand ?? (_currentStateChangedCommand = new RelayCommand<object>((param) =>
        {
            var newState = (param as VisualStateChangedEventArgs).NewState.Name;
            if (VisualState != newState) VisualState = newState;
        }));
    }
}