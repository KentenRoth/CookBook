using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CookBook.DTOs.Account.Response
{
    public class RefreshDto
    {
        public required string AccessToken { get; set; }
    }
}