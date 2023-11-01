using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows.Forms;
using Roznitsa;

namespace UnitTestsDatabase
{
    [TestClass]
    public class DatabaseUnitTests
    {
        string sqlConnectionString = @"Data Source=DARRSO\DARRSO;Initial Catalog=""Розничная торговля"";Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        [TestMethod]
        public void SelectAllDataFromFirmaToDataGridView()
        {
            string query = Firma.Queries.SelectAllFirmas;
            DataGridView testingDGV = new DataGridView();

            testingDGV.BindingContext = new BindingContext();

            Database.SelectQueryToDataGridView(sqlConnectionString, query, testingDGV);

            Assert.AreEqual(9, testingDGV.Rows.Count);
        }
        [TestMethod]
        public void FindDataFromTovarToDataGridView()
        {
            string query = Tovar.Queries.FindTovar + " [Код товара] = 0";
            DataGridView testingDGV = new DataGridView();

            testingDGV.BindingContext = new BindingContext();

            Database.SelectQueryToDataGridView(sqlConnectionString, query, testingDGV);

            Assert.AreEqual(2, testingDGV.Rows.Count);
        }
        [TestMethod]
        public void FindDataFromTovarToComboBox()
        {
            string query = Tovar.Queries.FindTovar + " [Код товара] = 0";
            ComboBox testingCB = new ComboBox();

            testingCB.BindingContext = new BindingContext();

            Database.SelectQueryToComboBox(sqlConnectionString, query, testingCB, "Наименование", "Код товара");

            Assert.AreEqual("Принтер M-9", testingCB.Text.ToString());
        }
        [TestMethod]
        public void ConvertDateTime()
        {
            DateTime time = new DateTime(2023, 10, 1);
            Assert.AreEqual("2023-10-1", Database.OpDate(time));
        }
        [TestMethod]
        public void ConvertDateTimeTicks()
        {
            DateTime time = new DateTime(2023);
            Assert.AreEqual("1-1-1", Database.OpDate(time));
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SelectAllDataFromFirmaToDataGridViewNULL()
        {
            string query = "";
            DataGridView testingDGV = new DataGridView();

            testingDGV.BindingContext = new BindingContext();

            Database.SelectQueryToDataGridView(sqlConnectionString, query, testingDGV);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindDataFromTovarToDataGridViewNULL()
        {
            string query = "";
            DataGridView testingDGV = new DataGridView();

            testingDGV.BindingContext = new BindingContext();

            Database.SelectQueryToDataGridView(sqlConnectionString, query, testingDGV);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FindDataFromTovarToComboBoxNULL()
        {
            string query = "";
            ComboBox testingCB = new ComboBox();

            testingCB.BindingContext = new BindingContext();

            Database.SelectQueryToComboBox(sqlConnectionString, query, testingCB, "Наименование", "Код товара");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertNULL()
        {
            string query = "";
            Database.InsertQuery(sqlConnectionString, query);
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InsertNullQueryWithParametrs()
        {
            string query = "";
            Database.InsertQuery(sqlConnectionString, query, new Parametr[1] {new Parametr("Name", "One")});
        }
        [TestMethod]
        public void CryptMethodForPassword()
        {
            string pass = "";
            Assert.AreEqual("D41D8CD98F00B204E9800998ECF8427E", Database.Crypt(pass));
        }
        [TestMethod]
        public void Test()
        {
            Assert.AreEqual(new byte[0] { }, Database.test());
        }
    }
}
