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
    [Route("ofertas")]
    public class OfertaController : Controller
    {
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly ContextDb _context;
        public OfertaController(ContextDb context,
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

            ViewData["TermSearch"] = searchTerm;
            var oferta = from c in _context.Ofertas select c;

            if (!String.IsNullOrEmpty(searchTerm))
            {
                oferta = oferta.Where(o => o.Curso.Nome.Contains(searchTerm) || o.Curso.Cidade.Contains(searchTerm));

                if(user == null)
                {
                    return View(await oferta.Include(c => c.Curso).Where(c => c.Disponivel == true).OrderBy(p => p.Curso.Nome).ToListAsync());
                }
                else if(await UserIsInRole("Admin") || await UserIsInRole("Ceo"))
                {
                    return View(await oferta.Include(c => c.Curso).OrderBy(c => c.Curso.Nome).ToListAsync());
                }

                return View(await _context.Ofertas.Include(c => c.Curso).Where(c => c.Disponivel == true).OrderBy(c => c.Curso.Nome).ToListAsync());
            }
            else
            {
                if(user == null)
                {
                    return View(await _context.Ofertas.Include(c => c.Curso).Where(o => o.Disponivel == true).OrderBy(c => c.Curso.Nome).ToArrayAsync());
                }
                else if(await UserIsInRole("Admin") || await UserIsInRole("Ceo"))
                {
                    return View(await _context.Ofertas.Include(c => c.Curso).OrderBy(c => c.Curso.Nome).ToListAsync());
                }

                return View(await _context.Ofertas.Include(c => c.Curso).Where(c => c.Disponivel == true).OrderBy(c => c.Curso.Nome).ToListAsync());
            }
        }

        [Route("adicionar")]
        [ClaimsAuthorize("Ofertas", "Create")]
        public IActionResult Create()
        {
            var cursos = _context.Cursos.OrderBy(c => c.Nome).ToList();
            cursos.Insert(0, new Curso() { CursoId = 0, Nome = "[Selecione o Curso]" });
            ViewBag.Cursos = cursos;

            return View();
        }

        [HttpPost]
        [Route("adicionar")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Ofertas", "Create")]
        public async Task<IActionResult> Create(Oferta oferta)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    _context.Ofertas.Add(oferta);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(oferta);
        }

        [Route("editar-{id:long}")]
        [ClaimsAuthorize("Ofertas", "Edit")]
        public async Task<IActionResult> Edit(long id)
        {
            var oferta = await _context.Ofertas.FindAsync(id);

            if(oferta == null)
            {
                return NotFound();
            }
            ViewBag.Cursos = new SelectList(_context.Cursos.OrderBy(c => c.CursoId), "CursoId", "Nome", oferta.CursoId);

            return View(oferta);
        }

        [HttpPost]
        [Route("editar-{id:long}")]
        [ValidateAntiForgeryToken]
        [ClaimsAuthorize("Ofertas", "Edit")]
        public async Task<IActionResult> Edit(long id, Oferta oferta)
        {
            if(id != oferta.OfertaId)
            {
                return NotFound();
            }

            var ofertaAtualizacao = await _context.Ofertas.FindAsync(id);

            if(ModelState.IsValid)
            {
                try
                {
                    ofertaAtualizacao.CursoId = oferta.CursoId;
                    ofertaAtualizacao.Disponivel = oferta.Disponivel;
                    ofertaAtualizacao.Preco = oferta.Preco;

                    _context.Update(ofertaAtualizacao);
                    await _context.SaveChangesAsync();
                }
                catch(DbUpdateException)
                {
                    if(!OfertaExists(oferta.OfertaId))
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
            return View(oferta);
        }

        private bool OfertaExists(long? id)
        {
            return _context.Ofertas.Any(o => o.OfertaId == id);
        }

        [Route("detalhes-{id:long}")]
        public async Task<IActionResult> Details(long id)
        {
            var oferta = await _context.Ofertas.SingleOrDefaultAsync(o => o.OfertaId == id);

            _context.Cursos.Where(c => oferta.CursoId == c.CursoId).Load();

            if(oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("Ofertas", "Delete")]
        public async Task<IActionResult> Delete(long id)
        {
            var oferta = await _context.Ofertas.Include(c => c.Curso).SingleOrDefaultAsync(c => c.OfertaId == id);

            if(oferta == null)
            {
                return NotFound();
            }
            return View(oferta);
        }

        [Route("excluir-{id:long}")]
        [ClaimsAuthorize("Ofertas", "Delete")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var oferta = await _context.Ofertas.SingleOrDefaultAsync(o => o.OfertaId == id);

            _context.Ofertas.Remove(oferta);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UserIsInRole(string role)
        {
            var user = await _userManager.GetUserAsync(User);

            if (!await _userManager.IsInRoleAsync(user, role))
            {
                return false;
            }

            return true;
        }
    }
}
