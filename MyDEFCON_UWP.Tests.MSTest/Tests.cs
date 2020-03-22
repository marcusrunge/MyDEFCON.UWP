using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using MyDEFCON_UWP.ViewModels;

namespace MyDEFCON_UWP.Tests.MSTest
{
    // TODO WTS: Add appropriate tests
    [TestClass]
    public class Tests
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        // TODO WTS: Add tests for functionality you add to ChecklistViewModel.
        [TestMethod]
        public void TestChecklistViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new ChecklistViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to MainViewModel.
        [TestMethod]
        public void TestMainViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new MainViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to MessagesViewModel.
        [TestMethod]
        public void TestMessagesViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new MessagesViewModel();
            Assert.IsNotNull(vm);
        }

        // TODO WTS: Add tests for functionality you add to SettingsViewModel.
        [TestMethod]
        public void TestSettingsViewModelCreation()
        {
            // This test is trivial. Add your own tests for the logic you add to the ViewModel.
            var vm = new SettingsViewModel();
            Assert.IsNotNull(vm);
        }
    }
}
