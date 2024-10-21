using DWIS.API.DTO;
using DWIS.MicroState.Model;
using Microsoft.CodeAnalysis;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DWIS.MicroState.UnitTests
{
    public class UtilsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            List<List<string>> variablesSet = new List<List<string>>();
            List<string>? commonVariables = Utils.CommonVariables(variablesSet);
            Assert.IsNull(commonVariables);
        }
        [Test]
        public void Test2()
        {
            List<List<string>> variablesSet = new List<List<string>>();
            variablesSet.Add(new List<string>() { "test" });
            List<string>? commonVariables = Utils.CommonVariables(variablesSet);
            Assert.IsNotNull(commonVariables);
            Assert.AreEqual(1, commonVariables.Count);
            Assert.AreEqual(variablesSet[0][0], commonVariables[0]);
        }

        [Test]
        public void Test3()
        {
            List<List<string>> variablesSet = new List<List<string>>();
            variablesSet.Add(new List<string>() { "test1", "test2" });
            variablesSet.Add(new List<string>() { "test1", "test2" });
            List<string>? commonVariables = Utils.CommonVariables(variablesSet);
            Assert.IsNotNull(commonVariables);
            Assert.AreEqual(2, commonVariables.Count);
            Assert.AreEqual(variablesSet[0][0], commonVariables[0]);
            Assert.AreEqual(variablesSet[0][1], commonVariables[1]);
        }
        [Test]
        public void Test4()
        {
            List<List<string>> variablesSet = new List<List<string>>();
            variablesSet.Add(new List<string>() { "test1", "test2", "test3" });
            variablesSet.Add(new List<string>() { "test1", "test2" });
            variablesSet.Add(new List<string>() { "test1",  });
            List<string>? commonVariables = Utils.CommonVariables(variablesSet);
            Assert.IsNotNull(commonVariables);
            Assert.AreEqual(1, commonVariables.Count);
            Assert.AreEqual(variablesSet[0][0], commonVariables[0]);
        }
        [Test]
        public void Test5()
        {
            List<List<string>> variablesSet = new List<List<string>>();
            variablesSet.Add(new List<string>() { "test1", });
            variablesSet.Add(new List<string>() { "test1", "test2", "test3" });
            variablesSet.Add(new List<string>() { "test1", "test2" });
            List<string>? commonVariables = Utils.CommonVariables(variablesSet);
            Assert.IsNotNull(commonVariables);
            Assert.AreEqual(1, commonVariables.Count);
            Assert.AreEqual(variablesSet[0][0], commonVariables[0]);
        }
    }
}
