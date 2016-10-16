using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using todoList.Models;

namespace todoList.Controllers
{
    [Authorize]
    public class TarefaController : ApiController
    {
        private Contexto db = new Contexto();

        // GET: api/Tarefa
        public IQueryable<Tarefas> GetTarefas()
        {
            return db.Tarefas.Where(w => w.Username == User.Identity.Name);
        }

        // GET: api/Tarefa/5
        [ResponseType(typeof(Tarefas))]
        public async Task<IHttpActionResult> GetTarefas(int id)
        {
            Tarefas tarefas = await db.Tarefas.FindAsync(id);
            if (tarefas == null)
            {
                return NotFound();
            }

            if(tarefas.Username != User.Identity.Name)
            {
                return BadRequest("Você não tem permissão para consultar a tarefa de outro usuário.");
            }
            return Ok(tarefas);
            
        }

        // PUT: api/Tarefa/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutTarefas(int id, Tarefas tarefas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tarefas.Id)
            {
                return BadRequest();
            }

            var novaTarefa = await db.Tarefas.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            var mesmoUsuario = novaTarefa.Username == User.Identity.Name &&
                novaTarefa.Username == tarefas.Username;

            if (!mesmoUsuario)
            {
                return BadRequest("Você não tem permissão para alterar a tarefa de outro usuário.");
            }


            db.Entry(tarefas).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TarefasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Tarefa
        [ResponseType(typeof(Tarefas))]
        public async Task<IHttpActionResult> PostTarefas(Tarefas tarefas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tarefas.Username = User.Identity.Name;

            db.Tarefas.Add(tarefas);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = tarefas.Id }, tarefas);
        }

        // DELETE: api/Tarefa/5
        [ResponseType(typeof(Tarefas))]
        public async Task<IHttpActionResult> DeleteTarefas(int id)
        {
            Tarefas tarefas = await db.Tarefas.FindAsync(id);
            if (tarefas == null)
            {
                return NotFound();
            }

            var novaTarefa = await db.Tarefas.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);

            var mesmoUsuario = novaTarefa.Username == User.Identity.Name &&
                novaTarefa.Username == tarefas.Username;

            if (!mesmoUsuario)
            {
                return BadRequest("Você não tem permissão para excluir a tarefa de outro usuário.");
            }

            db.Tarefas.Remove(tarefas);
            await db.SaveChangesAsync();

            return Ok(tarefas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TarefasExists(int id)
        {
            return db.Tarefas.Count(e => e.Id == id) > 0;
        }
    }
}