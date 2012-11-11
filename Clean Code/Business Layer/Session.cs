using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
	/// <summary>
	/// Represents a single 1 hour session at KCDC
	/// </summary>
	public class Session
	{
		public string Name { get; set; }
		public string Title { get; set; }
		public string Description { get; set; }
		public bool Approved { get; set; }
		
		private bool _AboutOldTech;
		public bool AboutOldTech {
			get { return _AboutOldTech; }
		}

		public Session(string title, string description)
		{
			Title = title;
			Description = description;
			_AboutOldTech = SessionIsAboutOldTech(description);
		}

		private bool SessionIsAboutOldTech(string description)
		{
			string[] oldTechnologies = new string[] { "Cobol", "Punch Cards", "Commodore", "VBScript" };

			foreach (var oldTech in oldTechnologies)
			{
				if (description.Contains(oldTech)) return true;
			}
			return false;
		}

		private bool SessionIsAboutNewTech(string description)
		{
			string[] newTechnologies = new string[] { "MVC4", "Node.js", "CouchDB", "KendoUI", "Dapper" };

			foreach (var newTech in newTechnologies)
			{
				if (description.Contains(newTech)) return true;
			}
			return false;
		}
	}
}
