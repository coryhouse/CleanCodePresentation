using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
	/// <summary>
	/// Represents a single attendee to KCDC
	/// </summary>
	public class Attendee
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Email { get; set; }
		public DateTime RegistrationDate { get; set; }
		public int Age { get; set; }
	}
}
