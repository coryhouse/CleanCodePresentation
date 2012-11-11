using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PresentationLayer.Models;
using BusinessLayer;

namespace PresentationLayer.Controllers
{
    public class RegisterController : Controller
    {
		//
		// GET: /Register/

		public ActionResult Index()
		{
			var model = new RegisterModel();

			model.Sessions.Add(new SessionModel()
				{
					Number = 1
				});

			model.Certifications.Add("");
	
			return View(model);
		}

		//
		// POST: /Register/
		[HttpPost]
		public ActionResult Index(RegisterModel model)
		{
			if (ModelState.IsValid)
			{
				var speaker = new SpeakerClean()
				{
					FirstName = model.FirstName,
					LastName = model.LastName,
					Email = model.Email,
					Employer = model.Employer,
					YearsExperience = model.YearsExperience,
					BlogURL = model.BlogURL,
					HasBeard = model.HasBeard,
					Browser = new Business_Layer.WebBrowser(),
					Certifications = new List<string>(),
					Sessions = new List<Session>()
				};

				foreach (var certification in model.Certifications)
				{
					speaker.Certifications.Add(certification);
				}

				foreach (var session in model.Sessions)
				{
					speaker.Sessions.Add(new BusinessLayer.Session(session.Name, session.Description));
				}

				speaker.Register(new CreditCard(5,5,5,5));

				return View("Thanks");
			}
			return View(model);
		}
    }
}
