using Checklists;
using LiveTile;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using MyDEFCON_UWP.Core.Eventaggregator;
using MyDEFCON_UWP.Core.Helpers;
using MyDEFCON_UWP.Services;
using MyDEFCON_UWP.ViewModels;
using Sockets;
using Storage;
using System;
using ToastNotifications;
using Unity;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP
{
    public sealed partial class App : Application
    {
        public IUnityContainer Container { get; set; }
        private readonly Lazy<ActivationService> _activationService;
        private ISockets _sockets;
        private IStorage _storage;

        private ActivationService ActivationService
        {
            get { return _activationService.Value; }
        }

        public App()
        {
            InitializeComponent();

            Container = new UnityContainer();
            RegisterContainer();
            EnteredBackground += App_EnteredBackground;
            Resuming += App_Resuming;

            // TODO WTS: Add your app in the app center and set your secret here. More at https://docs.microsoft.com/appcenter/sdk/getting-started/uwp
            AppCenter.Start("{Your App Secret}", typeof(Analytics), typeof(Crashes));

            // Deferred execution until used. Check https://msdn.microsoft.com/library/dd642331(v=vs.110).aspx for further info on Lazy<T> class.
            _activationService = new Lazy<ActivationService>(CreateActivationService);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {            
            if (!args.PrelaunchActivated)
            {
                await ActivationService.ActivateAsync(args);
            }
            await Container.Resolve<IChecklists>().Initialize();
            _sockets = Container.Resolve<ISockets>();
            _storage = Container.Resolve<IStorage>();
            if (_storage.Setting.GetSetting<bool>("LanBroadcastIsOn"))
            {
                await _sockets.Datagram.StartListener();
                _sockets.Datagram.IncomingMessageReceived += (s, e) =>
                  {
                      if (int.TryParse(e, out int parsedDefconStatus) && parsedDefconStatus > 0 && parsedDefconStatus < 6) _storage.Setting.SetSetting("defconStatus", parsedDefconStatus.ToString(), StorageStrategies.Roaming);
                  };
            }
            if (_storage.Setting.GetSetting<bool>("LanMulticastIsOn")) await _sockets.Stream.StartListener();
        }

        protected override async void OnActivated(IActivatedEventArgs args)
        {
            await ActivationService.ActivateAsync(args);
        }

        private ActivationService CreateActivationService()
        {
            return new ActivationService(this, typeof(Views.MainPage), Container.Resolve<ILiveTile>(), Container.Resolve<IStorage>(), new Lazy<UIElement>(CreateShell));
        }

        private UIElement CreateShell()
        {
            return new Views.ShellPage();
        }

        private async void App_EnteredBackground(object sender, EnteredBackgroundEventArgs e)
        {
            var deferral = e.GetDeferral();
            await Singleton<SuspendAndResumeService>.Instance.SaveStateAsync();
            deferral.Complete();
        }

        private void App_Resuming(object sender, object e)
        {
            Singleton<SuspendAndResumeService>.Instance.ResumeApp();
        }

        private void RegisterContainer()
        {            
            Container.RegisterType<MainViewModel>();
            Container.RegisterType<ChecklistViewModel>();
            Container.RegisterType<MessagesViewModel>();
            Container.RegisterType<SettingsViewModel>();
            Container.RegisterType<ShellViewModel>();
            Container.RegisterType<FullScreenViewModel>();
            Container.RegisterType<AboutPivotViewModel>();
            Container.RegisterType<SettingsPivotViewModel>();
            Container.RegisterInstance(StorageFactory.Create());
            Container.RegisterInstance(ChecklistsFactory.Create(/*Container.Resolve<IStorage>()*/));
            Container.RegisterInstance(LiveTileFactory.Create());
            Container.RegisterInstance(SocketsFactory.Create(/*Container.Resolve<IChecklists>()*/));
            Container.RegisterInstance(EventAggregatorFactory.Create());
            Container.RegisterInstance(ToastNotificationsFactory.Create());
        }
    }
}
