using BL.Bases;
using DAL;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BL.Repositories
{
   public class ContactRepository : BaseRepository<Contact>
    {
        private ApplicationDbContext EC_DbContext;
        public ContactRepository(ApplicationDbContext EC_DbContext) : base(EC_DbContext)
        {
            this.EC_DbContext = EC_DbContext;
        }

        public List<Contact>GetAllContacts()
        {
            return GetAll().ToList();
        }

        public bool InsertContact(Contact contact)
        {
            return Insert(contact);
        }
        public void UpdateContact(Contact contact)
        {
            Update(contact);
        }
        public void DeleteContact(int id)
        {
            Delete(id);
        }
        public bool CheckContactExists(Contact contact)
        {
            return GetAny(C =>C.ContactID==contact.ContactID);
        }
     
        public Contact GetContactById(int id)
        {
            return GetFirstOrDefault(c => c.ContactID == id);
        }
      
    }
}
