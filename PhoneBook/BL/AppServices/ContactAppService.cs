using BL.Bases;
using BL.Interfaces;
using BL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BL.AppServices
{
   public class ContactAppService : BaseAppServices
    {
        public ContactAppService(IUnitOfWork theUnitOfWork) : base(theUnitOfWork)
        {
        }
        public IEnumerable<ContactViewModel> GetAllContact()
        {
            IEnumerable<Contact> allcontacts = TheUnitOfWork.Contact.GetAllContacts();
            return Mapper.Map<IEnumerable<ContactViewModel>>(allcontacts);
        }
        
       
      
       

      
        public ContactViewModel GetContact(int id)
        {
            return Mapper.Map<ContactViewModel>(TheUnitOfWork.Contact.GetContactById(id));
        }
        public bool UpdateContact(ContactViewModel contactViewModel)
        {
            var pro = Mapper.Map<Contact>(contactViewModel);
            TheUnitOfWork.Contact.Update(pro);
            TheUnitOfWork.Commit();
            return true;
        }

        public bool AddNewContact(ContactViewModel contactViewModel)
        {
            if (contactViewModel == null)
                throw new ArgumentNullException();
            bool result = false;
            var contact = Mapper.Map<Contact>(contactViewModel);
            if (TheUnitOfWork.Contact.Insert(contact))
            {
                result = TheUnitOfWork.Commit() > new int();
            }
            return result;
        }

        public bool DeleteContact(int id)
        {
            bool result = false;
            TheUnitOfWork.Contact.Delete(id);
            result = TheUnitOfWork.Commit() > new int();

            return result;
        }

        public bool CheckContactExists(ContactViewModel contactViewModel)
        {
            Contact contact = Mapper.Map<Contact>(contactViewModel);
            return TheUnitOfWork.Contact.CheckContactExists(contact);
        }
       
      
    }
}
