using Gymnastika.Modules.Sports.Views;
using System;
using Gymnastika.Modules.Sports.Services;
using GongSolutions.Wpf.DragDrop;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Gymnastika.Modules.Sports.Models;
using System.ComponentModel;
using NUnit.Framework;
using Moq;
using Gymnastika.Modules.Sports.ViewModels;

namespace Gymnastika.Modules.Sports.Test
{

    [TestFixture]
    public class SportsListViewModelTests
    {

        private TestContext testContextInstance;
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region
        ObservableCollection<SportsCategory> Categories
        {
            get
            {
                return new ObservableCollection<SportsCategory>()
                {

                    new SportsCategory()
                    {
                        Sports = new ObservableCollection<Sport>()
                        {
                            new Sport()
                            {
                                Name = "Sport"
                            },
                                                        new Sport()
                            {
                                Name = "Sport2"
                            },
                                                        new Sport()
                            {
                                Name = "Sport3"
                            },
                                                        new Sport()
                            {
                                Name = "Sport4"
                            }
                            ,                            new Sport()
                            {
                                Name = "Sport5"
                            },
                                                        new Sport()
                            {
                                Name = "Sport6"
                            },
                                                        new Sport()
                            {
                                Name = "Sport7"
                            }
                        }
                    }
                };
            }
        }
        Mock<ISportsProvider> GetProviderMock()
        {
            var mock = new Mock<ISportsProvider>();
            mock.Setup(n => n.SportsCategories).Returns(() => Categories);
            return mock;
        }

        #endregion
       
        [Test]
        public void AfterConstructedPropertiesShouldHaveValues()
        {
            var mock = GetProviderMock();

            var provider = mock.Object;

            var model = new SportsListViewModel(provider);

            Assert.AreEqual(model.CurrentPage, 1);

            Assert.NotNull(model.Categories);

            Assert.NotNull(model.CloseCommand);

            Assert.AreEqual(model.CurrentSports.Count, 7);

            Assert.AreEqual(model.CurrentSports[0].Name, "Sport");

            Assert.NotNull(model.NextPageCommand);

            Assert.NotNull(model.PreviousPageCommand);

            Assert.AreEqual(model.SelectedCategory, model.Categories[0]);

            Assert.AreEqual(model.SelectedSport.Name, "Sport");

            Assert.NotNull(model.ShowMoreCommand);

            Assert.IsNull(model.SportsFilter);

            Assert.IsNullOrEmpty(model.SportsNameFilter);

            Assert.NotNull(model.SportsView);

            Assert.NotNull(model.GotoPageCommand);
        }


        [Test]
        public void ThenCommandsShouldRespondCorrectly()
        {
            var mock = GetProviderMock();
            ISportsProvider provider = mock.Object;
            var model = new SportsListViewModel(provider);
            Assert.False(model.CloseCommand.CanExecute(null));
            Assert.True(model.NextPageCommand.CanExecute(null));
            Assert.False(model.PreviousPageCommand.CanExecute(null));
            Assert.False(model.ShowMoreCommand.CanExecute(null));
            Assert.AreEqual(1, model.CurrentPage);
            model.NextPageCommand.Execute(null);
            Assert.AreEqual(2, model.CurrentPage);
            Assert.False(model.NextPageCommand.CanExecute(null));
            Assert.True(model.PreviousPageCommand.CanExecute(null));
            model.PreviousPageCommand.Execute(null);
            Assert.False(model.PreviousPageCommand.CanExecute(null));

            Assert.False(model.GotoPageCommand.CanExecute(0));
            Assert.False(model.GotoPageCommand.CanExecute(3));
            Assert.True(model.GotoPageCommand.CanExecute(1));
            Assert.True(model.GotoPageCommand.CanExecute(2));
            model.GotoPageCommand.Execute(1);
            Assert.AreEqual(model.CurrentPage, 1);
        }

        
    }
}
