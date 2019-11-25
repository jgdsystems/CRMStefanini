using Microsoft.VisualStudio.TestTools.UnitTesting;
using CRM.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRM.Models;

namespace CRM.Controllers.Tests
{
    [TestClass()]
    public class CustomerControllerTests
    {
        [TestMethod()]
        public void EditTest()
        {
            // Arrange
            Customers customerMock = new Customers{
                City = "Porto Alegre",
                Classification = "V",
                Id_customer = 1,
                LastPurchase = DateTime.Now,
                Name = "Pedro Silva",
                Phone = "(51) 98177-6202",
                Region = "Zona Sul",
                Sex = "M",
                Until = DateTime.Now.AddDays(1),
                User = null,
                Users_id_users = 3
            };


            // Act

            DateTime LastPurchaseBegin = customerMock.LastPurchase;

            customerMock.City = "Santa Catarina";
            customerMock.Classification = "V";
            customerMock.Id_customer = 1;
            customerMock.LastPurchase = DateTime.Now;
            customerMock.Name = "Pedro Silva";
            customerMock.Phone = "(51) 98177-6202";
            customerMock.Region = "Zona Sul"; 
            customerMock.Sex = "M";
            customerMock.Until = DateTime.Now.AddDays(1);
            customerMock.User = null;
            customerMock.Users_id_users = 3;

            // Assert
            Assert.IsNotNull(customerMock.Id_customer, "O id do cliente não foi localizado");
            Assert.IsNotNull(customerMock.Name, "O nome do cliente não foi localizado");
            Assert.AreNotEqual(customerMock.LastPurchase, LastPurchaseBegin);

        }
              

        [TestMethod()]
        public void CreateTest()
        {
            // Arrange
            Customers customerMock = new Customers();
            // Act
            customerMock.Id_customer = 1;
            // Assert
            Assert.IsNotNull(customerMock.Id_customer, "O id do cliente não foi localizado");
            Assert.IsNull(customerMock.Name, "O nome do cliente não está vazio");

        }

       

    }   
}