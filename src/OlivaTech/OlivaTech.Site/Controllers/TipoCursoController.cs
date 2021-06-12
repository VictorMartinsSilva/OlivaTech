using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Route("admin/formacao")]
    public class TipoCursoController : Controller
    {
        private readonly ContextDb _context;
        public TipoCursoController(ContextDb context)
        {
            _context = context;
        }

        [ClaimsAuthorize("TipoCursos", "View")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.CursoTipos.OrderBy(p => p.Nome).ToListAsync());
        }

        [Route("adicionar")]
        [ClaimsAuthorize("TipoCursos", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("adicionar")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("TipoCursos", "Create")]
        public async Task<IActionResult> Create(CursoTipo tipoCurso)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Add(tipoCurso);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(tipoCurso);
        }

        [Route("editar-{id:long}")]
        [ClaimsAuthorize("TipoCursos", "Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var tipoCurso = await _context.CursoTipos.FindAsync(id);

            if(tipoCurso == null)
            {
                return NotFound();
            }

            return View(tipoCurso);
        }

        [HttpPost]
        [Route("editar-{id:long}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("TipoCursos", "Edit")]
        public async Task<IActionResult> Edit(long id, CursoTipo cursoTipo)
        {
            if(id != cursoTipo.CursoTipoId)
            {
                return NotFound();
            }

            var tipoAtualizacao = await _context.CursoTipos.FindAsync(id);

            if(ModelState.IsValid)
            {
                try 
                {
                    tipoAtualizacao.Nome = cursoTipo.Nome;

                    _context.Update(tipoAtualizacao);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!TipoExists(cursoTipo.CursoTipoId))
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
            return View(cursoTipo);
        }

        private bool TipoExists(long? id)
        {
            return _context.CursoTipos.Any(e => e.CursoTipoId == id);
        }

        [Route("detalhes-{id:long}")]
        [ClaimsAuthorize("TipoCursos", "Details")]
        public async Task<IActionResult> Details(long id)
        {
          
            var tipoCurso = await _context.CursoTipos.SingleOrDefaultAsync(t => t.CursoTipoId == id);

            if(tipoCurso == null)
            {
                return NotFound();
            }

            return View(tipoCurso);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("TipoCursos", "Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            
            var tipoCurso = await _context.CursoTipos.SingleOrDefaultAsync(t => t.CursoTipoId == id);
            
            if(tipoCurso == null)
            {
                return NotFound();
            }

            return View(tipoCurso);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("TipoCursos", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var tipoCurso = await _context.CursoTipos.SingleOrDefaultAsync(t => t.CursoTipoId == id);

            _context.CursoTipos.Remove(tipoCurso);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
