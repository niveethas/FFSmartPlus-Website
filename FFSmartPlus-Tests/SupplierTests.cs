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
        Suppliers _suppliers;

        public SupplierDto newSupplier = new SupplierDto() { Address = "123 Lake Street",
        Email = "test@test.com", Name = "Test Supplier", Id = 10009 };

        public SupplierTests()
        {
            _suppliers = new Suppliers();
            _suppliers._client = new SetupClient().SignInAdmin();
        }

        #region AddNewSupplier
        // AddNewSupplier (string address, string email, string name)
        [TestMethod]
        public async Task TA1_AddNewSupplier_Valid()
        {
            string address = "123 Lake Street";
            string email = "test@test.com";
            string name = "Test Supplier";

            await _suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierAdditionSuccess);


        }


        [TestMethod]
        public async Task TA2_AddNewSupplier_Invalid_Empty()
        {
            string address = "";
            string email = "";
            string name = "";

            await _suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierAdditionSuccess);
        }

        #endregion

        #region getSupplier(s)
        //getSuppliersToList() and GetSupplierById
        [TestMethod]
        public async Task TB1_GetSuppliersToList()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            List<SupplierDto> suppliersList = _suppliers.getSuppliersToList();

            Assert.IsTrue(suppliersList.Count() > 0);

            newSupplier = suppliersList.Last();
        }

        [TestMethod]
        public async Task TB2_GetSupplierById()
        {
            _suppliers.selectedSupplierId = newSupplier.Id;

            await _suppliers.getSupplierById();

            Assert.AreEqual(newSupplier.Id, _suppliers.selectedSupplier.Id);
            Assert.AreEqual(newSupplier.Address, _suppliers.selectedSupplier.Address);
        }

        #endregion

        #region onChangeSupplier
        //OnChangeSupplier(string supplier)
        [TestMethod]
        public async Task TC1_OnChangeSupplier_ValidSupplier()
        {
            String validSupplierId = "1";

            await _suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreEqual(validSupplierId, _suppliers.selectedSupplierId.ToString());
        }

        [TestMethod]
        public async Task TC2_OnChangeSupplier_InvalidSupplier_NonExistent()
        {
            String validSupplierId = "500";

            await _suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreNotEqual(validSupplierId, _suppliers.selectedSupplierId.ToString());
        }

        [TestMethod]
        public async Task TC3_OnChangeSupplier_InvalidSupplier_StringChar()
        {
            String validSupplierId = "abc";

            await _suppliers.OnChangeSupplier(validSupplierId);

            Assert.AreNotEqual(validSupplierId, _suppliers.selectedSupplierId.ToString());
        }

        #endregion

        #region changeSupplierDetails
        //changeSupplierDetails(long id, string? address, string? email, string? name)

        [TestMethod]
        public async Task TD1_ChangeSupplierDetails_Valid_AllParams()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await _suppliers.changeSupplierDetails(newSupplier.Id,newAddress,newEmail,newName);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD2_ChangeSupplierDetails_Valid_NullAddress()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            _suppliers.selectedSupplier = newSupplier;

            string newEmail = "test3@test.com";
            string newName = "Supplier Test 3";
            await _suppliers.changeSupplierDetails(id:newSupplier.Id, address:null, email:newEmail, name:newName);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
            Assert.AreEqual(newSupplier.Address, _suppliers.allSuppliers.Last().Address);

        }

        [TestMethod]
        public async Task TD3_ChangeSupplierDetails_Valid_NullEmail()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            _suppliers.selectedSupplier = newSupplier;

            string newAddress = "123 Street Street";
            string newName = "Test Supplier";
            await _suppliers.changeSupplierDetails(id: newSupplier.Id, address: newAddress, email: null, name: newName);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
        }


        [TestMethod]
        public async Task TD4_ChangeSupplierDetails_Valid_NullName()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            _suppliers.selectedSupplier = newSupplier;

            string newAddress = "123 Lake Street";
            string newEmail = "test@test.com";
            await _suppliers.changeSupplierDetails(id: newSupplier.Id, address: newAddress, email: newEmail, name: null);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD5_ChangeSupplierDetails_Valid_AllNull()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            _suppliers.selectedSupplier = newSupplier;

            await _suppliers.changeSupplierDetails(id: newSupplier.Id, address: null, email: null, name: null);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD6_ChangeSupplierDetails_Invalid_Id()
        {
            _suppliers.selectedSupplier = newSupplier;

            long invalidId = 500;

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await _suppliers.changeSupplierDetails(invalidId, newAddress, newEmail, newName);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierModifySuccess);
        }
        #endregion

        #region deleteSupplier
        //deleteSupplier(long id)

        [TestMethod]
        public async Task TE1_DeleteSupplier_Valid()
        {
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            await _suppliers.deleteSupplier(newSupplier.Id);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierDeleteSuccess);

            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            CollectionAssert.DoesNotContain(_suppliers.allSuppliers.ToList(), newSupplier);

            _suppliers.supplierDeleteSuccess = "";
        }

        [TestMethod]
        public async Task TE1_DeleteSupplier_Invalid_NonExistent()
        {
            long invalidId = 500;

            await _suppliers.deleteSupplier(invalidId);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierDeleteSuccess);

            _suppliers.supplierDeleteSuccess = "";
        }

        #endregion
    }
}
