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
    }
}