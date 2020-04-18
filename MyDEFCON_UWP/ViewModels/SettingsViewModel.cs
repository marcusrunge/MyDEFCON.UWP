using MyDEFCON_UWP.Helpers;
using System.Threading.Tasks;

namespace MyDEFCON_UWP.ViewModels
{
    // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
    public class SettingsViewModel : Observable
    {
        private AboutPivotViewModel _aboutPivotViewModel;
        public AboutPivotViewModel AboutPivotViewModel { get => _aboutPivotViewModel; set => Set(ref _aboutPivotViewModel, value); }

        private SettingsPivotViewModel _settingsPivotViewModel;
        public SettingsPivotViewModel SettingsPivotViewModel { get => _settingsPivotViewModel; set => Set(ref _settingsPivotViewModel, value); }

        public SettingsViewModel(SettingsPivotViewModel settingsPivotViewModel, AboutPivotViewModel aboutPivotViewModel)
        {
            SettingsPivotViewModel = settingsPivotViewModel;
            AboutPivotViewModel = aboutPivotViewModel;
        }

        public async Task InitializeAsync()
        {
            await AboutPivotViewModel.InitializeAsync();
        }
    }
}
