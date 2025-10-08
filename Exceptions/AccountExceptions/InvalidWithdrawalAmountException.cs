using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.Exceptions.AccountExceptions
{
    public class InvalidWithdrawalAmountException(string error) : DomainException(error);
}