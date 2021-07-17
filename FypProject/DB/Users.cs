using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FypProject.DB
{
    public class Users
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Address { get; set; }
        public string Cnic { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string RecordBy { get; set; }
        public DateTime? RecordAt { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateAt { get; set; }
        public DateTime? DisableDate { get; set; }
        public int? FranchiseId { get; set; }

    }
}