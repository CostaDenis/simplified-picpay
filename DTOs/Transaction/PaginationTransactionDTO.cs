using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace simplified_picpay.DTOs.Transaction
{
    public class PaginationTransactionDTO
    {
        public int Skip { get; set; } = 0;
        public int Take { get; set; } = 10;
    }
}