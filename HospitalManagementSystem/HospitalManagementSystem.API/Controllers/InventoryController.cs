using HospitalManagementSystem.Data;
using HospitalManagementSystem.Data.Entities;
using HospitalManagementSystem.Data.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace HospitalManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;
        private readonly ApplicationDbContext _context;

        public InventoryController(IInventoryService inventoryService, ApplicationDbContext context)
        {
            _inventoryService = inventoryService;
            _context = context;
        }

        /// <summary>
        /// Get all inventory items
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetInventory()
        {
            try
            {
                var items = await _context.Inventory
                    .Select(i => new
                    {
                        i.ItemId,
                        i.ItemName,
                        i.CurrentStock,
                        i.MinimumStock,
                        i.ExpiryDate,
                        i.UnitPrice,
                        Status = i.CurrentStock <= i.MinimumStock ? "Low Stock" : "In Stock"
                    })
                    .ToListAsync();
                return Ok(items);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get inventory item by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetInventoryItem(int id)
        {
            try
            {
                var item = await _context.Inventory
                    .Where(i => i.ItemId == id)
                    .Select(i => new
                    {
                        i.ItemId,
                        i.ItemName,
                        i.CurrentStock,
                        i.MinimumStock,
                        i.ExpiryDate,
                        i.UnitPrice,
                        Status = i.CurrentStock <= i.MinimumStock ? "Low Stock" : "In Stock"
                    })
                    .FirstOrDefaultAsync();

                if (item == null)
                    return NotFound(new { message = "Inventory item not found" });

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Add new medicine to inventory
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<object>> AddInventory([FromBody] CreateInventoryDto dto)
        {
            try
            {
                if (dto.CurrentStock <= 0 || dto.UnitPrice <= 0)
                    return BadRequest(new { message = "CurrentStock and UnitPrice must be greater than 0" });

                var inventory = new Inventory
                {
                    ItemName = dto.ItemName,
                    ItemCode = dto.ItemCode,
                    Category = dto.Category,
                    CurrentStock = dto.CurrentStock,
                    MinimumStock = dto.MinimumStock,
                    ExpiryDate = dto.ExpiryDate,
                    UnitPrice = dto.UnitPrice
                };

                _context.Inventory.Add(inventory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventoryItem), new { id = inventory.ItemId }, new
                {
                    inventory.ItemId,
                    inventory.ItemName,
                    inventory.CurrentStock,
                    inventory.MinimumStock,
                    inventory.ExpiryDate,
                    inventory.UnitPrice
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Update inventory stock
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInventory(int id, [FromBody] UpdateInventoryDto dto)
        {
            try
            {
                var inventory = await _context.Inventory.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                if (dto.CurrentStock.HasValue && dto.CurrentStock >= 0)
                    inventory.CurrentStock = dto.CurrentStock.Value;
                if (dto.MinimumStock.HasValue && dto.MinimumStock >= 0)
                    inventory.MinimumStock = dto.MinimumStock.Value;
                if (dto.ExpiryDate.HasValue)
                    inventory.ExpiryDate = dto.ExpiryDate.Value;
                if (dto.UnitPrice.HasValue && dto.UnitPrice > 0)
                    inventory.UnitPrice = dto.UnitPrice.Value;

                _context.Inventory.Update(inventory);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Inventory updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Delete inventory item
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                var inventory = await _context.Inventory.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                _context.Inventory.Remove(inventory);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Inventory item deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get low stock items (below reorder level)
        /// </summary>
        [HttpGet("low-stock/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetLowStockItems()
        {
            try
            {
                var lowStockItems = await _context.Inventory
                    .Where(i => i.CurrentStock <= i.MinimumStock)
                    .Select(i => new
                    {
                        i.ItemId,
                        i.ItemName,
                        i.CurrentStock,
                        i.MinimumStock,
                        Shortage = i.MinimumStock - i.CurrentStock,
                        i.UnitPrice,
                        EstimatedCost = (i.MinimumStock - i.CurrentStock) * i.UnitPrice
                    })
                    .ToListAsync();

                return Ok(lowStockItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Reorder medicine (increase stock)
        /// </summary>
        [HttpPost("{id}/reorder")]
        public async Task<IActionResult> ReorderMedicine(int id, [FromBody] ReorderDto dto)
        {
            try
            {
                if (dto.Quantity <= 0)
                    return BadRequest(new { message = "Reorder quantity must be greater than 0" });

                var inventory = await _context.Inventory.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                inventory.CurrentStock += dto.Quantity;
                _context.Inventory.Update(inventory);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Medicine reordered successfully",
                    inventory.ItemId,
                    inventory.CurrentStock,
                    ReorderedAmount = dto.Quantity
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get expired medicines
        /// </summary>
        [HttpGet("expired/list")]
        public async Task<ActionResult<IEnumerable<object>>> GetExpiredMedicines()
        {
            try
            {
                var expiredItems = await _context.Inventory
                    .Where(i => i.ExpiryDate < DateTime.Now)
                    .Select(i => new
                    {
                        i.ItemId,
                        i.ItemName,
                        i.CurrentStock,
                        i.ExpiryDate,
                        DaysExpired = i.ExpiryDate.HasValue ? (DateTime.Now - i.ExpiryDate.Value).Days : 0
                    })
                    .ToListAsync();

                return Ok(expiredItems);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    // DTOs
    public class CreateInventoryDto
    {
        public string ItemName { get; set; }
        public string ItemCode { get; set; }
        public string Category { get; set; }
        public int CurrentStock { get; set; }
        public int MinimumStock { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class UpdateInventoryDto
    {
        public int? CurrentStock { get; set; }
        public int? MinimumStock { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class ReorderDto
    {
        public int Quantity { get; set; }
    }
}
