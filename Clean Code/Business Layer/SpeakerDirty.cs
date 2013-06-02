using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business_Layer;
using DataAccessLayer;

namespace BusinessLayer
{
	/// <summary>
	/// Represents a single conference speaker
	/// </summary>
	public class SpeakerDirty
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public int? Exp { get; set; }
		public bool HasBlog { get; set; }
		public string BlogURL { get; set; }
		public WebBrowser Browser { get; set; }
		public List<string> Certifications { get; set; }
		public string Employer { get; set; }
		public List<BusinessLayer.Session> Sessions { get; set; }

		/// <summary>
		/// Register a speaker
		/// </summary>
		/// <returns>speakerID</returns>
		public int? Register(CreditCard cc)
		{
			//lets init some vars
			int? speakerID = null;
			bool good = false;
			bool appr = false;
			//string[] nt = new string[] {"MVC4", "Node.js", "CouchDB", "KendoUI", "Dapper"};
			string[] ot = new string[] { "Cobol", "Punch Cards", "Commodore", "VBScript" };

			if (!string.IsNullOrWhiteSpace(FirstName))
			{
				if (!string.IsNullOrWhiteSpace(LastName))
				{



					if (!string.IsNullOrWhiteSpace(Email))
					{
						//put list of employers in array
						string [] emps = new string [] {"Microsoft", "Google", "Fog Creek Software", "37Signals"};

						//DFCT #838 Jimmy 
						//We're now requiring 3 certifications so I changed the hard coded number. Boy, programming is hard.
						good = ((Exp > 10||HasBlog||Certifications.Count() > 3||emps.Contains(Employer)));
								
								
								
						if (!good)
						{
							//DEFECT #5274 DA 12/10/2012
							//We weren't filtering out the prodigy domain so I added it.
							string[] domains=new string [] {"aol.com", "hotmail.com", "prodigy.com", "CompuServe.com"};
							//need to get just the domain from the email
							string emailDomain=Email.Split('.').Last();

							if (!domains.Contains(emailDomain))
							{
								good = true;
							}


			//Here's some awful and deceiving indentation. I can't be bothered to properly format my code. Gotta get back to Facebook.
			else
			{
				throw new SpeakerDoesntMeetRequirementsException("User has email from the turn of the century.");
			}

							if (Browser.Name == WebBrowser.BrowserName.InternetExplorer && Browser.MajorVersion < 9)
							{
								throw new SpeakerDoesntMeetRequirementsException("You're using an old version of IE so we suspect you don't belong here.");
							}
						}

						if (good)
						{
							//DEFECT #5013 CO 1/12/2012
							//We weren't requiring at least one session
							if (Sessions.Count() == 0) throw new ArgumentException("Can't register speaker with no sessions to present.");
							foreach (var session in Sessions)
							{
								//foreach (var tech in nt)
								//{
								//    if (session.Title.Contains(tech))
								//    {
								//        session.Approved = true;
								//        break;
								//    }
								//}

								foreach (var tech in ot)
								{
									if (session.Title.Contains(tech))
									{
										session.Approved = false;
									}
								}

								foreach (var session2 in Sessions)
								{
									if (session2.Approved)
										appr = true;
								}

								if (appr)
								{
									//if we got this far, the speaker is approved
									//let's go ahead and register him/her now.
									//First, let's calculate the registration fee. 
									//More experienced speakers pay a lower fee.
									int registrationFee;

									if (Exp <= 1)
									{
										registrationFee = 50;
									}
									else if (Exp >= 2 && Exp <=3)
									{
										registrationFee = 25;
									}
									//else if (YearsExperience >= 4 && YearsExperience <=5)
									//{
									//    registrationFee = 10;
									//}
									else if (Exp >= 6 && Exp <=9)
									{
										registrationFee = 5;
									}
									else
									{
										registrationFee = 0;
									}


									
									//if (YearsExperience <= 1)
									//{
									//    registrationFee = 40;
									//}
									//else if (YearsExperience >= 4 && YearsExperience <= 5)
									//{
									//    registrationFee = 10;
									//}
									//else if (YearsExperience >= 6 && YearsExperience <= 9)
									//{
									//    registrationFee = 5;
									//}
									//else
									//{
									//    registrationFee = 0;
									//}


									if (registrationFee > 0)
									{
										cc.Charge(registrationFee);
									}

									//Now, save the speaker and sessions to the db.
									var speaker = new Speaker()
									{
										FirstName = FirstName,
										LastName = LastName,
										Email = Email,

										YearsExperience = Exp,
										Employer = Employer
									};

									try
									{
										Save(speaker);
									}
									catch(Exception e)
									{
										ErrorLog.Log(e);
										//db insert sometimes fails.
									}

									using (var db = new KCDC2012Entities())
									{
										foreach (var sessionTemp in Sessions)
										{
											db.Sessions.AddObject(new DataAccessLayer.Session()
											{
												Name = sessionTemp.Name,
												Description = sessionTemp.Description,
												Approved = sessionTemp.Approved
											});
										}
										db.SaveChanges();
									}

								}
								else
								{
									throw new NoSessionsApprovedException();
								}
							}
						}
						else
						{
							throw new SpeakerDoesntMeetRequirementsException("This speaker doesn't meet our abitrary and capricious standards.");
						}
					}
					else
					{
						throw new ArgumentNullException("Email is required.");
					}
				}
				else
				{
					throw new ArgumentNullException("Last name is required.");
				}
			}
			else
			{
				throw new ArgumentNullException("First Name is required");
			}

			//if we got this far, the speaker is registered.
			return speakerID;
		}

		public int? Save(Speaker speaker)
		{
			using (var db = new KCDC2012Entities())
			{
				db.Speakers.AddObject(speaker);
				db.SaveChanges();

				foreach (var sessionToSave in Sessions)
				{
					db.Sessions.AddObject(new DataAccessLayer.Session()
					{
						Name = sessionToSave.Name,
						Description = sessionToSave.Description,
						Approved = sessionToSave.Approved
					});
				}
				db.SaveChanges();

				return speaker.SpeakerID;
			}
		}

		#region Custom Exceptions
		public class SpeakerDoesntMeetRequirementsException : Exception
		{
			public SpeakerDoesntMeetRequirementsException(string message) : base(message) 
			{ 
			}
		}

		public class NoSessionsApprovedException : Exception
		{

		}
		#endregion
	}
}