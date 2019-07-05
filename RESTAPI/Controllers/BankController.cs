using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RESTAPI.Models;

namespace RESTAPI.Controllers
{
    public class BankController : ApiController
    {
        // GET: api/Bank
        public IEnumerable<BankInfo> Get()
        {
            var bankInfoList = new List<BankInfo>();
            for (int i = 0; i < 10; i++)
            {
                var bankInfo = new BankInfo
                {
                    username = $"user {i}",
                    password = $"{i}",
                    currencyAmount = i / 10 * 4500
                };

                bankInfoList.Add(bankInfo);
            }
            return bankInfoList;
        }

        // GET: api/Bank/5
        public BankInfo Get(int id)
        {
            return new BankInfo
            {
                username = $"user {id}",
                password = $"{id}",
                currencyAmount = id / 10 * 4500
            };
        }
    }
}
