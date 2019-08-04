using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;

namespace Vidly.Controllers
{
	public class CustomerController : Controller
	{


		private ApplicationDbContext _context;


		public CustomerController()
		{
			_context = new ApplicationDbContext();
		}


		protected override void Dispose(bool disposing)
		{
			_context.Dispose();
		}

		//
		// GET: /Customer/
		public ActionResult Index()
		{

			//var customers = _context.Customers.Include(c => c.MembershipType).ToList();
		  
			return View();
		}

		

		public ActionResult Details(int id)
		{
			Customer customer = _context.Customers.Include(m => m.MembershipType).FirstOrDefault(c => c.Id == id);

			if (customer != null) return View(customer);

			else return HttpNotFound();
		}


		public ActionResult New()
		{
			var membershipTypes = _context.MembershipTypes.ToList();

			var viewModel = new CustomerForViewModel
			{
				Customer = new Customer(),
				MembershipType = membershipTypes
			};

			return View(viewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult Create(Customer customer)
		{

			if (!ModelState.IsValid)
			{
				var viewModel = new CustomerForViewModel
				{
					Customer = customer,
					MembershipType = _context.MembershipTypes.ToList()
				};

				return View("New", viewModel);
			}

			_context.Customers.Add(customer);

			_context.SaveChanges();

			return RedirectToAction("Index");
		}

		public ActionResult Edit(int id)
		{
			var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

			if (customer == null) return HttpNotFound();

			var viewModel = new CustomerForViewModel
			{
				Customer = customer,
				MembershipType = _context.MembershipTypes.ToList()
			};

			return View(viewModel);
		}

		[HttpPost]
        [ValidateAntiForgeryToken]
		public ActionResult EditCustomer(Customer customer)
		{
			var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

			//TryUpdateModel(customerInDb);

			customerInDb.Name = customer.Name;
			customerInDb.DateOfBirth = customer.DateOfBirth;
			customerInDb.MembershipTypeId = customer.MembershipTypeId;
			customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;

			_context.SaveChanges();

			return RedirectToAction("Index");
		}
	}
}