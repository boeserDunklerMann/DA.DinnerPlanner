using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DA.DinnerPlanner.Model.Exceptions
{
    class DeleteReferenceException(string? errorMessage) : Exception
    {
		public string? ErrorMessage { get => errorMessage; }
	}
}
