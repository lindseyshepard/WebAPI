using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/Todo")] //this is the route that is used in postman
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)  //this is the Constructer  and this does dependency injection
        {
            _context = context;

            if (!_context.TodoItems.Any()) //if the count is 0 go add something to it
            {
                // Create a new TodoItem if collection is empty,
                // which means you can't delete all TodoItems.
                _context.TodoItems.Add(new TodoItem { Name = "Item1" }); //this will display the javascript object notation
                _context.TodoItems.Add(new TodoItem { Name = "Item2" });
                _context.TodoItems.Add(new TodoItem { Name = "Item3" });
                _context.TodoItems.Add(new TodoItem { Name = "Item4" });
                _context.TodoItems.Add(new TodoItem { Name = "Item5" });
                _context.TodoItems.Add(new TodoItem { Name = "Item6" });
                _context.TodoItems.Add(new TodoItem { Name = "Item7" });
                _context.SaveChanges();
            }
        }
        // GET: api/Todo
        [HttpGet] //Attribute HTTPGET so its a get method
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems() //returning task object that is generic. Generic in a generic.
                                                                              //          IEnumerable is a list of <todoitems> thats part of the action result
        {  //async is happening in the background.
            //Task library is the threading namespace, but its different then threading
            //Task represents async operation that will RETURN a value!

            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/Todo/5        HttpGet the id is the place holder
        [HttpGet("{id}")] //Get method by the ID: 0 to 1 resulta bACK. Most to return back is 1 and the least is 0
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id); //findasync helps. cannot use find and pass just anything in

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }

        [HttpGet("byName/{name}")] //Get method by the ID: 0 to 1 resulta bACK. Most to return back is 1 and the least is 0
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItemByName(string name)
        {
            var todoItem = await _context.TodoItems  //hits DB
                .Where(t => t.Name.Equals(name, System.StringComparison.InvariantCultureIgnoreCase))
                //where a name is equal to a name(however we dont care the language its written in like french or upper/lower case)
                .ToListAsync<TodoItem>();
            //convert it to a list of todoitems async




            if (todoItem == null) // if nothing
            {
                return NotFound();//tell me found nothing
            }

            return todoItem;//otherwise kick back the list
        }



        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            _context.TodoItems.Add(item); //adding to todo items
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }


        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        // DELETE: api/Todo/5
        [HttpDelete("byName /{name}")]
        public async Task<IActionResult> DeleteTodoByName(string name)
        {
            var todoItem = await _context.TodoItems.FindAsync(name);

            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }

}