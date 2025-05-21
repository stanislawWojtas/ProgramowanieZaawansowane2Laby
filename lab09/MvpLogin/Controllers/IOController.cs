
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MvpLogin.Models;
public class IOController : Controller
{
	public IActionResult Login() => View();

	[HttpPost]
	public IActionResult Login(IFormCollection form)
	{
		string? username = form["username"];
		string? password = form["password"];

		if (DatabaseHelper.CheckLogin(username, password))
		{
			HttpContext.Session.SetString("logged", "true");
			HttpContext.Session.SetString("username", username);
			return RedirectToAction("Successful");
		}
		ViewBag.Error = "Wrong username or password";
		return View();
	}

	public IActionResult Successful()
	{
		if (HttpContext.Session.GetString("logged") == "true")
		{
			return View();
		}
		return RedirectToAction("Login");
	}

	[HttpPost]
	public IActionResult LogOut()
	{
		HttpContext.Session.Clear();
		return RedirectToAction("Login");
	}


	public IActionResult Register() => View();

	[HttpPost]
	public IActionResult Register(IFormCollection form)
	{
		string? username = form["username"];
		string? password = form["password"];

		if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
		{
			ViewBag.Error = "Username and password are required";
			return View();
		}

		if (DatabaseHelper.AddUser(username, password))
		{
			ViewBag.Message = "Registration succesfull. You can log in.";
			return View();
		}
		else
		{
			ViewBag.Error = "Username already exist";
			return View();
		}
	}


}