using ClientAPI;
using FFSmartPlus_Website;
using FFSmartPlus_Website.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FFSmartPlus_Tests
{
    [TestClass]
    public class SupplierTests
    {
        Suppliers suppliers;

        public SupplierDto newSupplier = new SupplierDto() { Address = "123 Lake Street",
        Email = "test@test.com", Name = "Test Supplier", Id = 10009 };

        public SupplierTests()
        {
            suppliers = new Suppliers();
            suppliers._client = new FFSBackEnd("https://localhost:7041/", new HttpClient());

            var newLogin = new LoginModel();
            newLogin.Username = "admin1";
            newLogin.Password = "@Admin123"; // Password requires a symbol, capital letter and 3 numbers
            suppliers._client.LoginAsync(newLogin);
            var loginCode = suppliers._client.LoginAsync(newLogin);
            suppliers._client.Authorisation(loginCode.Result.Token);
        }

        #region AddNewSupplier
        // AddNewSupplier (string address, string email, string name)
        [TestMethod]
        public async Task TA1_AddNewSupplier_Valid()
        {
            string address = "123 Lake Street";
            string email = "test@test.com";
            string name = "Test Supplier";

            await suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierAdditionSuccess);


        }


        [TestMethod]
        public async Task TA2_AddNewSupplier_Invalid_Empty()
        {
            string address = "";
            string email = "";
            string name = "";

            await suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.FALSE_STR, suppliers.supplierAdditionSuccess);
        }

        #endregion

        #region getSupplier(s)
        //getSuppliersToList() and GetSupplierById
        [TestMethod]
        public async Task TB1_GetSuppliersToList()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            List<SupplierDto> suppliersList = suppliers.getSuppliersToList();

            Assert.IsTrue(suppliersList.Count() > 0);

            newSupplier = suppliersList.Last();
        }

        [TestMethod]
        public async Task TB2_GetSupplierById()
        {
            suppliers.selectedSupplierId = newSupplier.Id;

            await suppliers.getSupplierById();

            Assert.AreEqual(newSupplier.Id, suppliers.selectedSupplier.Id);
            Assert.AreEqual(newSupplier.Address, suppliers.selectedSupplier.Address);
        }

        #endregion

        #region onChangeSupplier
        //OnChangeSupplier(string supplier)
        [TestMethod]
        public async Task TC1_OnChangeSupplier_ValidSupplier()
        {
            String validSupplierId = "1";

            await suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreEqual(validSupplierId, suppliers.selectedSupplierId.ToString());
        }

        [TestMethod]
        public async Task TC2_OnChangeSupplier_InvalidSupplier_NonExistent()
        {
            String validSupplierId = "500";

            await suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreNotEqual(validSupplierId, suppliers.selectedSupplierId.ToString());
        }

        [TestMethod]
        public async Task TC3_OnChangeSupplier_InvalidSupplier_StringChar()
        {
            String validSupplierId = "abc";

            await suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreNotEqual(validSupplierId, suppliers.selectedSupplierId.ToString());
        }

        #endregion

        #region changeSupplierDetails
        //changeSupplierDetails(long id, string? address, string? email, string? name)

        [TestMethod]
        public async Task TD1_ChangeSupplierDetails_Valid_AllParams()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await suppliers.changeSupplierDetails(newSupplier.Id,newAddress,newEmail,newName);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierModifySuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD2_ChangeSupplierDetails_Valid_NullAddress()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            suppliers.selectedSupplier = newSupplier;

            string newEmail = "test3@test.com";
            string newName = "Supplier Test 3";
            await suppliers.changeSupplierDetails(id:newSupplier.Id, address:null, email:newEmail, name:newName);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierModifySuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, suppliers.allSuppliers.Last());
            Assert.AreEqual(newSupplier.Address, suppliers.allSuppliers.Last().Address);

        }

        [TestMethod]
        public async Task TD3_ChangeSupplierDetails_Valid_NullEmail()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            suppliers.selectedSupplier = newSupplier;

            string newAddress = "123 Street Street";
            string newName = "Test Supplier";
            await suppliers.changeSupplierDetails(id: newSupplier.Id, address: newAddress, email: null, name: newName);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierModifySuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, suppliers.allSuppliers.Last());
        }


        [TestMethod]
        public async Task TD4_ChangeSupplierDetails_Valid_NullName()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            suppliers.selectedSupplier = newSupplier;

            string newAddress = "123 Lake Street";
            string newEmail = "test@test.com";
            await suppliers.changeSupplierDetails(id: newSupplier.Id, address: newAddress, email: newEmail, name: null);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierModifySuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD5_ChangeSupplierDetails_Valid_AllNull()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            suppliers.selectedSupplier = newSupplier;

            await suppliers.changeSupplierDetails(id: newSupplier.Id, address: null, email: null, name: null);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierModifySuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD6_ChangeSupplierDetails_Invalid_Id()
        {
            suppliers.selectedSupplier = newSupplier;

            long invalidId = 500;

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await suppliers.changeSupplierDetails(invalidId, newAddress, newEmail, newName);

            Assert.AreEqual(TestConsts.FALSE_STR, suppliers.supplierModifySuccess);
        }
        #endregion

        #region deleteSupplier
        //deleteSupplier(long id)

        [TestMethod]
        public async Task TE1_DeleteSupplier_Valid()
        {
            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            newSupplier = suppliers.allSuppliers.Last();

            await suppliers.deleteSupplier(newSupplier.Id);

            Assert.AreEqual(TestConsts.TRUE_STR, suppliers.supplierDeleteSuccess);

            suppliers.allSuppliers = await suppliers._client.SupplierAllAsync();
            CollectionAssert.DoesNotContain(suppliers.allSuppliers.ToList(), newSupplier);

            suppliers.supplierDeleteSuccess = "";
        }

        [TestMethod]
        public async Task TE1_DeleteSupplier_Invalid_NonExistent()
        {
            long invalidId = 500;

            await suppliers.deleteSupplier(invalidId);

            Assert.AreEqual(TestConsts.FALSE_STR, suppliers.supplierDeleteSuccess);

            suppliers.supplierDeleteSuccess = "";
        }

        #endregion
    }
}
