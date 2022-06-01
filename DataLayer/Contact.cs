using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace DataLayer
{
    public class Contact
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Company { get; set; }
        public string Title { get; set; }
        
        //when using Dapper.Contrib we can't choose which properties to use with SQL statement
        //IsNew is a computed field that won't exist in the db, so we need to use Computed attribute
        [Computed]
        public bool IsNew => this.Id == default(int);
        //False Write attribute means Dapper.Contrib won't attempt to add this to the db (there is no column called Addresses)
        [Write(false)]
        public List<Address> Addresses { get; } = new List<Address>();
    }
}