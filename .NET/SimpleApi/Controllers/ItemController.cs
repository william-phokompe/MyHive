using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SimpleApi.Models;
using System.Linq;

namespace SimpleApi.Controllers {
    [Route("api/[controller]")]
    public class ItemController : Controller {
        private readonly ItemContext _context;

        public ItemController(ItemContext context) {
            _context = context;

            if (_context.Items.Count() == 0) {
                _context.Items.Add(new Item { Name = "Default" });
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Item item) {
            if (item == null) return BadRequest();

            _context.Items.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetItem", new { id = item.Id }, item);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] Item item) {
            if (item == null || item.Id != id) return BadRequest();

            var updatedItem = _context.Items.FirstOrDefault(t => t.Id == id);
            if (updatedItem == null) return NotFound();

            updatedItem.IsComplete = item.IsComplete;
            updatedItem.Name = item.Name;

            _context.Items.Update(updatedItem);
            _context.SaveChanges();

            return new OkResult();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(long id) {
            var itemToDelete = _context.Items.FirstOrDefault();

            if (itemToDelete == null) return NotFound();

            _context.Items.Remove(itemToDelete);
            _context.SaveChanges();

            return new OkResult();
        }
    }
}