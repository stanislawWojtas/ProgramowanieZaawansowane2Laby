using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using MvpLogin.Models;

public class ProductController : Controller
{
	public IActionResult Index()
	{
		var products = DatabaseHelper.GetAllProducts();
		ViewBag.Products = products;
		return View();
	}

	public IActionResult Add() => View();

	[HttpPost]
	public IActionResult Add(IFormCollection form)
	{
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