using System.ComponentModel;
using Shared.Models;
using InventoryManagementApp.Services.Interfaces;
using InventoryManagementApp.ViewModels;
using Moq;
using InventoryManagementApp.Models;

namespace InventoryManagementAppTest
{
    public class Tests
    {
        private Mock<IServiceFactory> _mockServiceFactory;
        private Mock<IInventoryService> _mockInventoryService;
        private Mock<IFeatureManagerService> _mockFeatureManager;
        private Mock<ISeedDataService> _mockSeedDataService;
        private Mock<SearchBarViewModel> _mockSearchBarVM;
        private DataItemsViewModel _dataItemsVM;
        private EditBarViewModel _editBarVM;

        [SetUp]
        public void Setup()
        {
            _mockServiceFactory = new Mock<IServiceFactory>();
            _mockInventoryService = new Mock<IInventoryService>();
            _mockFeatureManager = new Mock<IFeatureManagerService>();
            _mockSeedDataService = new Mock<ISeedDataService>();
            _mockSearchBarVM = new Mock<SearchBarViewModel>();
            _dataItemsVM = new DataItemsViewModel();
            _editBarVM = new EditBarViewModel(_mockFeatureManager.Object, _mockServiceFactory.Object);
            
            _mockFeatureManager.Setup(f => f.IsUseSqlDb).Returns(false);
            _mockServiceFactory.Setup(f => f.GetService(It.IsAny<string>())).Returns(_mockInventoryService.Object);
            _mockSeedDataService.Setup(s => s.Seed());

            _mockInventoryService.Setup(s => s.GetItemsAsync(It.IsAny<int?>(), It.IsAny<int?>(),
                It.IsAny<Filters?>()))
                .ReturnsAsync(new List<InventoryItem>
            {
                new InventoryItem { Name = "Item1", Category = "Electronics", Quantity = 2 },
                new InventoryItem { Name = "Item2", Category = "Food", Quantity = 5 },
                new InventoryItem { Name = "Item3", Category = "Clothing", Quantity = 10 }
            }.AsEnumerable());
        }

        private MainWindowViewModel CreateVm()
        {
            return new MainWindowViewModel(
                _mockServiceFactory.Object,
                _mockFeatureManager.Object,
                _mockSeedDataService.Object,
                _mockSearchBarVM.Object,
                _dataItemsVM,
                _editBarVM);
        }

        [Test]
        public async Task Refresh_LoadsItemsAndSetsFilteredItems()
        {
            var vm = CreateVm();

            await vm.Refresh();

            var itemsView = vm.DataItemsVM.ItemsView;

            Assert.NotNull(itemsView);
            Assert.AreEqual(3, itemsView.Cast<object>().Count());

            Assert.IsTrue(itemsView.Cast<InventoryItem>().Any(i => i.Name == "Item1"));
        }

    }
}