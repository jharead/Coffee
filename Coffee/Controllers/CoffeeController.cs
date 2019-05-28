using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coffee.Models;
using Microsoft.AspNetCore.Authorization;
using LinqKit;

namespace Coffee.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CoffeeController : ControllerBase
    {
        private readonly CoffeeContext _context;
        public CoffeeController(CoffeeContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult<IEnumerable<CoffeeItem>> GetCoffeeType([FromQuery] string flavor, [FromQuery] string size)
        {
            if (flavor == null & size == null)
                return _context.CoffeeItems.ToList();
            else if (size == null & flavor != null)
            {
                var search = from c in _context.CoffeeItems where c.Flavor == flavor select c;
                return search.ToList();
            }
            else if (flavor == null & size != null)
            {
                var search = from c in _context.CoffeeItems where c.Size == size select c;
                return search.ToList();
            }        
            else
            {
                //var search = from c in _context.CoffeeItems where c.Flavor == flavor & c.Size == size select c;
                IQueryable<CoffeeItem> query = _context.CoffeeItems;
                var predicate = PredicateBuilder.New<CoffeeItem>(true);

                var flavorToken = flavor.Split(",");
                foreach (var token in flavorToken)
                {
                    predicate = predicate.And(p => p.Flavor == token);
                }

                return query.Where(predicate).ToList();
            }
        }

        [HttpPost]
        public ActionResult<CoffeeItem> PostCoffeeItem(CoffeeItem item)
        {
            _context.CoffeeItems.Add(item);
            _context.SaveChanges();
            return Ok();
        }

        [HttpPatch]
        public ActionResult<CoffeeItem> PatchCoffeeItem(CoffeeItem item)
        {
            _context.CoffeeItems.Update(item);
            _context.SaveChanges();
            return Ok();
        }

        [HttpDelete]
        public ActionResult<CoffeeItem> RemoveCoffeeItem([FromQuery] int id)
        {
            var coffeeItem = _context.CoffeeItems.First(c => c.Id == id);
            _context.CoffeeItems.Remove(coffeeItem);
            _context.SaveChanges();
            return Ok();
        }

    }
}
