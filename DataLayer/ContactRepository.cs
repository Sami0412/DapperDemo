using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DataLayer
{
    public class ContactRepository : IContactRepository
    {
        private IDbConnection db;
        public ContactRepository(string connString)
        {
            db = new SqlConnection(connString);
        }
        public Contact Add(Contact contact)
        {
            var sql =
                "INSERT INTO Contacts (FirstName, LastName, Email, Company, Title) VALUES (@FirstName, @LastName, @Email, @Company, @Title); " +
                "SELECT CAST(SCOPE_IDENTITY() as int)";
                var id = db.Query<int>(sql, contact).Single();
                contact.Id = id;
                return contact;
        }

        public Contact Find(int id)
        {
            return db.Query<Contact>("SELECT * FROM Contacts WHERE Id = @Id", new {id}).SingleOrDefault();
        }

        public List<Contact> GetAll()
        {
            return db.Query<Contact>("SELECT * FROM Contacts").ToList();
        }

        public void Remove(int id)
        {
            db.Execute("DELETE FROM Contacts WHERE Id = @Id", new {id});
        }

        public Contact GetFullContact(int id)
        {
            var sql =
                "SELECT * FROM Contacts WHERE Id = @Id; " +
                "SELECT * FROM Addresses WHERE ContactId = @Id";
            
            using (var multipleResults = db.QueryMultiple(sql, new { Id = id }))
            {
                //QueryMultiple Dapper method - expects multiple result sets from the query
                //returns a GridReader object that we have named multipleResults
                //Call Read method on this object multiple times - once to get the contact object, and again to get any associated addresses
                var contact = multipleResults.Read<Contact>().SingleOrDefault();

                var addresses = multipleResults.Read<Address>().ToList();

                //if neither are null, add all the addresses to the Contact object's Addresses property
                if (contact != null && addresses != null)
                {
                    contact.Addresses.AddRange(addresses);
                }

                return contact;
            }
        }

        public Contact Update(Contact contact)
        {
            var sql =
                "UPDATE Contacts " +
                "SET FirstName = @FirstName, " +
                "    LastName  = @LastName, " +
                "    Email     = @Email, " +
                "    Company   = @Company, " +
                "    Title     = @Title " +
                "WHERE Id = @Id";
            db.Execute(sql, contact);
            return contact;
        }
    }
}