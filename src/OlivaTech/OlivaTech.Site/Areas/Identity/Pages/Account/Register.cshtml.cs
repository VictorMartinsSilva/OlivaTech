using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using OlivaTech.Site.Models;

namespace OlivaTech.Site.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUserCustom> _signInManager;
        private readonly UserManager<IdentityUserCustom> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;

        public RegisterModel(
            UserManager<IdentityUserCustom> userManager,
            SignInManager<IdentityUserCustom> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required(ErrorMessage = "O {0} é obrigatório.")]
            [Display(Name = "Nome")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
            public string Nome { get; set; }

            [Display(Name = "Sobrenome")]
            public string Sobrenome { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "A {0} é obrigatória.")]
            [Display(Name = "Cidade")]
            [StringLength(100, MinimumLength = 2, ErrorMessage = "A {0} deve conter de {2} a {1} caracteres.")]
            public string Cidade { get; set; }

            [Required(ErrorMessage = "O {0} é obrigatório.")]
            [Display(Name = "Bairro")]
            [StringLength(120, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
            public string Bairro { get; set; }

            [Required(ErrorMessage = "O {0} é obrigatório.")]
            [Display(Name = "UF")]
            [StringLength(2, MinimumLength = 2, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
            public string UF { get; set; }

            [Required(ErrorMessage = "O {0} é obrigatório.")]
            [Display(Name = "CEP")]
            [StringLength(9, MinimumLength = 8, ErrorMessage = "O {0} deve conter de {2} a {1} caracteres.")]
            public string CEP { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new IdentityUserCustom { UserName = Input.Email, Email = Input.Email, Nome = Input.Nome, Sobrenome = Input.Sobrenome, Bairro = Input.Bairro, Cidade = Input.Cidade, UF = Input.UF, CEP = Input.CEP };
                    
                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
