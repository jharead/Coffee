using System;
using Xunit;
using Coffee.Models;
using Coffee.Controllers;
using Microsoft.EntityFrameworkCore;
using Moq;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using FluentAssertions;

namespace CoffeeTest
{
    public class UnitTest1
    {
        [Fact]
        public void If_coffee_exist()
        {
            var options = new DbContextOptionsBuilder<CoffeeContext>()             .UseInMemoryDatabase("Find_coffee_item")             .Options;              using (var context = new CoffeeContext(options))              {                 context.CoffeeItems.Add(new CoffeeItem { Id = 1, Name = "coffee1", Flavor = "mocha", Size = "small" });              }              using (var context = new CoffeeContext(options))             {                 var cof = new CoffeeController(context);                 var result = cof.GetCoffeeType("mocha", "small");

                Assert.NotNull(result);             }
        }

        [Fact]
        public void Negative_Test_If_coffee_exist()
        {
            var options = new DbContextOptionsBuilder<CoffeeContext>()
            .UseInMemoryDatabase("Find_coffee_item")
            .Options;

            using (var context = new CoffeeContext(options))
            {
                context.CoffeeItems.Add(new CoffeeItem { Name = "coffee1", Flavor = "mocha", Size = "small" });
            }

            using (var context = new CoffeeContext(options))
            {
                var cof = new CoffeeController(context);
                var result = cof.GetCoffeeType("latte","grande");

                Assert.Null(result);
            }
        }

        [Fact]
        public void Testing3()
        {
            var data = new List<CoffeeItem> 
            {
                new CoffeeItem { Id = 1, Name = "coffee1", Flavor = "mocha", Size = "small" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<CoffeeItem>>();
            //mockSet.As<IQueryable<CoffeeItem>>().Setup(m => m.Provider).Returns(data.Provider);
            //mockSet.As<IQueryable<CoffeeItem>>().Setup(m => m.Expression).Returns(data.Expression);
            //mockSet.As<IQueryable<CoffeeItem>>().Setup(m => m.ElementType).Returns(data.ElementType);
            //mockSet.As<IQueryable<CoffeeItem>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

            var mockContext = new Mock<CoffeeContext>();
            mockContext.Setup(c => c.CoffeeItems).Returns(mockSet.Object);

            var service = new CoffeeController(mockContext.Object);
            var result = service.GetCoffeeType("mocha", "small");

            //Assert.NotNull(blogs);
            result.Result.Should().BeOfType<OkObjectResult>()
                .Which.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }
    }
}
