using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OlivaTech.Site.Data;
using OlivaTech.Site.Extension;
using OlivaTech.Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OlivaTech.Site.Controllers
{
    [Authorize]
    [Route("cursos")]
    public class CursoController : Controller
    {
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly ContextDb _context;
        public CursoController(ContextDb context,
                               UserManager<IdentityUserCustom> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Index(string searchTerm)
        {
            var user = await _userManager.GetUserAsync(User);

            Console.WriteLine(User.GetType());

            ViewData["TermSearch"] = searchTerm;
            var curso = from c in _context.Cursos select c;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                curso = curso.Where(b => b.CursoTipo.Nome.Contains(searchTerm) || b.Cidade.Contains(searchTerm));

                if (user == null)
                {
                    return View(await curso.Include(t => t.CursoTipo).Where(c => c.Disponivel == true).OrderBy(p => p.Nome).ToListAsync());
                }
                else if (await UserIsInRole("Admin") || await UserIsInRole("Ceo"))
                {
                    return View(await curso.Include(t => t.CursoTipo).OrderBy(p => p.Nome).ToListAsync());
                }

                return View(await curso.Include(t => t.CursoTipo).Where(c => c.Disponivel == true).AsNoTracking().ToListAsync());
            }
            else
            {
                if (user == null)
                {
                    return View(await _context.Cursos.Include(t => t.CursoTipo).Where(c => c.Disponivel == true).OrderBy(p => p.Nome).ToListAsync());
                }
                else if (await UserIsInRole("Admin") || await UserIsInRole("Ceo"))
                {
                    return View(await _context.Cursos.Include(t => t.CursoTipo).OrderBy(p => p.Nome).ToListAsync());
                }

                return View(await _context.Cursos.Include(t => t.CursoTipo).Where(c => c.Disponivel == true).OrderBy(p => p.Nome).ToListAsync());
            }
        }

        [Route("adicionar")]
        [ClaimsAuthorize("Cursos", "Create")]
        public IActionResult Create()
        {
            var tipoCursos = _context.CursoTipos.OrderBy(t => t.Nome).ToList();
            tipoCursos.Insert(0, new CursoTipo() { CursoTipoId = 0, Nome = "[Selecione a Formação]" });
            ViewBag.TipoCursos = tipoCursos;

            return View();
        }

        [HttpPost]
        [Route("adicionar")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Cursos", "Create")]
        public async Task<IActionResult>Create(Curso curso)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Cursos.Add(curso);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(curso);
        }

        [Route("editar-{id:long}")]
        [ClaimsAuthorize("Cursos", "Edit")]
        public async Task<IActionResult> Edit(long id)
        {
          
            var curso = await _context.Cursos.FindAsync(id);

            if(curso == null)
            {
                return NotFound();
            }

            ViewBag.CursoTipos = new SelectList(_context.CursoTipos.OrderBy(c => c.CursoTipoId), "CursoTipoId", "Nome", curso.CursoTipoId);

            return View(curso);
        }

        [HttpPost]
        [Route("editar-{id:long}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Cursos", "Edit")]
        public async Task<IActionResult> Edit(long id, Curso curso)
        {
            if(id != curso.CursoId)
            {
                return NotFound(); 
            }

            var cursoAtualizacao = await _context.Cursos.FindAsync(id);
            
            if(ModelState.IsValid)
            {
                try
                {
                    cursoAtualizacao.CursoTipoId = curso.CursoTipoId;
                    cursoAtualizacao.Nome = curso.Nome;
                    cursoAtualizacao.Cidade = curso.Cidade;
                    cursoAtualizacao.UF = curso.UF;
                    cursoAtualizacao.Disponivel = curso.Disponivel;

                    _context.Update(cursoAtualizacao);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException)
                {
                    if (!CursoExists(curso.CursoId))
                    {
                        return NotFound();
                    }
                    else 
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(curso);
        }

        private bool CursoExists(long? id)
        {
            return _context.Cursos.Any(e => e.CursoId == id);
        }

        [Route("detalhes-{id:long}")]
        public async Task<IActionResult>Details(long id)
        {
            var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.CursoId == id);

            _context.CursoTipos.Where(t => curso.CursoTipoId == t.CursoTipoId).Load();
            
            if(curso == null)
            {
                return NotFound();
            }
            return View(curso);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("Cursos", "Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var curso = await _context.Cursos.Include(t => t.CursoTipo).SingleOrDefaultAsync(c => c.CursoId == id);

            if(curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("Cursos", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var curso = await _context.Cursos.SingleOrDefaultAsync(c => c.CursoId == id);

            _context.Cursos.Remove(curso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        //private async Task<object> GetUser(object user)
        //{
        //    return await _userManager.GetUserAsync(User);
        //
        //    
        //}
        private async Task<bool> UserIsInRole(string role)
        {
            var user = await _userManager.GetUserAsync(User);

            if(!await _userManager.IsInRoleAsync(user, role))
            {
                return false;
            }

            return true;
        }

    }
}
