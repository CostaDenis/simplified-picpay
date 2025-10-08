using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidAccountTypeException(string error) : DomainException(error);
}