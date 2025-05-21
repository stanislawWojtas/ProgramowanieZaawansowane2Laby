using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using MvpLogin.Models;

public class ProductController : Controller
{
	public IActionResult Index()
	{
		if (HttpContext.Session.GetString("logged") != "true")
		{
			return RedirectToAction("Login", "IO");
		}
		var products = DatabaseHelper.GetAllProducts();
		ViewBag.Products = products;
		return View();
	}

	public IActionResult Add() {
		if (HttpContext.Session.GetString("logged") != "true")
		{
			return RedirectToAction("Index");
		}
		return View();
	}

	[HttpPost]
	public IActionResult Add(IFormCollection form)
	{
		if (HttpContext.Session.GetString("logged") != "true")
		{
			return RedirectToAction("Index");
		}
		string? title = form["title"];
		string? author = form["author"];
		string? year = form["year"];

		if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(author))
		{
			ViewBag.Error = "Author and Title are required";
			return View();
		}
		var product = new Product
		{
			Title = title,
			Author = author,
			Year = year
		};
		DatabaseHelper.AddProduct(product);
		return RedirectToAction("Index");
	}
}