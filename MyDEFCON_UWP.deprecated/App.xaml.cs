using MyDEFCON_UWP.Services.SettingsServices;
using System.Threading.Tasks;
using Template10.Common;
using Template10.Controls;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyDEFCON_UWP
{
    /// Documentation on APIs used in this page:
    /// https://github.com/Windows-XAML/Template10/wiki

    [Bindable]
    sealed partial class App : BootStrapper
    {
        public App()
        {
            InitializeComponent();
            SplashFactory = (e) => new Views.Splash(e);

            #region app settings

            // some settings must be set in app.constructor
            var settings = SettingsService.Instance;
            RequestedTheme = settings.AppTheme;
            CacheMaxDuration = settings.CacheMaxDuration;
            ShowShellBackButton = settings.UseShellBackButton;
            AutoSuspendAllFrames = true;
            AutoRestoreAfterTerminated = true;
            AutoExtendExecutionSession = true;
            #endregion
        }

        public override UIElement CreateRootElement(IActivatedEventArgs e)
        {
            var service = NavigationServiceFactory(BackButton.Attach, ExistingContent.Exclude);
            return new ModalDialog
            {
                DisableBackButtonWhenModal = true,
                Content = new Views.Shell(service),
                ModalContent = new Views.Busy(),
            };
        }

        public override async Task OnStartAsync(StartKind startKind, IActivatedEventArgs args)
        {
            // TODO: add your long-running task here
            var appView = ApplicationView.GetForCurrentView();
            appView.TitleBar.ButtonBackgroundColor = Color.FromArgb(255, 232, 255, 0);
            appView.TitleBar.ForegroundColor = Colors.Red;
            appView.TitleBar.BackgroundColor = Color.FromArgb(255, 232, 255, 0);
            appView.TitleBar.ButtonForegroundColor = Colors.Red;
            appView.TitleBar.ButtonHoverBackgroundColor = Colors.Red;
            appView.TitleBar.ButtonHoverForegroundColor = Color.FromArgb(255, 232, 255, 0);            
            await NavigationService.NavigateAsync(typeof(Views.MainPage));
        }
    }
}

