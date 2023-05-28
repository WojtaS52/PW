using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace ViewModelTests
{
    [TestClass]
    public class ViewModelTest
    {
        [TestClass] //działa :>
        public class SimulationViewModelTest
        {
            private readonly ViewModelSim simulationViewModel = new();

            [TestMethod]
            public void BallsCountPropertyChanged()
            {
                bool ballsCountChangedRaised = false;

                simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => ballsCountChangedRaised = true;

                Assert.IsFalse(ballsCountChangedRaised);

                simulationViewModel.LiczbaKulek = 15;
                Assert.IsTrue(ballsCountChangedRaised);
            }

            [TestMethod]
            public void StartStopSimulationTest()
            {
                bool isSimulationRunningChangedRaised = false;

                simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => isSimulationRunningChangedRaised = true;

                Assert.IsFalse(simulationViewModel.getSetFlag);
                Assert.IsFalse(isSimulationRunningChangedRaised);

                simulationViewModel.SimStart();
                Assert.IsTrue(simulationViewModel.getSetFlag);
                Assert.IsTrue(isSimulationRunningChangedRaised);

                simulationViewModel.SimStop();
                Assert.IsFalse(simulationViewModel.getSetFlag);
            }

            [TestMethod]
            public void UpdateBallsTest()
            {
                bool ballsChangedRaised = false;

                simulationViewModel.PropertyChanged += (object? sender, PropertyChangedEventArgs e) => ballsChangedRaised = true;

                Assert.IsFalse(ballsChangedRaised);
                var collectionBefore = simulationViewModel.Kulki;

                simulationViewModel.SimStart();
                simulationViewModel.OnNext(collectionBefore.First());

                /*Assert.IsTrue(ballsChangedRaised);*/
                var collectionAfter = simulationViewModel.Kulki;

                Assert.AreSame(collectionBefore, collectionAfter);
                simulationViewModel.SimStop();
            }
        }
    }
}
