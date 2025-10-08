using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidDocumentException(string error) : DomainException(error);
}