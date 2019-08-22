using DynamicConnections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethodCreateDatabase()
        {
            var retdrop = Program.CreateDatabase();
        }
        [TestMethod]
        public void TestMethodCreateClientTable()
        {
            var retcreate = Program.CreateClientTable();
        }
        [TestMethod]
        public void TestMethodDropClientTable()
        {
            var retdrop = Program.DropClientTable();
        }

        [DataRow(10)]
        [DataRow(20)]
        [DataRow(50)]
        [DataRow(100)]
        [DataTestMethod]
        public void TestMethodCreateClients(int n)
        {
            var retdrop = Program.FillClientTable(n);
        }

    }
}
