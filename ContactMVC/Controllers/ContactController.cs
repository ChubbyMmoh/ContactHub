using ContactApi.Model;
using ContactApi.Model.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;

namespace ContactBookMVC.Controllers
{
	public class ContactController : Controller
	{
        string baseAddress = string.Empty;

		private readonly HttpClient _httpClient;
		//private readonly IConfiguration _configuration;
		public ContactController(IConfiguration configuration)
		{
			_httpClient = new HttpClient();
			baseAddress = configuration.GetSection("ExternalUrl:ContactBookApi").Value;
			_httpClient.BaseAddress = new Uri(baseAddress);
		}

		[HttpGet]
		public async Task<IActionResult> ContactList()
		{
			List<Contact> contacts = new List<Contact>();

			HttpResponseMessage response = await _httpClient.GetAsync(_httpClient.BaseAddress + "/Contact");

			if (response.IsSuccessStatusCode)
			{
				var data = await response.Content.ReadAsStringAsync();
				contacts = JsonConvert.DeserializeObject<List<Contact>>(data);
			}

			return View(contacts);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(Contact contact)
		{
			try
			{
				string data = JsonConvert.SerializeObject(contact);
				StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

				HttpResponseMessage response = await _httpClient.PostAsync(_httpClient.BaseAddress + "/Contact", content);

				if (response.IsSuccessStatusCode)
				{
					TempData["successMessage"] = "Contact Created Successfully";
					return RedirectToAction("ContactList");
				}
				else
				{
					TempData["errorMessage"] = "Contact creation failed. Please check your input.";
				}
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
			}

			return View();
		}
		[HttpGet]
		public async Task<IActionResult> Edit(Guid id)
		{
			try
			{
				Contact contact = new Contact();

				// Fetch the contact details by ID
				HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Contact/{id}");

				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					contact = JsonConvert.DeserializeObject<Contact>(data);

					if (contact != null)
					{
						TempData["successMessage"] = "Contact updated successfully.";
						return View(contact);
					}
				}

				// Handle the case where the contact is not found
				return NotFound();
			}
			catch (Exception ex)
			{
				// Handle any exceptions that occur during the retrieval of contact data
				// You can log the exception or display an error message to the user
				TempData["errorMessage"] = "An error occurred while retrieving the contact data.";
				return RedirectToAction("ContactList");
			}
		}

		[HttpPost]
		public async Task<IActionResult> Edit(Guid id, Contact contact)
		{
			if (ModelState.IsValid)
			{
				try
				{
					string data = JsonConvert.SerializeObject(contact);
					StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await _httpClient.PutAsync($"{_httpClient.BaseAddress}/Contact/{id}", content);

					if (response.IsSuccessStatusCode)
					{
						TempData["successMessage"] = "Contact Updated Successfully";
						return RedirectToAction("ContactList");
					}
					else
					{
						TempData["errorMessage"] = "Contact update failed. Please check your input.";
					}
				}
				catch (Exception ex)
				{
					TempData["errorMessage"] = ex.Message;
				}
			}
			else
			{
				TempData["errorMessage"] = "Validation failed. Please check your input.";
			}

			return View(contact);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(Guid id)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.GetAsync($"{_httpClient.BaseAddress}/Contact/{id}");

				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					Contact contact = JsonConvert.DeserializeObject<Contact>(data);

					if (contact != null)
					{
						TempData["successMessage"] = "Are you sure you want to delete this contact?";
						return View(contact);
					}
				}
				return NotFound();
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return RedirectToAction("ContactList");
			}
		}

		[HttpPost, ActionName("Delete")]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{
			try
			{
				HttpResponseMessage response = await _httpClient.DeleteAsync($"{_httpClient.BaseAddress}/Contact/{id}");

				if (response.IsSuccessStatusCode)
				{
					TempData["successMessage"] = "Contact Deleted Successfully";
				}
				else
				{
					TempData["errorMessage"] = "Contact deletion failed. Please try again.";
				}
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
			}

			return RedirectToAction("ContactList");
		}

		[HttpGet]
		public async Task<IActionResult> SearchContacts(string searchTerm, int page = 1, int pageSize = 10)
		{
			try
			{
				if (string.IsNullOrEmpty(searchTerm))
				{
					// If no search term is provided, simply return an empty view
					return View(new List<ContactResponseDTO>());
				}

				var queryParams = new Dictionary<string, string>
		{
			{ "searchTerm", searchTerm },
			{ "page", page.ToString() },
			{ "pageSize", pageSize.ToString() }
		};

				var requestUri = QueryHelpers.AddQueryString("api/Contact/search", queryParams);
				HttpResponseMessage response = await _httpClient.GetAsync(requestUri);

				if (response.IsSuccessStatusCode)
				{
					var data = await response.Content.ReadAsStringAsync();
					var contactResponseDTOs = JsonConvert.DeserializeObject<List<ContactResponseDTO>>(data);
					return View(contactResponseDTOs);
				}
				else
				{
					TempData["errorMessage"] = "Failed to retrieve contacts. Please try again.";
					return View();
				}
			}
			catch (Exception ex)
			{
				TempData["errorMessage"] = ex.Message;
				return View();
			}
		}

	}
}
