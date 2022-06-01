using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper.Contrib.Extensions;

namespace DataLayer;

public class ContactRepositoryContrib : IContactRepository
{
    private IDbConnection db;
    
    public ContactRepositoryContrib(string connString)
    {
        db = new SqlConnection(connString);
    }
    
    //Dapper.Contrib is useful for simple SQL queries - straight CRUD operations (no mapping of column names/customisation)
    //Don't have to manually specify SQL code
    public Contact Find(int id)
    {
        return db.Get<Contact>(id);
    }

    public List<Contact> GetAll()
    {
        //Dapper.Contrib GetAll() method does a SELECT * on Contacts table
        return db.GetAll<Contact>().ToList();
    }

    public Contact Add(Contact contact)
    {
        //Dapper.Contrib Insert method generates standard Insert statement based on Contact object, and returns new Id
        var id = db.Insert(contact);
        //Assign new Id to id of contact object
        contact.Id = (int) id;
        return contact;
    }

    public Contact Update(Contact contact)
    {
        db.Update(contact);
        return contact;
    }

    public void Remove(int id)
    {
        //Dapper.Contrib delete method needs to know what object to expect
        //Create temporary Contact object as an intermediary
        db.Delete(new Contact {Id = id});
    }
}