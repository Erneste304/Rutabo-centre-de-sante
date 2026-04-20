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
                var items = await _context.Inventories
                    .Include(i => i.Medicine)
                    .Select(i => new
                    {
                        i.InventoryId,
                        i.Medicine.MedicineName,
                        i.Quantity,
                        i.ReorderLevel,
                        i.ExpiryDate,
                        i.UnitPrice,
                        Status = i.Quantity <= i.ReorderLevel ? "Low Stock" : "In Stock"
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
                var item = await _context.Inventories
                    .Include(i => i.Medicine)
                    .Where(i => i.InventoryId == id)
                    .Select(i => new
                    {
                        i.InventoryId,
                        i.MedicineId,
                        i.Medicine.MedicineName,
                        i.Quantity,
                        i.ReorderLevel,
                        i.ExpiryDate,
                        i.UnitPrice,
                        Status = i.Quantity <= i.ReorderLevel ? "Low Stock" : "In Stock"
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
                if (dto.Quantity <= 0 || dto.UnitPrice <= 0)
                    return BadRequest(new { message = "Quantity and UnitPrice must be greater than 0" });

                var inventory = new Inventory
                {
                    MedicineId = dto.MedicineId,
                    Quantity = dto.Quantity,
                    ReorderLevel = dto.ReorderLevel,
                    ExpiryDate = dto.ExpiryDate,
                    UnitPrice = dto.UnitPrice
                };

                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventoryItem), new { id = inventory.InventoryId }, new
                {
                    inventory.InventoryId,
                    inventory.MedicineId,
                    inventory.Quantity,
                    inventory.ReorderLevel,
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
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                if (dto.Quantity.HasValue && dto.Quantity >= 0)
                    inventory.Quantity = dto.Quantity.Value;
                if (dto.ReorderLevel.HasValue && dto.ReorderLevel >= 0)
                    inventory.ReorderLevel = dto.ReorderLevel.Value;
                if (dto.ExpiryDate.HasValue)
                    inventory.ExpiryDate = dto.ExpiryDate.Value;
                if (dto.UnitPrice.HasValue && dto.UnitPrice > 0)
                    inventory.UnitPrice = dto.UnitPrice.Value;

                _context.Inventories.Update(inventory);
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
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                _context.Inventories.Remove(inventory);
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
                var lowStockItems = await _context.Inventories
                    .Where(i => i.Quantity <= i.ReorderLevel)
                    .Include(i => i.Medicine)
                    .Select(i => new
                    {
                        i.InventoryId,
                        i.Medicine.MedicineName,
                        i.Quantity,
                        i.ReorderLevel,
                        Shortage = i.ReorderLevel - i.Quantity,
                        i.UnitPrice,
                        EstimatedCost = (i.ReorderLevel - i.Quantity) * i.UnitPrice
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

                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                    return NotFound(new { message = "Inventory item not found" });

                inventory.Quantity += dto.Quantity;
                _context.Inventories.Update(inventory);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Medicine reordered successfully",
                    inventory.InventoryId,
                    inventory.Quantity,
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
                var expiredItems = await _context.Inventories
                    .Where(i => i.ExpiryDate < DateTime.Now)
                    .Include(i => i.Medicine)
                    .Select(i => new
                    {
                        i.InventoryId,
                        i.Medicine.MedicineName,
                        i.Quantity,
                        i.ExpiryDate,
                        DaysExpired = (DateTime.Now - i.ExpiryDate).Days
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
        public int MedicineId { get; set; }
        public int Quantity { get; set; }
        public int ReorderLevel { get; set; }
        public DateTime ExpiryDate { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class UpdateInventoryDto
    {
        public int? Quantity { get; set; }
        public int? ReorderLevel { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? UnitPrice { get; set; }
    }

    public class ReorderDto
    {
        public int Quantity { get; set; }
    }
}
