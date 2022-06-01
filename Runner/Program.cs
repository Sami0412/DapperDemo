using DataLayer;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics;

namespace Runner
{
    class Program
    {
        private static IConfigurationRoot config;
        
        static void Main(string[] args)
        {
            Initialise();
            
            //Get_all_should_return_6_results();
            
            var id =Insert_should_assign_identity_to_new_entity();
            Find_should_retrieve_existing_entity(id);
            Modify_should_update_existing_entity(id);
            Delete_should_remove_entity(id);
        }

        static void Delete_should_remove_entity(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();
            
            //act
            repository.Remove(id);
            
            //create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var deletedEntity = repository2.Find(id);

            //assert
            Debug.Assert(deletedEntity == null);
            Console.WriteLine("*** Contact Deleted ***");

        }
        static void Modify_should_update_existing_entity(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();
            
            //act
            var contact = repository.Find(id);
            contact.FirstName = "Billy";
            repository.Update(contact);
            
            //create a new repository for verification purposes
            IContactRepository repository2 = CreateRepository();
            var modifiedContact = repository2.Find(id);
            
            //assert
            Console.WriteLine("*** Contact Modified ***");
            modifiedContact.Output();
            Debug.Assert(modifiedContact.FirstName == "Billy");

        }
        static void Find_should_retrieve_existing_entity(int id)
        {
            //arrange
            IContactRepository repository = CreateRepository();
            
            //act
            var contact = repository.Find(id);
            
            //assert
            Console.WriteLine("*** Get Contact ***)");
            contact.Output();
            Debug.Assert(contact.FirstName == "Greg");
            Debug.Assert(contact.LastName == "Bingham");
        }
        static int Insert_should_assign_identity_to_new_entity()
        {
            //arrange
            IContactRepository repository = CreateRepository();
            var contact = new Contact()
            {
                FirstName = "Greg",
                LastName = "Bingham",
                Email = "email@gmail.com",
                Company = "Microsoft",
                Title = "Developer"
            };
            
            //act
            repository.Add(contact);
            
            //assert
            Debug.Assert(contact.Id != 0);
            Console.WriteLine("*** Contact Inserted ***");
            Console.WriteLine($"New Id: {contact.Id}");
            return contact.Id;
        }
        
        static void Get_all_should_return_6_results()
        {
            //arrange
            var repository = CreateRepository();
            //act
            var contacts = repository.GetAll();
            //assert
            Console.WriteLine($"Count: {contacts.Count}");
            Debug.Assert(contacts.Count == 6);
            contacts.Output();
        }

        private static void Initialise()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            config = builder.Build();
        }

        private static IContactRepository CreateRepository()
        {
            // return new ContactRepository(config.GetConnectionString("DefaultConnection"));
            return new ContactRepositoryContrib(config.GetConnectionString("DefaultConnection"));
        }
    }
}