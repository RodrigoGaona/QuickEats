using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

// Las referencias al modelo y el contexto son correctas
using FastFoodAPI.Data;
using FastFoodAPI.Models;
// Agregamos la referencia al namespace de la raíz (FastFoodAPI)
using FastFoodAPI;

namespace FastFoodAPI.Tests
{
	public class OrdersControllerTests
	{
		// ----------------------------------------------------
		// MÉTODO DE CONFIGURACIÓN (SETUP)
		// ----------------------------------------------------
		private AppDbContext GetInMemoryDbContext()
		{
			var options = new DbContextOptionsBuilder<AppDbContext>()
				.UseInMemoryDatabase(databaseName: "TestDb_" + Guid.NewGuid().ToString())
				.Options;

			return new AppDbContext(options);
		}

		// ----------------------------------------------------
		// PRUEBAS DE VISUALIZACIÓN (GET)
		// ----------------------------------------------------

		[Fact]
		public async Task GetOrders_ShouldReturnEmptyList_IfDatabaseIsEmpty()
		{
			// Arrange
			using var context = GetInMemoryDbContext();
			// INICIALIZACIÓN: Apunta directamente al namespace del controlador
			var controller = new FastFoodAPI.Controllers.OrdersController(context);

			// Act
			var result = await controller.GetOrders();

			// Assert
			var orders = Assert.IsAssignableFrom<IEnumerable<Order>>(result.Value);
			Assert.Empty(orders);
		}

		// ----------------------------------------------------
		// PRUEBAS DE AGREGAR (POST)
		// ----------------------------------------------------

		[Fact]
		public async Task AddOrder_ShouldReturnCreatedResult_WhenOrderIsValid()
		{
			// Arrange
			using var context = GetInMemoryDbContext();
			var controller = new FastFoodAPI.Controllers.OrdersController(context);
			var newOrder = new Order { OrderId = "POST_TEST_101" };

			// Act
			var result = await controller.AddOrder(newOrder);

			// Assert

			// 1. Verifica que el tipo de retorno sea ActionResult (o el tipo que envuelve el 201)
			//    Y que contenga un objeto de tipo Order
			var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
			var returnedOrder = Assert.IsType<Order>(createdAtActionResult.Value);

			// 2. Verifica que el objeto se haya insertado en la DB en memoria
			var savedOrder = await context.Orders.FirstOrDefaultAsync(o => o.OrderId == "POST_TEST_101");
			Assert.NotNull(savedOrder);
			Assert.Equal(newOrder.OrderId, returnedOrder.OrderId);
		}

		// ----------------------------------------------------
		// PRUEBAS DE ELIMINAR (DELETE)
		// ----------------------------------------------------

		[Fact]
		public async Task DeleteOrder_ShouldReturnNoContent_WhenOrderExists()
		{
			// Arrange
			using var context = GetInMemoryDbContext();
			var orderToDelete = new Order { OrderId = "DELETE_TEST_1", CreationDate = DateTime.Now };
			context.Orders.Add(orderToDelete);
			await context.SaveChangesAsync();
			var controller = new FastFoodAPI.Controllers.OrdersController(context);

			// Act: Ejecuta el método DELETE
			var result = await controller.DeleteOrder(orderToDelete.Id);

			// Assert
			Assert.IsType<NoContentResult>(result);
			var deletedOrder = await context.Orders.FindAsync(orderToDelete.Id);
			Assert.Null(deletedOrder);
		}

		[Fact]
		public async Task DeleteOrder_ShouldReturnNotFound_WhenOrderDoesNotExist()
		{
			// Arrange
			using var context = GetInMemoryDbContext();
			var controller = new FastFoodAPI.Controllers.OrdersController(context);

			// Act: Intenta eliminar un ID que no existe (ej. 999)
			var result = await controller.DeleteOrder(999);

			// Assert
			Assert.IsType<NotFoundResult>(result);
		}
	}
}