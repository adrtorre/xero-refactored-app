using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XeroRefactoredApp.Exceptions
{
    public class InvalidArgumentException : Exception
    {
        public InvalidArgumentException(string message) : base(message) { }
    }
}
