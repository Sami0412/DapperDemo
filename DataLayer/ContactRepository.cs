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