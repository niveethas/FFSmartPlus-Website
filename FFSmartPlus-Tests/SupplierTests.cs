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
            // adding a valid new supplier
            string address = "123 Lake Street";
            string email = "test@test.com";
            string name = "Test Supplier";

            await _suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierAdditionSuccess);
        }


        [TestMethod]
        public async Task TA2_AddNewSupplier_Invalid_Empty()
        {
            // adding invalid new supplier - empty strings
            string address = "";
            string email = "";
            string name = "";

            await _suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierAdditionSuccess);
        }

        [TestMethod]
        public async Task TA3_AddNewSupplier_InvalidEmail()
        {
            // adding invalid new supplier - invalid email format - missing @ and .
            string address = "123 Lake Street";
            string email = "test";
            string name = "Test Supplier";

            await _suppliers.AddNewSupplier(address, email, name);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierAdditionSuccess);
        }

        #endregion

        #region getSupplier(s)
        //getSuppliersToList() and GetSupplierById
        [TestMethod]
        public async Task TB1_GetSuppliersToList()
        {
            // get a list of all suppliers
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            List<SupplierDto> suppliersList = _suppliers.getSuppliersToList();

            Assert.IsTrue(suppliersList.Count() > 0);

            // save the supplier added in first tc
            newSupplier = suppliersList.Last();
        }

        [TestMethod]
        public async Task TB2_GetSupplierById()
        {
            // getting supplier by id - valid id from previous tc
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
            // on change supplier (occurs when new supplier is selected and loads new supplier into selected supplier)
            await _suppliers.OnChangeSupplier(newSupplier.Id.ToString());

            Assert.AreEqual(newSupplier.Id, _suppliers.selectedSupplierId);
        }

        [TestMethod]
        public async Task TC2_OnChangeSupplier_InvalidSupplier_NonExistent()
        {
            // invalid supplier id - supplier does not exist
            string invalidSupplierId = "500";

            await _suppliers.OnChangeSupplier(invalidSupplierId);

            Assert.AreNotEqual(invalidSupplierId, _suppliers.selectedSupplierId.ToString());
        }

        [TestMethod]
        public async Task TC3_OnChangeSupplier_InvalidSupplier_StringChar()
        {
            // invalid supplier id - supplier id is not numerical value
            string invalidSupplierId = "abc";

            await _suppliers.OnChangeSupplier(invalidSupplierId);

            Assert.AreNotEqual(invalidSupplierId, _suppliers.selectedSupplierId.ToString());
        }

        #endregion

        #region changeSupplierDetails
        //changeSupplierDetails(long id, string? address, string? email, string? name)

        [TestMethod]
        public async Task TD1_ChangeSupplierDetails_Valid_AllParams()
        {
            // updating supplier from previous tc - updating all values
            // ensuring all suppliers are loaded correctly
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await _suppliers.changeSupplierDetails(newSupplier.Id,newAddress,newEmail,newName);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            // reloading suppliers to ensure it was updated correctly
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
        }

        [TestMethod]
        public async Task TD2_ChangeSupplierDetails_Valid_NullAddress()
        {
            // updating a supplier from preious tc - valid - not updating address
            // reloading suppliers
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            newSupplier = _suppliers.allSuppliers.Last();

            _suppliers.selectedSupplier = newSupplier;

            string newEmail = "test3@test.com";
            string newName = "Supplier Test 3";
            await _suppliers.changeSupplierDetails(id:newSupplier.Id, address:null, email:newEmail, name:newName);

            Assert.AreEqual(TestConsts.TRUE_STR, _suppliers.supplierModifySuccess);

            // checking supplier was updated
            _suppliers.allSuppliers = await _suppliers._client.SupplierAllAsync();
            Assert.AreNotEqual(newSupplier, _suppliers.allSuppliers.Last());
            Assert.AreEqual(newSupplier.Address, _suppliers.allSuppliers.Last().Address);

        }

        [TestMethod]
        public async Task TD3_ChangeSupplierDetails_Valid_NullEmail()
        {
            // updating a supplier from preious tc - valid - not updating email
            // reloading suppliers
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
            // updating a supplier from preious tc - valid - not updating name
            // reloading suppliers
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
            // updating a supplier from preious tc - valid - not updating any
            // reloading suppliers
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
            // updating a supplier - invalid - supplier id does not exist
            _suppliers.selectedSupplier = newSupplier;

            long invalidId = 500;

            string newAddress = "123 Angel Row";
            string newEmail = "test2@test.com";
            string newName = "Supplier Test 2";
            await _suppliers.changeSupplierDetails(invalidId, newAddress, newEmail, newName);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierModifySuccess);
        }

        [TestMethod]
        public async Task TD7_ChangeSupplierDetails_Invalid_Email()
        {
            // updating a supplier - invalid - invalid email format - no @ or .
            _suppliers.selectedSupplier = newSupplier;

            string newAddress = "123 Angel Row";
            string newEmail = "test";
            string newName = "Supplier Test 2";
            await _suppliers.changeSupplierDetails(newSupplier.Id, newAddress, newEmail, newName);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierModifySuccess);
        }
        #endregion

        #region deleteSupplier
        //deleteSupplier(long id)

        [TestMethod]
        public async Task TE1_DeleteSupplier_Valid()
        {
            // delete existing supplier
            // reloading suppliers - should delete one added from previous tc
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
            // delete supplier - invalid - supplier id does not exist
            long invalidId = 500;

            await _suppliers.deleteSupplier(invalidId);

            Assert.AreEqual(TestConsts.FALSE_STR, _suppliers.supplierDeleteSuccess);

            _suppliers.supplierDeleteSuccess = "";
        }

        #endregion
    }
}
