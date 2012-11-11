using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business_Layer;
using DataAccessLayer;

namespace BusinessLayer
{
	/// <summary>
	/// Represents a single speaker at KCDC
	/// </summary>
	public class SpeakerClean
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? YearsExperience { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public bool HasBeard { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public List<BusinessLayer.Session> Sessions { get; set; }

		/// <summary>
		/// Register a speaker for KCDC
		/// </summary>
		/// <returns>speakerID</returns>
		public int? Register(CreditCard creditCard)
		{
			ValidateRegistration();

			int registrationFee = GetRegistrationFee();

			if (registrationFee > 0) creditCard.Charge(registrationFee);

			int speakerID = SaveSpeakerToDB();
			return speakerID;
		}

		private void ValidateRegistration()
		{
			ValidateData();

			if (!SpeakerMeetsOurRequirements())
			{
				throw new SpeakerDoesntMeetRequirementsException("This speaker doesn't meet our abitrary and capricious standards.");
			}
			
			ApproveSessions();
		}

		private void ValidateData()
		{
			if (string.IsNullOrEmpty(FirstName)) throw new ArgumentNullException("First Name is required.");
			if (string.IsNullOrEmpty(LastName)) throw new ArgumentNullException("Last Name is required.");
			if (string.IsNullOrEmpty(Email)) throw new ArgumentNullException("Email is required.");
			if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");
		}

		private bool SpeakerMeetsOurRequirements()
		{
			return IsExceptionalOnPaper() || !ObviousRedFlags();
		}

		private bool IsExceptionalOnPaper()
		{
			if (HasBlog) return true;
			if (HasBeard && YearsExperience > 10) return true;
			if (Certifications.Count() > 3) return true;
			if (HasPrestigeousEmployment()) return true;
			return false;
		}

		private bool HasPrestigeousEmployment()
		{
			string[] prestigeousEmployers = new string[] { "Microsoft", "Google", "Fog Creek Software", "37Signals", "Self-employed", "Consultant", "Independent Contractor" };
			return prestigeousEmployers.Contains(Employer);
		}

		private bool ObviousRedFlags()
		{
			return HasAncientEmail() || UsesAwfulBrowser();
		}

		private bool HasAncientEmail()
		{
			string[] emailDomainBlacklist = new string[] { "aol.com", "hotmail.com", "prodigy.com", "compuserve.com" };
			string emailDomain = Email.Split('.').Last();
			return emailDomainBlacklist.Contains(emailDomain);
		}

		private void ApproveSessions()
		{
			foreach (var session in Sessions)
			{
				session.Approved = !SessionIsAboutOldTechnology(session.Description);
			}

			bool noSessionsApproved = Sessions.Where(s => s.Approved).Count() == 0;
			if (noSessionsApproved) throw new NoSessionsApprovedException();
		}

		private bool UsesAwfulBrowser()
		{
			return Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9;
		}

		private void SaveSessions()
		{
			using (var db = new KCDC2012Entities())
			{
				foreach (var session in Sessions)
				{
					db.Sessions.AddObject(new DataAccessLayer.Session()
					{
						Name = session.Name,
						Description = session.Description,
						Approved = session.Approved
					});
				}
				db.SaveChanges();
			}
		}
		
		private int GetRegistrationFee()
		{
			using (var db = new KCDC2012Entities())
			{
				return db.RegistrationFees
					.Where(s=>s.MinYearsExperience <= YearsExperience)
					.Where(s=>s.MaxYearsExperience <= YearsExperience)
					.Select(s=>s.Fee).First();
			}
		}

		private bool SessionIsAboutOldTechnology(string sessionDescription)
		{
			string[] oldTechnologies = new string[] { "Cobol", "Punch Cards", "Commodore", "VBScript" };
			foreach (var oldTech in oldTechnologies)
			{
				if (sessionDescription.Contains(oldTech)) return true;
			}
			return false;
		}

		private int SaveSpeakerToDB()
		{
			var speaker = new Speaker()
			{
				FirstName = FirstName,
				LastName = LastName,
				Email = Email,
				YearsExperience = YearsExperience,
				HasBeard = HasBeard,
				Employer = Employer
			};

			using (var db = new KCDC2012Entities())
			{
				db.Speakers.AddObject(speaker);
				db.SaveChanges();
			}
			SaveSessions();

			return speaker.SpeakerID;
		}

		#region Custom Exceptions
		public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message)
				: base(message)
			{
			}
		}

		public class NoSessionsApprovedException : Exception
		{

		}
		#endregion
	}
}