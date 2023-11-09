using ContactMVC.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace ContactBookMVC.Controllers
{
	public class AuthController : Controller
	{
		Uri baseAddress = new Uri("https://localhost:7250/api"); 
		private readonly HttpClient _httpClient;

        public AuthController()
		{
			_httpClient = new HttpClient();
			_httpClient.BaseAddress = baseAddress;
        }

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
		{
			try
			{
				string data = JsonConvert.SerializeObject(registerViewModel);
				StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Auth/register", content);

				if (response.IsSuccessStatusCode)
				{
					TempData["successMessage"] = "Registration Successful";
					// Handle a successful registration, e.g., redirect to a success page or login page
					return RedirectToAction("Login");
				}
				else
				{
					TempData["errorMessage"] = "Registration failed. Please check your input.";
				}
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
			}

			return View();
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                string data = JsonConvert.SerializeObject(loginViewModel);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync($"{_httpClient.BaseAddress}/Auth/login", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var token = JsonConvert.DeserializeAnonymousType(responseData, new { token = "" });

                    if (!string.IsNullOrEmpty(token.token))
                    {
                        // Store the token and implement user authentication
                        TempData["successMessage"] = "Login Successful";

                        // Check if there is a return URL
                        if (!string.IsNullOrWhiteSpace(loginViewModel.ReturnUrl))
                        {
                            return Redirect(loginViewModel.ReturnUrl); // Redirect to the return URL
                        }

                        return RedirectToAction("Index", "Home"); // Redirect to your home page or dashboard
                    }
                    else
                    {
                        TempData["errorMessage"] = "Authentication failed";
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Login failed. Please check your input.";
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
            }

            return View();
        }

        //[HttpPost]
        ////[Authorize] // This action is only accessible to authenticated users
        //public async Task<IActionResult> Logout()
        //{
        //    // Sign the user out to clear their authentication token
        //    await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

        //    // Redirect to the login page or any other appropriate page
        //    return RedirectToAction("Login");
        //}

    }
}
