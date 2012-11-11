using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BusinessLayer
{
	public class CreditCard
	{
		public int Number { get; set; }
		public int ExpirationMonth { get; set; }
		public int ExpirationYear { get; set; }
		public int CVV2 { get; set; }
		private bool _Expired;
		private bool _ValidCardNumber;

		public CreditCard(int number, int expirationMonth, int expirationYear, int cvv2)
		{
			Number = number;
			ExpirationMonth = expirationMonth;
			ExpirationYear = expirationYear;
			CVV2 = cvv2;
		}

		public bool Charge(int amount)
		{
			if (!_ValidCardNumber) throw new InvalidCreditCardNumberException();
			if (!_Expired) throw new ExpiredCreditCardException();
			if (!ChargeCard(amount)) throw new CreditCardDeclinedException();
			return true;
		}

		private bool ChargeCard(int amount)
		{
			throw new NotImplementedException();
		}

		public class InvalidCreditCardNumberException : Exception
		{

		}

		public class ExpiredCreditCardException : Exception
		{

		}

		public class CreditCardDeclinedException : Exception
		{

		}
	}
}
