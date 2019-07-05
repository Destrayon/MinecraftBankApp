using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace RESTAPI.Models
{
    [DataContract]
    public class BankInfo
    {
        [DataMember(Name = "username")]
        public string username { get; set; }
        [DataMember(Name = "password")]
        public string password { get; set; }
        [DataMember(Name = "amount")]
        public string currencyAmount { get; set; }
    }
}