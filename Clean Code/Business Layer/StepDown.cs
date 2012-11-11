using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business_Layer
{
	class StepDownExample
	{
		public StepDownExample()
		{
		    Method1();
			Method2();
			Method3();
		}

		private void Method1()
		{
			Method1Submethod1();
			Method1Submethod2();
		}

		private void Method1Submethod1()
		{
			//logic here...
		}

		private void Method1Submethod2()
		{
			//logic here...
		}

		private void Method2()
		{
			//logic here...
		}

		private void Method3()
		{
			Method3Submethod1();
			Method3Submethod2();
		}

		private void Method3Submethod1()
		{
			//logic here...
		}

		private void Method3Submethod2()
		{
			//logic here...
		}
	}
}
